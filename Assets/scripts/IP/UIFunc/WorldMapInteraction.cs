using System.Collections.Generic;
using System.Linq;
using IP.Control;
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
                        ConnectCities(startCity, endCity);
                    }
                }
            }
            MakeCustomTrees();
        }

        private void MakeCustomTrees()
        {
            string[] customTargets =
            {
                "Luoes", 
                "Olkcaster", "Yhoupset", "Khore", "Akaling", "Amadeus",
                "Ruftol", "Vrophis", "Ogabus", "Playbridge", "Agoland",
                "Esterrith", "Sastead", "Chetin", "Qufland", "Wramore",
                "Izlumore", "Hiavine", "Nalo", "Badledo"
            };
            Dictionary<string, City> customMaps = new Dictionary<string, City>();
            foreach (City city in _mapCityDictionary.Values)
            {
                if (customTargets.Contains(city.Name)) customMaps[city.Name] = city;
            }
            
            // 8시 3국가 삼각형
            ConnectCities(customMaps[customTargets[0]], customMaps[customTargets[1]]);
            ConnectCities(customMaps[customTargets[1]], customMaps[customTargets[2]]);
            ConnectCities(customMaps[customTargets[2]], customMaps[customTargets[0]]);

            // 12시 연결
            ConnectCities(customMaps[customTargets[5]], customMaps[customTargets[6]]);
            ConnectCities(customMaps[customTargets[5]], customMaps[customTargets[7]]);
            ConnectCities(customMaps[customTargets[5]], customMaps[customTargets[12]]);
            
            // 12시 숏컷
            ConnectCities(customMaps[customTargets[13]], customMaps[customTargets[14]]);
            ConnectCities(customMaps[customTargets[13]], customMaps[customTargets[15]]);
            
            // 중앙 3시
            ConnectCities(customMaps[customTargets[16]], customMaps[customTargets[8]]);
            
            // 3시 
            ConnectCities(customMaps[customTargets[8]], customMaps[customTargets[9]]);
            
            // 6시 숏컷
            ConnectCities(customMaps[customTargets[10]], customMaps[customTargets[11]]);
            
            // 6시 중앙 연결
            ConnectCities(customMaps[customTargets[3]], customMaps[customTargets[4]]);
            
            // 6시 3시
            ConnectCities(customMaps[customTargets[10]], customMaps[customTargets[18]]);
            ConnectCities(customMaps[customTargets[10]], customMaps[customTargets[11]]);
            
            // 6시 9시
            ConnectCities(customMaps[customTargets[1]], customMaps[customTargets[3]]);
            ConnectCities(customMaps[customTargets[1]], customMaps[customTargets[17]]);
            
            // 중앙
            ConnectCities(customMaps[customTargets[19]], customMaps[customTargets[4]]);
            ConnectCities(customMaps[customTargets[19]], customMaps[customTargets[12]]);
            

        }

        /**
         * 연결 생성
         */
        private void ConnectCities(City c1, City c2)
        {
            LineRenderer line = Instantiate(lineDrawer).GetComponent<LineRenderer>();
            line.startWidth = 1.0f;
            line.endWidth = 1.0f;
            line.useWorldSpace = true;
            Vector3 scv = c1.Button.transform.position;
            Vector3 ecv = c2.Button.transform.position;
            line.SetPositions(new []{scv, ecv});
            float distance = Vector3.Distance(scv, ecv);
            Connections[c1].Add(new Connection(c2, distance, line));
            Connections[c2].Add(new Connection(c1, distance, line));
            
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
            if (PopupManager.Instance.IsPopupOpen()) return; // 팝업이 열려있는 동안에는 상호작용하지 않는다.
            
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
