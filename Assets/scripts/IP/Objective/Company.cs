using System;
using System.Collections.Generic;
using IP.Objective.Builds;
using IP.Screen;

namespace IP.Objective
{
    public class Company
    {
        private readonly Dictionary<City, List<BuildBase>> _builds;
        private List<PaymentPlan> _plans;
        private List<Debt> _debts;

        public readonly string Name;
        
        public long Money { get; private set; }
        public int UseLoanTimes { get; private set; }
        public int RepayLoanTimes { get; private set; }

        public Company(string name, long startMoney)
        {
            Name = name;
            Money = startMoney;
            _builds = new Dictionary<City, List<BuildBase>>();
            _plans = new List<PaymentPlan>();
            _debts = new List<Debt>();
            UseLoanTimes = 0;
            RepayLoanTimes = 0;
        }

        public void AddBuild(City city, BuildBase build)
        {
            if (!_builds.ContainsKey(city)) _builds[city] = new List<BuildBase>();
            _builds[city].Add(build);
        }

        public long GetMoney()
        {
            return Money;
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

        public long GetTotalDebtInterest()
        {
            long scale = 0;
            foreach (Debt debt in _debts)
            {
                scale += (long) Math.Round(debt.Scale * (debt.Interest / 12));
            }

            return scale;
        }

        public long GetTotalCustomers()
        {
            long customers = 0L;
            GetConnectedCities().ForEach(city =>
            {
                foreach (PaymentPlan plan in city.PlanShares.Keys)
                {
                    if(plan.OwnerCompany == this) customers += city.PlanShares[plan];
                }
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