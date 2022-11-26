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
            rank.GetComponent<TextMeshProUGUI>().text = $"{_rank:00}";
            companyName.GetComponent<TextMeshProUGUI>().text = _company.GetName();
            marketShare.GetComponent<TextMeshProUGUI>().text = $"{GetShare():F2}%";
            buildings.GetComponent<TextMeshProUGUI>().text = $"{_company.GetTotalBuilds()}";
            traffics.GetComponent<TextMeshProUGUI>().text = $"{_company.GetTotalTraffic()}";
            cities.GetComponent<TextMeshProUGUI>().text = $"{_company.GetConnectedCities().Count} / 50";
            countries.GetComponent<TextMeshProUGUI>().text = $"{_company.GetConnectedCountries()} / 5";
        }

        public void SetCompany(Company company)
        {
            _company = company;
        }

        public void SetRank(int rank)
        {
            _rank = rank;
        }

        private float GetShare()
        {
            return 0.234f;
        }
    }
}