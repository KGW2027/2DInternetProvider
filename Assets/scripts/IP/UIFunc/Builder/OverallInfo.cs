using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class OverallInfo : MonoBehaviour, IUIBuilder
    {
        [Header("기업 정보 1")]
        public TextMeshProUGUI companyName;
        public TextMeshProUGUI startDate;
        
        [Header("기업 정보 2")]
        public TextMeshProUGUI connCountries;
        public TextMeshProUGUI connCities;
        public TextMeshProUGUI qtyBuilds;
        public TextMeshProUGUI qtyCustomers;

        [Header("기업 정보 3")]
        public TextMeshProUGUI usingTraffic;
        public TextMeshProUGUI maxTraffic;
        
        [Header("기업 정보 4")]
        public TextMeshProUGUI money;
        public TextMeshProUGUI monthMoney;
        public TextMeshProUGUI debt;
        public TextMeshProUGUI useMoney;
        public TextMeshProUGUI changeMoney;
        
        
        public void Build()
        {
            Company company = GameManager.Instance.Company;
            companyName.text = company.Name;
            startDate.text = $"창립 일자 : {GameManager.Instance.GetStartDate()[0]:000}Y {GameManager.Instance.GetStartDate()[1]:00}M";
            connCountries.text = $"연결된 국가 : {company.GetConnectedCountries()} / 5";
            connCities.text = $"연결된 도시 : {company.GetConnectedCities().Count} / 50";
            qtyBuilds.text = $"설치한 건물 수 : {GetBuilds(company):n0}";
            qtyCustomers.text = $"총 소비자 수 : {company.GetTotalCustomers():n0}";

            usingTraffic.text = $"이용 트래픽 : {StaticFunctions.Bytes.ToByteString(company.GetTotalTraffic())}";
            maxTraffic.text = $"최대 트래픽 : {StaticFunctions.Bytes.ToByteString(company.BandwidthAllowance)}";
            
            long revenue = company.CalcRevenue();
            long minus = (company.CalcInterest() + company.CalcMaintenance()) * 1000;
            money.text = $"보유 자금 : {company.Money*1000:n0}";
            monthMoney.text = $"월 수익 : {revenue:n0}";
            debt.text = $"총 부채 : {company.GetTotalDebtScale() * 1000:n0}" ;
            useMoney.text = $"월 지출 : {minus:n0}";
            changeMoney.text = $"총 변동 : {(revenue - minus):n0}";
        }

        private int GetBuilds(Company comp)
        {
            int builds = 0;
            comp.GetConnectedCities().ForEach(city =>
            {
                builds += comp.GetBuilds(city).Count;
            });
            return builds;
        }

        public void SendData(params object[] datas)
        {
            
        }
    }
}