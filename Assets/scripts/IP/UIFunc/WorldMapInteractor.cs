using System.Collections.Generic;
using System.Numerics;
using IP.Objective;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace IP.UIFunc
{

    class Connection
    {
        public City EndCity;
        public float Distance;
        public LineRenderer Line;

        public Connection(City a, float b, LineRenderer c)
        {
            EndCity = a;
            Distance = b;
            Line = c;
        }
    }
    
    public class WorldMapInteractor : MonoBehaviour
    {
        public GameObject lineDrawer;
        
        private List<GameObject> _cityObjects;
        private List<GameObject> _countryObjects;
        private List<Country> _countries;
        private Dictionary<City, List<Connection>> _connections;
        private const int MaxConnectDistance = 30;

        // Start is called before the first frame update
        void Start()
        {
            _cityObjects = new List<GameObject>();
            _countryObjects = new List<GameObject>();
            _countries = new List<Country>();
            _connections = new Dictionary<City, List<Connection>>();

            RegisterCities();
            MakeTree();
        }

        private void MakeTree()
        {
            List<City> allCities = new List<City>();
            _countries.ForEach(country =>
            {
                country.Cities.ForEach(city =>
                {
                    allCities.Add(city);
                    _connections[city] = new List<Connection>();
                });
            });

            for (int city = 0; city < allCities.Count; city++)
            {
                City startCity = allCities[city];
                Vector3 scv = startCity.Button.transform.position;
                for (int ecity = city + 1; ecity < allCities.Count; ecity++)
                {
                    City endCity = allCities[ecity];
                    Vector3 ecv = endCity.Button.transform.position;
                    float distance = Vector3.Distance(scv, ecv);
                    if (distance < MaxConnectDistance)
                    {
                        LineRenderer line = Instantiate(lineDrawer).GetComponent<LineRenderer>();
                        line.startWidth = 1.0f;
                        line.endWidth = 1.0f;
                        line.useWorldSpace = true;
                        line.SetPositions(new []{scv, ecv});
                        _connections[startCity].Add(new Connection(endCity, distance, line));
                        _connections[endCity].Add(new Connection(startCity, distance, line));
                    }
                }
            }
        }

        private void RegisterCities()
        {
            foreach(Transform countries in transform)
            {
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
            if (_cityObjects == null || _countryObjects == null) return;
            _cityObjects.ForEach(city => city.SetActive(visibleCity));
            _countryObjects.ForEach(country => country.SetActive(!visibleCity));
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
