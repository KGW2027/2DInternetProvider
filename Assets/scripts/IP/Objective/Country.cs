using System.Collections.Generic;
using UnityEngine;

namespace IP.Objective
{
    public class Country
    {
        public List<City> Cities;
        public string Name;
        public GameObject Button;

        public Country(string name, GameObject button)
        {
            Name = name;
            Button = button;
            Cities = new List<City>();
        }

        public void AddCity(City city)
        {
            Cities.Add(city);
        }

        public List<City> GetCities()
        {
            return Cities;
        }
        
    }
}