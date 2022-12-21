using System;
using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.UIFunc;

namespace IP.AI
{
    public class UpgradeStrategy : Strategies
    {
        protected override void Stage1(Company comp)
        {
            // 최근 3달 수익 평균이 1000k$보다 낮을 경우 건설하지 않는다.
            if (comp.RecentRevenue(3) < WireOfficeLeast) return;
            
            // 1. 사무실이 건설된 도시 중 전선 연결이 안된 도시가 있다면 전선을 연결한다.
            City buildTarget = null;
            foreach (City city in comp.GetConnectedCities())
            {
                bool needWire = true;
                foreach (BuildBase builds in comp.GetBuilds(city))
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

                BuildBase wire = GetRecommendWire(comp.CalcRevenue()).Clone();
                float buildDate = wire.GetBuildDate() * nearest.Distance;
                int[] nowDate = CalcEndDate((int) Math.Round(buildDate));

                wire.OverrideValues(nowDate, (wire.GetBudget() * nearest.Distance) / buildDate);
                comp.ConstructWire(nearest.EndCity, buildTarget, wire);
                return;
            }

            // 2. 사무실이 있는 도시들이 모두 전선이 연결되어있다면, 새로운 사무실을 건설할 회사 개수가 적은 도시를 탐색한다.
            City lowCompanies = null;
            int connectedCompanies = Int32.MaxValue;
            foreach (City city in comp.GetConnectedCities())
            {
                foreach (Connection conn in GameManager.Instance.GetConnections(city))
                {
                    if (comp.GetConnectedCities().Contains(conn.EndCity)) continue;
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
                comp.ConstructBuild(lowCompanies, office);
            }
            
        }

        protected override void Stage2(Company comp)
        {
            City noIdc = null;
            foreach (City city in comp.GetConnectedCities())
            {
                bool hasIDC = false;
                foreach (BuildBase builds in comp.GetBuilds(city))
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

            if (noIdc == null) return;
                
            // 최근 3달 평균 수익이 3000k$보다 낮을 시 건설하지 않는다.
            if (comp.RecentRevenue(3) < SmallIDCLeast) return;

            BuildBase smallIDC = new IDCSmall().Clone();
            smallIDC.OverrideValues(CalcEndDate((int) smallIDC.GetBuildDate()), smallIDC.GetBudget());
            comp.ConstructBuild(noIdc, smallIDC);
            
        }

        protected override void Stage3(Company comp)
        {
            
        }

        protected override void Stage4(Company comp)
        {
            
        }

        protected override void Stage5(Company comp)
        {
            
        }

        protected override void Stage6(Company comp)
        {
            
        }

        protected override void Stage7(Company comp)
        {
            
        }

        protected override void Stage8(Company comp)
        {
            
        }

        protected override void Stage9(Company comp)
        {
            
        }

        protected override void Stage10(Company comp)
        {
            
        }

        protected override void Stage11(Company comp)
        {
            
        }

        protected override void Stage12(Company comp)
        {
            
        }

        protected override void Stage13(Company comp)
        {
            
        }

        protected override void Stage14(Company comp)
        {
            
        }

        protected override void Stage15(Company comp)
        {
            
        }

        protected override int GetMaxStage()
        {
            return 15;
        }
    }
}