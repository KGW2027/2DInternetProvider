using System;
using System.Collections.Generic;
using System.Linq;
using IP.Objective;
using IP.Objective.Builds;
using IP.Screen;
using IP.UIFunc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IP.AI
{
    public class AIManager
    {
        enum CompanyStrategy
        {
            EXPAND_FIRST,
            UPGRADE_FIRST
        }
        
        private static AIManager _instance;
        
        public static AIManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new AIManager();
                return _instance;
            }
        }
        
        ///// ///// ///// ///// [ Singleton ] \\\\\ \\\\\ \\\\\ \\\\\

        private WorldMapInteraction _wmi;
        public List<Company> Companies { get; private set; }

        private string[] _companyNames = {
            "XInfinity", "Goggle Fiber", "TNT Company", "Veryson", "WideOpenEast",
            "Spektrums", "XBroadband", "CityLink", "Z-Mobile", "Media.net",
            "Stanley Internet", "Optinum", "Fiber Zipper", "Milkylink", "O-Telecom",
        };

        private Dictionary<Company, CompanyStrategy> _strategies;
        private Dictionary<Company, List<double>> _shareLog;
        private int _replanMonth;

        private const long WireOfficeLeast = 1000L;
        private const long SmallIDCLeast = 3000L;

        /// <summary>
        /// 국가마다 도시 2개, 시골 1개를 중심으로 하는 회사를 총 15개 생성한다.
        /// </summary>
        public void Initialize(WorldMapInteraction wmi)
        {
            _wmi = wmi;
            
            // 회사 생성
            Companies = new List<Company>();
            _strategies = new Dictionary<Company, CompanyStrategy>();
            _shareLog = new Dictionary<Company, List<double>>();
            _replanMonth = 0;
            foreach (Country country in _wmi.Countries)
            {
                byte cityCreated = 0, countrysideCreated = 0;
                while (cityCreated < 2 || countrysideCreated < 1)
                {
                    City select = country.Cities[Random.Range(0, country.Cities.Count)];
                    if (select.IsCity && cityCreated < 2)
                    {
                        Companies.Add(NewCompany(select));
                        cityCreated++;
                    }
                    else if (!select.IsCity && countrysideCreated < 1)
                    {
                        Companies.Add(NewCompany(select));
                        countrysideCreated++;
                    }
                }
            }
        }

        public void Earn()
        {
            ++_replanMonth;
            foreach (Company company in Companies)
            {
                long planRevenue = company.CalcRevenue() / 1000;
                long used = company.GetUsingMoney();
                long change = planRevenue - used;
                if(change > 0) company.Earn(change);
                else company.Earn(2000);
                Replan(company);
            }

            if (_replanMonth == 3) _replanMonth = 0;
        }

        public void CheckTrust()
        {
            foreach (Company company in Companies)
            {
                company.CalcTrust();
            }
        }

        public void ExecuteStrategy()
        {
            foreach (Company ai in Companies)
            {
                if (IsStage1(ai)) continue;
                if (IsStage2(ai)) continue;
            }
        }


        // Stage 1. 연결된 도시가 10개보다 적으면 확장한다.
        private bool IsStage1(Company company)
        {
            if (company.GetConnectedCities().Count >= 10) return false;
            Expanding(company);
            return true;
        }
        
        // 도시 확장을 위한 시퀀스
        private void Expanding(Company company)
        {
            // 최근 3달 수익 평균이 1000k$보다 낮을 경우 건설하지 않는다.
            if (company.RecentRevenue(3) < WireOfficeLeast) return;
            
            // 1. 사무실이 건설된 도시 중 전선 연결이 안된 도시가 있다면 전선을 연결한다.
            City buildTarget = null;
            foreach (City city in company.GetConnectedCities())
            {
                bool needWire = true;
                foreach (BuildBase builds in company.GetBuilds(city))
                {
                    if (builds.IsWire()) needWire = false;
                }

                if (!needWire) continue;
                buildTarget = city;
                break;
            }

            if (buildTarget != null)
            {
                Connection nearest = null;
                float distance = float.MaxValue;
                foreach (Connection conn in GameManager.Instance.GetConnections(buildTarget))
                {
                    if (conn.Distance < distance)
                    {
                        nearest = conn;
                    }
                }

                BuildBase wire = GetRecommendWire(company.CalcRevenue()).Clone();
                float buildDate = wire.GetBuildDate() * nearest.Distance;
                int[] nowDate = CalcEndDate((int) Math.Round(buildDate));

                wire.OverrideValues(nowDate, (wire.GetBudget() * nearest.Distance) / buildDate);
                company.ConstructWire(nearest.EndCity, buildTarget, wire);
                return;
            }

            // 2. 사무실이 있는 도시들이 모두 전선이 연결되어있다면, 새로운 사무실을 건설할 회사 개수가 적은 도시를 탐색한다.
            City lowCompanies = null;
            int connectedCompanies = Int32.MaxValue;
            foreach (City city in company.GetConnectedCities())
            {
                foreach (Connection conn in GameManager.Instance.GetConnections(city))
                {
                    if (company.GetConnectedCities().Contains(conn.EndCity)) continue;
                    List<Company> servicing = new List<Company>();
                    foreach (PaymentPlan plan in city.ServicingPlans)
                        if (!servicing.Contains(plan.OwnerCompany))
                            servicing.Add(plan.OwnerCompany);

                    if (servicing.Count < connectedCompanies)
                    {
                        lowCompanies = conn.EndCity;
                        connectedCompanies = servicing.Count;
                    }

                    if (connectedCompanies == 0) break;
                }

                if (connectedCompanies == 0) break;
            }

            if (lowCompanies != null)
            {
                BuildBase office = new Office().Clone();
                int[] nowDate = CalcEndDate((int) office.GetBuildDate());
                office.OverrideValues(nowDate, office.GetBudget());
                company.ConstructBuild(lowCompanies, office);
            }
        }

        // Stage 2. EXPAND FIRST일 경우 20개까지 Stage 1을 반복, Upgrade First일 경우 SMALL IDC를 짓기 시작한다.
        private bool IsStage2(Company company)
        {
            if (_strategies[company] == CompanyStrategy.EXPAND_FIRST)
            {
                if(company.GetConnectedCities().Count >= 20) return false;
                Expanding(company);
            }
            else if (_strategies[company] == CompanyStrategy.UPGRADE_FIRST)
            {
                City noIdc = null;
                foreach (City city in company.GetConnectedCities())
                {
                    bool hasIDC = false;
                    foreach (BuildBase builds in company.GetBuilds(city))
                    {
                        if (builds.GetName().Contains("IDC"))
                        {
                            hasIDC = true;
                            break;
                        }
                    }

                    if (!hasIDC)
                    {
                        noIdc = city;
                        break;
                    }
                }

                if (noIdc == null) return false;
                
                // 최근 3달 평균 수익이 3000k$보다 낮을 시 건설하지 않는다.
                if (company.RecentRevenue(3) < SmallIDCLeast) return true;

                BuildBase smallIDC = new IDCSmall().Clone();
                smallIDC.OverrideValues(CalcEndDate((int) smallIDC.GetBuildDate()), smallIDC.GetBudget());
                company.ConstructBuild(noIdc, smallIDC);
            }
            return true;
        }
        
        // 회사 수익금에 따른 티어 구분
        private BuildBase GetRecommendWire(long rev)
        {
            if (rev >= 500_000) return new UndergroundFiberCable();
            if (rev >= 200_000) return new FiberCable();
            if (rev >= 50_000) return new UndergroundCoaxialCable();
            return new CoaxialCable();
        }

        private int[] CalcEndDate(int duration)
        {
            int[] nowDate = AppBarManager.Instance.GetDate();
            nowDate[1] += duration;
            while (nowDate[1] > 12)
            {
                nowDate[1] -= 12;
                nowDate[0]++;
            }

            return nowDate;
        }

        private Company NewCompany(City startCity)
        {
            Company company = new Company(_companyNames[Companies.Count], startCity.IsCity ? 100000L : 5000L);
            
            List<City> connections = new List<City>();
            connections.Add(startCity);
            
            // 기본 건물 지급
            company.AddBuild(startCity, new HeadOffice());
            company.AddBuild(startCity, startCity.IsCity ? new IDCMedium() : new IDCSmall());
            company.AddBuild(startCity, startCity.IsCity ? new UndergroundCoaxialCable() : new CoaxialCable());
            
            // 근처 도시들과 확률적 연결
            long totalCitizen = startCity.People;
            int maxConnect = startCity.IsCity ? 8 : 4;
            Queue<City> connectionFind = new Queue<City>();
            connectionFind.Enqueue(startCity);
            
            while(connectionFind.Count > 0)
            {
                if (connections.Count > maxConnect) break;
                City center = connectionFind.Dequeue();
                foreach (Connection conn in _wmi.Connections[center])
                {
                    if (connections.Contains(conn.EndCity)) continue;
                    if (!ChanceTest(startCity.IsCity ? 40 : 75)) continue;
                    totalCitizen += (long) (conn.EndCity.People * 0.002f);
                    
                    connections.Add(conn.EndCity);
                    connectionFind.Enqueue(conn.EndCity);
                    company.AddBuild(conn.EndCity, new Office());

                    if (startCity.IsCity) // 도시회사일 경우, 10% 확률로 Medium IDC, 50%확률로 Small IDC를 얻는다. 
                    {
                        company.AddBuild(conn.EndCity, new UndergroundCoaxialCable());
                        if (ChanceTest(10))
                        {
                            company.AddBuild(conn.EndCity, new IDCMedium());
                        }
                        else if (ChanceTest(50))
                        {
                            company.AddBuild(conn.EndCity, new IDCSmall());
                        }
                        
                    }
                    else // 시골 회사일 경우, 20% 확률로 Small IDC를 얻는다.
                    {
                        company.AddBuild(conn.EndCity, new CoaxialCable());
                        if (ChanceTest(20))
                        {
                            company.AddBuild(conn.EndCity, new IDCSmall());
                        }
                    }
                }
            }

            // 기본 플랜 생성
            // 전체 대역폭, 속도를 연결된 도시에 1/N한 균등 플랜을 만들어서 모든 도시에서 서비스한다.
            PaymentPlan plan = new PaymentPlan(company);
            plan.Name = $"{company.Name}'s Plan";
            plan.Bandwidth = company.BandwidthAllowance / totalCitizen;
            plan.Upload = company.UpDownSpeed / connections.Count;
            plan.Download = company.UpDownSpeed / connections.Count;
            plan.Budget = 13;
            company.AddPlan(plan, connections);

            Array strategies = Enum.GetValues(typeof(CompanyStrategy));
            CompanyStrategy randStrategy = (CompanyStrategy) strategies.GetValue(Random.Range(0, strategies.Length));
            _strategies[company] = randStrategy;
            
            return company;
        }

        public void Replan(Company comp)
        {
            if (!_shareLog.ContainsKey(comp)) _shareLog[comp] = new List<double>();
            _shareLog[comp].Add((double) comp.GetTotalCustomers() / GameManager.Instance.GetTotalPeopleCount());

            // 3달을 주기로 변화
            if (_replanMonth < 3) return;
            
            // 상승 추세면 요금제 변화 X
            if (_shareLog[comp][^3] < _shareLog[comp][^1]) return;
                
            PaymentPlan plan = comp.PlanList[0];
            long totalCitizen = 0L;
            plan.Cities.ForEach(city =>
            {
                totalCitizen += city.GetCustomer(plan);
            });
            
            // 1. 대역폭 확장 ( 현 대역폭의 70% 까지 점유하게 만듬 )
            long nowBandwidth = totalCitizen * plan.Bandwidth;
            double bwShare = (double) nowBandwidth / comp.BandwidthAllowance;
            if (bwShare <= 0.70)
            {
                plan.Bandwidth = (long) (comp.BandwidthAllowance * 0.7d);
                return;
            }
            
            // 2. 속도 확장 ( 최대치로 )
            if (plan.Download < comp.UpDownSpeed)
            {
                plan.Upload = comp.UpDownSpeed;
                plan.Download = comp.UpDownSpeed;
                return;
            }
            
            // 3. 가격 인하 ( 1$씩 )
            long estimateNewEarn = totalCitizen * (plan.Budget - 1);
            if (comp.GetUsingMoney() < estimateNewEarn)
            {
                plan.Budget -= 1;
            }
        }

        private bool ChanceTest(float probability)
        {
            return Random.Range(0.000f, 1.000f) <= probability / 100;
        }
    }
}