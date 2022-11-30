using System;
using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.Screen;
using IP.UIFunc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IP
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance { get; private set; }

        private const int StartYear = 22;
        private const int StartMonth = 11;
        private const long StartMoney = 1000L;

        private List<Country> _countries;
        private WorldMapInteractor _wmi;
        private WorldMapCameraController _wmcc;
        private bool _executedLateStart;

        public Company Company { get; private set; }

        void Start()
        {
            Instance = this;
        }

        public void LateStart()
        {
            _wmi = FindObjectOfType<WorldMapInteractor>();
            _wmcc = FindObjectOfType<WorldMapCameraController>();
            City startCity = _wmi.Cities[Random.Range(0, _wmi.Cities.Count)];
            Company.AddBuild(startCity, new HeadOffice());
            SetCameraFocus(startCity);
        }
        

        public void InitGame(String name)
        {
            Company = new Company(name, StartMoney);
        }

        public void SetCameraFocus(City city)
        {
            _wmcc.SetCameraFocus(city.Button.transform.position);
        }

        public Company GetUser()
        {
            return Company;
        }

        public string GetCompanyName()
        {
            return Company.GetName();
        }

        public void SetCompanyName(string name)
        {
            if (Company == null)
            {
                Company = new Company(name, 1000L);
            }
        }

        public long GetHaveMoney()
        {
            return Company.GetMoney();
        }

        public int[] GetStartDate()
        {
            return new[] {StartYear, StartMonth};
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
            return Company.GetConnectedCountries();
        }

        public int GetConnectCities()
        {
            return Company.GetConnectedCities().Count;
        }

        public int GetBuilds()
        {
            int buildCount = 0;
            Company.GetConnectedCities().ForEach(c =>
            {
                List<BuildBase> count = Company.GetBuilds(c);
                if (count != null) buildCount += count.Count;
            });
            return buildCount;
        }

        public long GetCustomers()
        {
            return Company.GetTotalCustomers();
        }

        public long GetDebt()
        {
            return Company.GetTotalDebtScale();
        }

        public long GetDebtInterest()
        {
            return Company.GetTotalDebtInterest();
        }

        public List<BuildBase> GetUnderConstructBuilds()
        {
            List<BuildBase> ucbList = new List<BuildBase>();
            Company.GetConnectedCities().ForEach(city =>
            {
                foreach (BuildBase build in Company.GetBuilds(city))
                {
                    if (!build.IsComplete()) ucbList.Add(build);
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
            Company.AddLoan(loan);

            AppBarManager.Instance.Refresh();


        }

        public bool UseMoney(long amount)
        {
            return Company.UseMoney(amount);
        }
    }
}
