using System.Collections.Generic;
using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc
{
    public class WorldMapInteractor : MonoBehaviour
    {
        private List<GameObject> _cityObjects;
        private List<GameObject> _countryObjects;
        private List<Country> _countries;
    
        // Start is called before the first frame update
        void Start()
        {
            _cityObjects = new List<GameObject>();
            _countryObjects = new List<GameObject>();
            _countries = new List<Country>();
            
            RegisterCities();
        }

        private void RegisterCities()
        {
            foreach(Transform countries in transform)
            {
                Debug.Log(countries.name);
                Country country = null;
                if (countries.CompareTag("CityParents"))
                {
                    Transform countryTransform = countries.Find("CountryLogo");
                    string countryName = countryTransform.GetChild(0).GetComponent<TextMeshPro>().text;
                    country = new Country(countryName, countryTransform.gameObject);
                    
                    _countryObjects.Add(country.Button);

                    foreach (Transform cities in countries.transform)
                    {
                        if (cities.CompareTag("WorldMapButton"))
                        {
                            TextMeshPro cityText = cities.GetChild(0).GetComponent<TextMeshPro>();
                            long people = City.GeneratePeople(cityText.name.Equals("Text_CityName"));
                            City city = new City(cityText.text, people, cities.gameObject);
                            country.AddCity(city);
                            
                            _cityObjects.Add(city.Button);
                        }
                    }
                }
                if(country != null) _countries.Add(country);
            }
            GameManager.Instance.SetCountriesInfo(_countries);
        }

        public void ChangeVisibleMode(bool visibleCity)
        {
            _cityObjects.ForEach(city => city.SetActive(visibleCity));
            _countryObjects.ForEach(city => city.SetActive(!visibleCity));
        }

        public string ClickMap(Vector2 location)
        {
            RaycastHit2D hit = Physics2D.Raycast(location, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                GameObject clicked = hit.transform.gameObject;
                if (clicked.CompareTag("WorldMapButton"))
                {
                    string cityName = clicked.transform.GetChild(0).GetComponent<TextMeshPro>().text;
                    return $"City {cityName}";
                }
                else if (clicked.CompareTag("WorldMapCountryButton"))
                {
                    string countryName = clicked.transform.GetChild(0).GetComponent<TextMeshPro>().text;
                    return $"Country {countryName}";
                }
            }

            return null;
        }
    }
}
