using System.Collections.Generic;
using IP.Objective;
using IP.UIFunc.Builder;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace IP.UIFunc
{
    public class Connection
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
    
    public class WorldMapInteraction : MonoBehaviour
    {
        [Header("월드맵 트리 간선")]
        public GameObject lineDrawer;

        [Header("도시 정보 팝업")] public GameObject cityInfoPrefab;
        
        private List<GameObject> _cityObjects;
        private List<GameObject> _countryObjects;
        private Dictionary<GameObject, City> _mapCityDictionary;
        private Dictionary<Collider2D, GameObject> _cityInfoPopups;
        private Dictionary<City, bool> _cityInfoPopupsOpen;

        public List<Country> Countries;
        public List<City> Cities;
        public Dictionary<City, List<Connection>> Connections;
        private const int MaxConnectDistance = 30;

        // Start is called before the first frame update
        void Start()
        {
            _cityObjects = new List<GameObject>();
            _countryObjects = new List<GameObject>();
            _mapCityDictionary = new Dictionary<GameObject, City>();
            _cityInfoPopups = new Dictionary<Collider2D, GameObject>();
            _cityInfoPopupsOpen = new Dictionary<City, bool>();
            
            Countries = new List<Country>();
            Cities = new List<City>();
            Connections = new Dictionary<City, List<Connection>>();

            RegisterCities();
            MakeTree();
            GameManager.Instance.LateStart();
        }

        /**
         * 월드맵에 존재하는 모든 도시와 국가들을 로드하는 함수
         */
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
                            City city = new City(cityText.text, cityText.name.Equals("Text_CityName"), cities.gameObject);
                            country.AddCity(city);

                            _mapCityDictionary[city.Button] = city;
                            _cityObjects.Add(city.Button);
                            _cityInfoPopupsOpen[city] = false;
                            Cities.Add(city);
                        }
                    }
                }
                if(country != null) Countries.Add(country);
            }
        }

        /**
         * 월드맵에서 연결을 지원하는 도시간 간선을 생성하는 함수
         */
        private void MakeTree()
        {
            List<City> allCities = new List<City>();
            Countries.ForEach(country =>
            {
                country.Cities.ForEach(city =>
                {
                    allCities.Add(city);
                    Connections[city] = new List<Connection>();
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
                        Connections[startCity].Add(new Connection(endCity, distance, line));
                        Connections[endCity].Add(new Connection(startCity, distance, line));
                    }
                }
            }
        }

        /**
         * 월드맵에서 도시와 국가가 보이는 상태를 변경하는 함수
         */
        public void ChangeVisibleMode(bool visibleCity)
        {
            if (_cityObjects == null || _countryObjects == null) return;
            _cityObjects.ForEach(city => city.SetActive(visibleCity));
            _countryObjects.ForEach(country => country.SetActive(!visibleCity));
        }

        /**
         * 월드맵에서 도시나 국가를 클릭했을 때 기능을 수행하는 함수
         */
        public void ClickMap(Vector2 location)
        {
            RaycastHit2D hit = Physics2D.Raycast(location, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                GameObject clicked = hit.transform.gameObject;
                if (_cityInfoPopups.ContainsKey(hit.collider))
                {
                    City city = _cityInfoPopups[hit.collider].GetComponent<CityInfoPopup>().GetCity();
                    _cityInfoPopupsOpen[city] = false;
                    
                    Destroy(_cityInfoPopups[hit.collider]);
                    _cityInfoPopups.Remove(hit.collider);
                }
                else if (clicked.CompareTag("WorldMapButton"))
                {
                    City city = _mapCityDictionary[clicked];
                    if (_cityInfoPopupsOpen[city]) return;
                    _cityInfoPopupsOpen[city] = true;
                    
                    Vector3 hudVec = clicked.transform.position;
                    hudVec.y += 5;
                    GameObject info = Instantiate(cityInfoPrefab, hudVec, Quaternion.identity);
                    CityInfoPopup cip = info.GetComponent<CityInfoPopup>();
                    cip.SendData(city);
                    cip.Build();
                    _cityInfoPopups[cip.GetCloseButton()] = info;
                }
            }
        }
    }
}
