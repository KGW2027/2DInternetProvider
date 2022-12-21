using System;
using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.Screen;
using IP.UIFunc;

namespace IP.AI
{
    public abstract class Strategies
    {
        protected Dictionary<Company, int> _stages;
        protected const long WireOfficeLeast = 1000L;
        protected const long SmallIDCLeast = 3000L;

        protected abstract void Stage1(Company comp);
        protected abstract void Stage2(Company comp);
        protected abstract void Stage3(Company comp);
        protected abstract void Stage4(Company comp);
        protected abstract void Stage5(Company comp);
        protected abstract void Stage6(Company comp);
        protected abstract void Stage7(Company comp);
        protected abstract void Stage8(Company comp);
        protected abstract void Stage9(Company comp);
        protected abstract void Stage10(Company comp);
        protected abstract void Stage11(Company comp);
        protected abstract void Stage12(Company comp);
        protected abstract void Stage13(Company comp);
        protected abstract void Stage14(Company comp);
        protected abstract void Stage15(Company comp);
        protected abstract int GetMaxStage();

        public void SetStrategy(Company comp)
        {
            if(_stages == null)
                _stages = new Dictionary<Company, int>();
            _stages[comp] = 1;
        }

        public void Do(Company comp)
        {
            if (_stages[comp] > GetMaxStage()) return;
            switch (_stages[comp])
            {
                case 1:
                    Stage1(comp);
                    break;
                case 2:
                    Stage2(comp);
                    break;
                case 3:
                    Stage3(comp);
                    break;
                case 4:
                    Stage4(comp);
                    break;
                case 5:
                    Stage5(comp);
                    break;
                case 6:
                    Stage6(comp);
                    break;
                case 7:
                    Stage7(comp);
                    break;
                case 8:
                    Stage8(comp);
                    break;
                case 9:
                    Stage9(comp);
                    break;
                case 10:
                    Stage10(comp);
                    break;
                case 11:
                    Stage11(comp);
                    break;
                case 12:
                    Stage12(comp);
                    break;
                case 13:
                    Stage13(comp);
                    break;
                case 14:
                    Stage14(comp);
                    break;
                case 15:
                    Stage15(comp);
                    break;
            }
        }

        public void Next(Company comp)
        {
            _stages[comp]++;
        }


        protected int[] CalcEndDate(int duration)
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
        
        // 회사 수익금에 따른 티어 구분
        protected BuildBase GetRecommendWire(long rev)
        {
            if (rev >= 500_000) return new UndergroundFiberCable();
            if (rev >= 200_000) return new FiberCable();
            if (rev >= 50_000) return new UndergroundCoaxialCable();
            return new CoaxialCable();
        }

        protected City FindNoWire(Company comp)
        {
            foreach (City conn in comp.GetConnectedCities())
            {
                List<BuildBase> builds = comp.GetBuilds(conn);

                bool hasWire = false;
                foreach (BuildBase constructed in builds)
                {
                    if (constructed.IsWire())
                    {
                        hasWire = true;
                        break;
                    }
                }

                if (!hasWire) return conn;
            }
            
            return null;
        }

        protected City FindNoOffice(Company comp)
        {
            City result = null;
            
            // 가장 적은 회사가 서비스되고 있는 도시와 연결
            foreach (City city in comp.GetConnectedCities())
            {
                foreach (Connection conn in GameManager.Instance.GetConnections(city))
                {
                    if (comp.GetConnectedCities().Contains(conn.EndCity)) continue;

                    if (result == null) result = conn.EndCity;
                    else if (result.ServicingPlans.Count > conn.EndCity.ServicingPlans.Count) result = conn.EndCity;
                }
            }
            return result;
        }

        protected City FindNoBuild(Company comp, Type build)
        {
            // 가장 적은 회사가 서비스되고 있는 도시와 연결
            foreach (City city in comp.GetConnectedCities())
            {
                bool hasBuild = false;
                foreach (BuildBase b in comp.GetBuilds(city))
                {
                    if (b.GetType() == build)
                    {
                        hasBuild = true;
                        break;
                    }
                }
                if (!hasBuild) return city;
            }
            return null;
        }

        protected City FindHasBuild(Company comp, Type cond)
        {
            foreach (City city in comp.GetConnectedCities())
            {
                foreach (BuildBase b in comp.GetBuilds(city))
                {
                    if (b.GetType() == cond)
                    {
                        return city;
                    }
                }
            }
            return null;
        }

        protected void ConstructWire(Company comp, City city)
        {
            Connection nearest = null;
            float distance = float.MaxValue;
            foreach (Connection conn in GameManager.Instance.GetConnections(city))
            {
                if (comp.GetConnectedCities().Contains(conn.EndCity) && conn.Distance < distance)
                {
                    nearest = conn;
                }
            }

            BuildBase wire = GetRecommendWire(comp.CalcRevenue()).Clone();
            float buildDate = wire.GetBuildDate() * nearest.Distance;
            int[] nowDate = CalcEndDate((int) Math.Round(buildDate));

            wire.OverrideValues(nowDate, (wire.GetBudget() * nearest.Distance) / buildDate);
            comp.ConstructWire(nearest.EndCity, city, wire);
        }

        protected int GetBuildCount(Company comp, Type build)
        {
            int cnt = 0;
            foreach (City city in comp.GetConnectedCities())
            {
                foreach (BuildBase buildBase in comp.GetBuilds(city))
                {
                    if (buildBase.GetType() == build)
                    {
                        cnt++;
                        break;
                    }
                }
            }

            return cnt;
        }

        protected void Expand(Company comp)
        {
            // 1. 사무실이 건설된 도시 중 전선 연결이 안된 도시가 있다면 전선을 연결한다.
            City buildTarget = FindNoWire(comp);
            if (buildTarget != null)
            {
                ConstructWire(comp, buildTarget);
                return;
            }

            // 2. 사무실이 있는 도시들이 모두 전선이 연결되어있다면, 새로운 사무실을 건설할 회사 개수가 적은 도시를 탐색한다.
            City newOffice = FindNoOffice(comp);
            if (newOffice != null)
            {
                ConstructBuild(comp, newOffice, new Office());
            }
        }

        protected long GetEstimateUse(Company comp)
        {
            long revenue = comp.RecentRevenue(3);
            foreach (City city in comp.GetConnectedCities())
            {
                foreach (BuildBase bb in comp.GetBuilds(city))
                {
                    if (!bb.IsComplete()) revenue -= (long) bb.GetMaintenance();
                }
            }

            return revenue;
        }

        private bool CheckBuildMoney(Company comp, BuildBase build)
        {
            long revenue = GetEstimateUse(comp);
            long need = (long) ((build.GetMaintenance() + build.GetBudget() / build.GetBuildDate()) * 0.8d);
            return revenue >= need;
        }

        protected bool ConstructBuild(Company comp, City city, BuildBase build)
        {
            BuildBase clone = build.Clone();
            if (!CheckBuildMoney(comp, build)) return false;
            
            int[] nowDate = CalcEndDate((int) clone.GetBuildDate());
            clone.OverrideValues(nowDate, clone.GetBudget());
            comp.ConstructBuild(city, clone);
            return true;
        }
    }
}