using System.Collections.Generic;
using UnityEngine;

namespace IP.Objective
{
    public class City
    {
        public static long GeneratePeople(bool isCity)
        {
            return isCity ? Random.Range(1000000, 50000000) : Random.Range(10000, 500000);
        }
        
        public string Name;
        public long People;
        public Dictionary<PaymentPlan, long> PlanShares;
        public GameObject Button;

        public City(string cityName, long cityPeople, GameObject button)
        {
            Name = cityName;
            People = cityPeople;
            Button = button;
            PlanShares = new Dictionary<PaymentPlan, long>();
        }
        
    }
}