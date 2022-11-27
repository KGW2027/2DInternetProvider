using System;
using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.Screen;
using UnityEngine;

namespace IP
{
    public class GameManager : MonoBehaviour
    {
        
        public static GameManager Instance { get; private set; }

        private readonly int _startYear = 22;
        private readonly int _startMonth = 11;

        private Company _company;
        private List<Country> _countries;

        void Start()
        {
            Instance = this;
        }

        public string GetCompanyName()
        {
            return _company.GetName();
        }

        public void SetCompanyName(string name)
        {
            if(_company == null) _company = new Company(name, 1000L);
        }

        public long GetHaveMoney()
        {
            return _company.GetMoney();
        }

        public int[] GetStartDate()
        {
            return new[] {_startYear, _startMonth};
        }

        public void SetCountriesInfo(List<Country> countries)
        {
            _countries = countries;
        }

        public List<Country> GetCountries()
        {
            return _countries;
        }

        public int GetConnectCountries()
        {
            return _company.GetConnectedCountries();
        }

        public int GetConnectCities()
        {
            return _company.GetConnectedCities().Count;
        }

        public int GetBuilds()
        {
            int buildCount = 0;
            _company.GetConnectedCities().ForEach(c =>
            {
                List<BuildBase> count = _company.GetBuilds(c);
                if (count != null) buildCount += count.Count;
            });
            return buildCount;
        }

        public long GetCustomers()
        {
            return _company.GetTotalCustomers();
        }

        public long GetDebt()
        {
            return _company.GetTotalDebtScale();
        }

        public long GetDebtInterest()
        {
            return _company.GetTotalDebtInterest();
        }

        public List<BuildBase> GetUnderConstructBuilds()
        {
            List<BuildBase> ucbList = new List<BuildBase>();
            _company.GetConnectedCities().ForEach(city =>
            {
                foreach (BuildBase build in _company.GetBuilds(city))
                {
                    if(!build.IsComplete()) ucbList.Add(build);
                }
            });
            return ucbList;
        }

        public void AddLoan(Bank bank, long money)
        {
            Debt loan = new Debt();
            loan.Interest = bank.Interest;
            loan.Scale = money;
            loan.FromBank = bank;

            string[] date = AppBarManager.Instance.GetDate().Split("Y");
            loan.StartYear = Int32.Parse(date[0]);
            loan.StartMonth = Int32.Parse(date[1].Substring(0, date[1].Length - 1));
            _company.AddLoan(loan);
            
            AppBarManager.Instance.Refresh();
            
            
        }
    }
}
