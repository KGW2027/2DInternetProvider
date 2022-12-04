﻿using System;
using System.Collections.Generic;
using IP.Objective.Builds;
using IP.Screen;
using UnityEngine;

namespace IP.Objective
{
    public class Company
    {
        private readonly Dictionary<City, List<BuildBase>> _builds;
        private List<PaymentPlan> _plans;
        private List<Debt> _debts;
        private List<long> _revenueLog;

        public readonly string Name;
        
        public long Money { get; private set; }
        public int UseLoanTimes { get; private set; }
        public int RepayLoanTimes { get; private set; }
        public int Trust { get; private set; }
        public long BandwidthAllowance;
        public long UpDownSpeed;

        public Company(string name, long startMoney)
        {
            Name = name;
            Money = startMoney;
            Trust = 0;
            _revenueLog = new List<long>();
            _builds = new Dictionary<City, List<BuildBase>>();
            _plans = new List<PaymentPlan>();
            _debts = new List<Debt>();
            UseLoanTimes = 0;
            RepayLoanTimes = 0;
            
            BandwidthAllowance = 0;
            UpDownSpeed = 0;
        }

        public void AddBuild(City city, BuildBase build)
        {
            if (!_builds.ContainsKey(city)) _builds[city] = new List<BuildBase>();
            build.Complete(this);
            _builds[city].Add(build);
        }

        public void AddPlan(PaymentPlan plan)
        {
            _plans.Add(plan);
        }

        public void ConstructBuild(City city, BuildBase build)
        {
            if (!_builds.ContainsKey(city)) _builds[city] = new List<BuildBase>();
            _builds[city].Add(build);
        }

        public long CalcRevenue()
        {
            long revenue = 0L;
            foreach (PaymentPlan plan in _plans)
            {
                revenue += plan.GetRevenue();
            }
            return revenue;
        }

        public long CalcInterest()
        {
            long interest = 0L;
            List<Debt> repayed = new List<Debt>();
            int[] date = AppBarManager.Instance.GetDate();
            foreach (Debt debt in _debts)
            {
                interest += (long) (debt.Scale * (debt.Interest / (12 * 100)));
                if (debt.StartYear + 3 <= date[0] && debt.StartMonth <= date[1])
                {
                    Debug.Log($"대출 {debt.Scale}상환");
                    Money -= debt.Scale;
                    RepayLoanTimes++;
                    repayed.Add(debt);
                }
            }
            repayed.ForEach(debt => _debts.Remove(debt));
            return interest;
        }

        public long CalcMaintenance()
        {
            long maintenance = 0L;
            foreach (List<BuildBase> builds in _builds.Values)
            {
                builds.ForEach(build =>
                {
                    if (build.IsComplete()) maintenance += (long) build.GetMaintenance();
                    else maintenance += (long) (build.GetBudget() / build.GetBuildDate());
                });
            }

            return maintenance;
        }

        public void Earn(long money)
        {
            _revenueLog.Add(money);
            Money += money;
        }

        public long RecentRevenue(int range)
        {
            long revenue = 0L;
            for (int key = 1; key <= range && _revenueLog.Count - key > 0; key++)
            {
                revenue += _revenueLog[^key];
            }
            return revenue;
        }

        public void CalcTrust()
        {
            double usingBandwidth = 0.0d;
            long usingUpDown = 0L;
            foreach (PaymentPlan plan in _plans)
            {
                usingBandwidth += plan.GetUsingBandwidth() * 1024L;
                usingUpDown += plan.GetUpDown();
            }

            // 서비스 중인 플랜이 없는 경우 신뢰도 계산 스킵
            if (usingBandwidth == 0 || usingUpDown == 0) return;

            double bandwidthTrust = (usingBandwidth / BandwidthAllowance) - 1;
            double updownTrust = (usingUpDown / UpDownSpeed) - 1;
            int beforeTrust = Trust;

            if (bandwidthTrust < 0 && updownTrust < 0)
            {
                Trust += 1;
            }
            else if (bandwidthTrust > 0 || updownTrust > 0)
            {
                Trust -= (int) Math.Ceiling(bandwidthTrust * 5);
                Trust -= (int) Math.Ceiling(updownTrust * 8);
            }
            Debug.Log($"신뢰도 변경 : {beforeTrust} -> {Trust}");
        }
        
        
        
        public List<City> GetConnectedCities()
        {
            return new List<City>(_builds.Keys);
        }       
        
        public List<BuildBase> GetUnderConstructBuilds()
        {
            List<BuildBase> ucbList = new List<BuildBase>();
            GetConnectedCities().ForEach(city =>
            {
                foreach (BuildBase build in GetBuilds(city))
                {
                    if (!build.IsComplete()) ucbList.Add(build);
                }
            });
            return ucbList;
        }

        public int GetConnectedCountries()
        {
            List<City> cities = GetConnectedCities();
            List<Country> countries = GameManager.Instance.GetCountries();
            List<string> connects = new List<string>();
            
            countries.ForEach(coun =>
            {
                foreach (City c in cities)
                {
                    if (coun.Cities.Contains(c))
                    {
                        connects.Add(coun.Name);
                        break;
                    }
                }
            });
            
            return connects.Count;
        }

        public List<BuildBase> GetBuilds(City city)
        {
            if (_builds.ContainsKey(city)) return _builds[city];
            return null;
        }
        
        public int GetTotalBuilds()
        {
            int buildCount = 0;
            GetConnectedCities().ForEach(c =>
            {
                List<BuildBase> count = GetBuilds(c);
                if (count != null) buildCount += count.Count;
            });
            return buildCount;
        }

        public long GetTotalDebtScale()
        {
            long scale = 0;
            foreach (Debt debt in _debts)
            {
                scale += debt.Scale;
            }

            return scale;
        }

        public long GetTotalCustomers()
        {
            long customers = 0L;
            GetConnectedCities().ForEach(city =>
            {
                customers += city.GetCustomer(this);
            });
            return customers;
        }

        public string GetTotalTraffic()
        {
            return "1GB";
        }

        public void AddLoan(Debt debt)
        {
            _debts.Add(debt);
            Money += debt.Scale;
            UseLoanTimes++;
        }

        public bool UseMoney(long amount)
        {
            if (Money < amount) return false;
            Money -= amount;
            AppBarManager.Instance.Refresh();
            return true;
        }
    }
}