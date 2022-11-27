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
            StaticFunctions.SetUIText(companyName, GameManager.Instance.GetCompanyName());
            StaticFunctions.SetUIText(startDate, $"창립 일자 : {GameManager.Instance.GetStartDate()[0]:000}Y {GameManager.Instance.GetStartDate()[1]:00}M");
            StaticFunctions.SetUIText(connCountries, $"연결된 국가 : {GameManager.Instance.GetConnectCountries()} / 5");
            StaticFunctions.SetUIText(connCities, $"연결된 도시 : {GameManager.Instance.GetConnectCities()} / 50");
            StaticFunctions.SetUIText(qtyBuilds, $"설치한 건물 수 : {GameManager.Instance.GetBuilds():n0}");
            StaticFunctions.SetUIText(qtyCustomers, $"총 소비자 수 : {GameManager.Instance.GetCustomers():n0}");
            StaticFunctions.SetUIText(money, $"보유 자금 : {GameManager.Instance.GetHaveMoney():n0}");
            StaticFunctions.SetUIText(monthMoney, $"월 수익 : -");
            StaticFunctions.SetUIText(debt, $"총 부채 : {GameManager.Instance.GetDebt():n0}" );
            StaticFunctions.SetUIText(useMoney, "월 지출 : -");
            StaticFunctions.SetUIText(changeMoney, "총 변동 : -");
        }

        public void SendData(params object[] datas)
        {
            
        }
    }
}