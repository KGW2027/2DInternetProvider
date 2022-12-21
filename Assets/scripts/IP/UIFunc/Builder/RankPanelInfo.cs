using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class RankPanelInfo : MonoBehaviour, IUIBuilder
    {
        public TextMeshProUGUI rank;
        public TextMeshProUGUI companyName;
        public TextMeshProUGUI marketShare;
        public TextMeshProUGUI buildings;
        public TextMeshProUGUI traffics;
        public TextMeshProUGUI cities;
        public TextMeshProUGUI countries;
        
        private Company _company;
        private int _rank;
        
        public void Build()
        {
            rank.text = $"{_rank:00}";
            companyName.text = _company.Name;
            marketShare.text = $"{GetShare():F2}%";
            buildings.text = $"{_company.GetTotalBuilds()}";
            traffics.text = $"{StaticFunctions.Bytes.ToByteString(_company.GetTotalTraffic())}";
            cities.text = $"{_company.GetConnectedCities().Count} / 51";
            countries.text = $"{_company.GetConnectedCountries()} / 5";
        }

        public void SendData(params object[] datas)
        {
            _company = (Company) datas[0];
            _rank = (int) datas[1];
        }

        private double GetShare()
        {
            return (double) _company.GetTotalCustomers() / GameManager.Instance.GetTotalPeopleCount()*100;
        }
    }
}