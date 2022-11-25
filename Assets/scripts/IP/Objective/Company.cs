﻿using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace IP.Objective
{
    public class Company
    {
        private string _companyName;
        private long _money;
        private Dictionary<City, List<IBuild>> _builds;
        private List<Debt> _debts;

        public Company(string name, long startMoney)
        {
            _companyName = name;
            _money = startMoney;
            _builds = new Dictionary<City, List<IBuild>>();
            _debts = new List<Debt>();
        }

        public string GetName()
        {
            return _companyName;
        }

        public long GetMoney()
        {
            return _money;
        }

        public List<City> GetConnectedCities()
        {
            return new List<City>(_builds.Keys);
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

        public List<IBuild> GetBuilds(City city)
        {
            if (_builds.ContainsKey(city)) return _builds[city];
            return null;
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
    }
}