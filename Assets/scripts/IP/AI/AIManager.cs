using System;
using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.UIFunc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IP.AI
{
    public class AIManager
    {
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
            "Company1", "Company2", "Company3", "Company4", "Company5",
            "Company6", "Company7", "Company8", "Company9", "Company10",
            "Company11", "Company12", "Company13", "Company14", "Company15",
        };
        
        /// <summary>
        /// 국가마다 도시 2개, 시골 1개를 중심으로 하는 회사를 총 15개 생성한다.
        /// </summary>
        public void Initialize(WorldMapInteraction wmi)
        {
            _wmi = wmi;
            
            // 회사 생성
            Companies = new List<Company>();
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
            foreach (Company company in Companies)
            {
                long planRevenue = company.CalcRevenue() / 1000;
                long used = company.CalcInterest() + company.CalcMaintenance();
                company.Earn(planRevenue - used);
            }
        }

        public void CheckTrust()
        {
            foreach (Company company in Companies)
            {
                company.CalcTrust();
            }
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
            plan.Name = $"{company.Name}-Default Plan";
            plan.Bandwidth = company.BandwidthAllowance / totalCitizen;
            plan.Upload = company.UpDownSpeed / connections.Count;
            plan.Download = company.UpDownSpeed / connections.Count;
            plan.Budget = 13;
            company.AddPlan(plan, connections);
            
            return company;
        }

        private bool ChanceTest(float probability)
        {
            return Random.Range(0.000f, 1.000f) <= probability / 100;
        }
    }
}