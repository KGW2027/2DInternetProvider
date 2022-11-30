using System.Collections.Generic;
using UnityEngine;

namespace IP.Objective
{
    public class City
    {
        private long GeneratePeople(bool isCity)
        {
            return isCity ? Random.Range(1000000, 50000000) : Random.Range(10000, 500000);
        }
        
        public string Name;
        public long People;
        public Dictionary<PaymentPlan, long> PlanShares;
        public GameObject Button;

        public City(string cityName, bool isCity, GameObject button)
        {
            Name = cityName;
            People = GeneratePeople(isCity);
            Button = button;
            PlanShares = new Dictionary<PaymentPlan, long>();
        }
        
    }
}