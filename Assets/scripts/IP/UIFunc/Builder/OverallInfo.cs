using TMPro;
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
            companyName.SetUIText(GameManager.Instance.GetCompanyName());
            startDate.SetUIText($"창립 일자 : {GameManager.Instance.GetStartDate()[0]:000}Y {GameManager.Instance.GetStartDate()[1]:00}M");
            connCountries.SetUIText($"연결된 국가 : {GameManager.Instance.GetConnectCountries()} / 5");
            connCities.SetUIText($"연결된 도시 : {GameManager.Instance.GetConnectCities()} / 50");
            qtyBuilds.SetUIText($"설치한 건물 수 : {GameManager.Instance.GetBuilds():n0}");
            qtyCustomers.SetUIText($"총 소비자 수 : {GameManager.Instance.GetCustomers():n0}");
            money.SetUIText($"보유 자금 : {GameManager.Instance.GetHaveMoney():n0}");
            monthMoney.SetUIText($"월 수익 : -");
            debt.SetUIText($"총 부채 : {GameManager.Instance.GetDebt():n0}" );
            useMoney.SetUIText("월 지출 : -");
            changeMoney.SetUIText("총 변동 : -");
        }

        public void SendData(params object[] datas)
        {
            
        }
    }
}