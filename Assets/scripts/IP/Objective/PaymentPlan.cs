using System;
using System.Collections.Generic;
using UnityEngine;

namespace IP.Objective
{
    public class PaymentPlan
    {
        public readonly Company OwnerCompany;
        private readonly List<City> _cities;
        public string Name;
        public long Budget;
        public double Bandwidth;
        public double Upload;
        public double Download;
        public List<City> Cities => _cities;

        public PaymentPlan(Company comp)
        {
            _cities = new List<City>();
            OwnerCompany = comp;
        }

        public void Service(City c)
        {
            _cities.Add(c);
            c.ServicingPlans.Add(this);
        }

        public void Deservice(City c)
        {
            if (_cities.Contains(c))
            {
                _cities.Remove(c);
                c.ServicingPlans.Remove(this);
            }
        }

        public double GetUsingBandwidth()
        {
            double sum = 0.0d;
            _cities.ForEach(city =>
            {
                sum += city.GetCustomer(this) * city.ActiveRate;
            });
            return sum * Bandwidth;
        }
        
        public double GetUpDown()
        {
            return Math.Max(Upload, Download);
        }

        public long GetRevenue()
        {
            long revenue = 0L;
            foreach (City city in _cities)
            {
                revenue += city.GetCustomer(this) * Budget;
            }
            return revenue;
        }

    }
}