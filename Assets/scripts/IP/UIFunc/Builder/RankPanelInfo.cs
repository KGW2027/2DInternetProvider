using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class RankPanelInfo : MonoBehaviour, IUIBuilder
    {
        public GameObject rank;
        public GameObject companyName;
        public GameObject marketShare;
        public GameObject buildings;
        public GameObject traffics;
        public GameObject cities;
        public GameObject countries;
        
        private Company _company;
        private int _rank;
        
        public void Build()
        {
            StaticFunctions.SetUIText(rank, $"{_rank:00}");
            StaticFunctions.SetUIText(companyName, _company.GetName());
            StaticFunctions.SetUIText(marketShare, $"{GetShare():F2}%");
            StaticFunctions.SetUIText(buildings, $"{_company.GetTotalBuilds()}");
            StaticFunctions.SetUIText(traffics, $"{_company.GetTotalTraffic()}");
            StaticFunctions.SetUIText(cities, $"{_company.GetConnectedCities().Count} / 50");
            StaticFunctions.SetUIText(countries, $"{_company.GetConnectedCountries()} / 5");
        }

        public void SendData(params object[] datas)
        {
            _company = (Company) datas[0];
            _rank = (int) datas[1];
        }

        private float GetShare()
        {
            return 0.234f;
        }
    }
}