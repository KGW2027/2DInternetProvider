using IP.Objective;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class OverallInfo : MonoBehaviour, IUIBuilder
    {
        public GameObject companyName;
        public GameObject startDate;
        public GameObject connCountries;
        public GameObject connCities;
        public GameObject qtyBuilds;
        public GameObject qtyCustomers;
        public GameObject money;
        public GameObject monthMoney;
        public GameObject debt;
        public GameObject useMoney;
        public GameObject changeMoney;
        
        
        public void Build()
        {
            Company company = GameManager.Instance.Company;
            companyName.SetUIText(company.Name);
            startDate.SetUIText($"창립 일자 : {GameManager.Instance.GetStartDate()[0]:000}Y {GameManager.Instance.GetStartDate()[1]:00}M");
            connCountries.SetUIText($"연결된 국가 : {company.GetConnectedCountries()} / 5");
            connCities.SetUIText($"연결된 도시 : {company.GetConnectedCities().Count} / 50");
            qtyBuilds.SetUIText($"설치한 건물 수 : {GetBuilds(company):n0}");
            qtyCustomers.SetUIText($"총 소비자 수 : {company.GetTotalCustomers():n0}");
            
            long revenue = company.CalcRevenue();
            long minus = (company.CalcInterest() + company.CalcMaintenance()) * 1000;
            money.SetUIText($"보유 자금 : {company.Money*1000:n0}");
            monthMoney.SetUIText($"월 수익 : {revenue:n0}");
            debt.SetUIText($"총 부채 : {company.GetTotalDebtScale() * 1000:n0}" );
            useMoney.SetUIText($"월 지출 : {minus:n0}");
            changeMoney.SetUIText($"총 변동 : {(revenue - minus):n0}");
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