using IP.Objective;
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
            rank.SetUIText($"{_rank:00}");
            companyName.SetUIText(_company.GetName());
            marketShare.SetUIText($"{GetShare():F2}%");
            buildings.SetUIText($"{_company.GetTotalBuilds()}");
            traffics.SetUIText($"{_company.GetTotalTraffic()}");
            cities.SetUIText($"{_company.GetConnectedCities().Count} / 50");
            countries.SetUIText($"{_company.GetConnectedCountries()} / 5");
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