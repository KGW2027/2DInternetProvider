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
        public enum CompanyStrategy
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
        private Dictionary<CompanyStrategy, Strategies> _strategyManager;
        
        /// <summary>
        /// 국가마다 도시 2개, 시골 1개를 중심으로 하는 회사를 총 15개 생성한다.
        /// </summary>
        public void Initialize(WorldMapInteraction wmi)
        {
            _wmi = wmi;
            
            // 회사 생성
            Companies = new List<Company>();
            _strategies = new Dictionary<Company, CompanyStrategy>();
            _strategyManager = new Dictionary<CompanyStrategy, Strategies>();
            _strategyManager[CompanyStrategy.EXPAND_FIRST] = new ExpandStrategy();
            _strategyManager[CompanyStrategy.UPGRADE_FIRST] = new UpgradeStrategy();
            
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
                // 수익 계산
                long planRevenue = company.CalcRevenue() / 1000;
                long used = company.GetUsingMoney();
                long change = planRevenue - used;
                if(change > 0) company.Earn(change);
                else company.Earn(2000);

                // 매 1월마다 랜덤으로 요금제 가격 인상
                if (AppBarManager.Instance.GetDate()[1] == 1 && AppBarManager.Instance.GetDate()[0] > GameManager.Instance.GetStartDate()[0])
                {
                    double leastShare = _shareLog[company][^1];
                    if (leastShare > 0.01d)
                    {
                        int min = (int) Math.Ceiling(leastShare * 10);
                        int max = (int) Math.Ceiling(leastShare * 30);
                        company.PlanList[0].Budget += Random.Range(min, max+1);
                        Debug.Log(company.Name + " 인상 : " + company.PlanList[0].Budget);
                    }
                }
                
                // 연결된 도시 중에 서비스 중이 아닌 도시가 있다면 요금제 서비스
                CheckService(company);
                
                // 요금제 설정
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
            Companies.ForEach(RunStrategy);
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
            plan.Bandwidth = company.BandwidthAllowance / (ulong) totalCitizen;
            plan.Upload = company.UpDownSpeed / (ulong) connections.Count;
            plan.Download = company.UpDownSpeed / (ulong) connections.Count;
            plan.Budget = 13;
            company.AddPlan(plan, connections);

            Array strategies = Enum.GetValues(typeof(CompanyStrategy));
            CompanyStrategy randStrategy = (CompanyStrategy) strategies.GetValue(Random.Range(0, strategies.Length));
            _strategies[company] = randStrategy;
            _strategyManager[randStrategy].SetStrategy(company);

            return company;
        }

        public void RunStrategy(Company comp)
        {
            _strategyManager[_strategies[comp]].Do(comp);
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
            long nowBandwidth = totalCitizen * (long) plan.Bandwidth;
            double bwShare = nowBandwidth / comp.BandwidthAllowance;
            if (bwShare <= 0.70)
            {
                plan.Bandwidth = comp.BandwidthAllowance * 0.7d;
                return;
            }
            
            // 2. 속도 확장 ( 최대치로 )
            if (plan.Download < comp.UpDownSpeed)
            {
                plan.Upload = comp.UpDownSpeed;
                plan.Download = comp.UpDownSpeed;
                return;
            }
            
            // 3. 가격 인하 ( 20%정도 여유로 )
            long estimateEarn = totalCitizen * plan.Budget;
            int subs = 0;
            while (estimateEarn > comp.GetUsingMoney())
            {
                estimateEarn -= totalCitizen;
                subs++;
            }

            if (subs > 2) plan.Budget -= subs - 2;
        }

        private void CheckService(Company comp)
        {
            List<City> connected = comp.GetConnectedCities();
            List<City> servicing = comp.PlanList[0].Cities;

            foreach (City conn in connected)
            {
                if (servicing.Contains(conn)) continue;
                comp.PlanList[0].Service(conn);
            }
        }

        private bool ChanceTest(float probability)
        {
            return Random.Range(0.000f, 1.000f) <= probability / 100;
        }
    }
}