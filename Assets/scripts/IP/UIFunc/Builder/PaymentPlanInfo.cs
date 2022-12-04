using System.Collections.Generic;
using System.Linq;
using IP.Objective;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class PaymentPlanInfo : MonoBehaviour, IUIBuilder
    {
        public GameObject planName;
        public GameObject planInfo;
        public GameObject planDesc;

        private PaymentPlan _plan;
        
        public void Build()
        {
            planName.SetUIText(_plan.Name);
            planInfo.SetUIText($"서비스 중인 국가 : {GetCities()}\n한 달 요금 : {_plan.Budget}$\n판매 비중 : {GetShare():F2}%");
            planDesc.SetUIText($"{StaticFunctions.Bytes.ToByteString(_plan.Bandwidth)} 대역폭, {_plan.Upload} Mbps, {_plan.Download} Mbps");
        }

        public void SendData(params object[] datas)
        {
            _plan = (PaymentPlan) datas[0];
        }

        private string GetCities()
        {
            List<string> citiesName = new List<string>();
            _plan.Cities.ForEach(city => citiesName.Add(city.Name));

            if (citiesName.Count == 0) return "서비스 지역 없음";
            if (citiesName.Count == 1) return citiesName[0];
            return citiesName.Aggregate((x, y) => $"{x}, {y}");
        }

        private float GetShare()
        {
            return 0.235f;
        }
    }
}