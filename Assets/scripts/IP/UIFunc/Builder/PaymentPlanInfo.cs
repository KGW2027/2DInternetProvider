using System.Collections.Generic;
using System.Linq;
using IP.Objective;
using TMPro;
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
            StaticFunctions.SetUIText(planName, _plan.Name);
            StaticFunctions.SetUIText(planInfo, $"서비스 중인 국가 : {GetCities()}\n한 달 요금 : {_plan.Budget}$\n판매 비중 : {GetShare():F2}%");
            StaticFunctions.SetUIText(planDesc, $"{_plan.Bandwidth}GB 대역폭, {_plan.Upload} Mbps, {_plan.Download} Mbps");
        }

        public void SendData(params object[] datas)
        {
            _plan = (PaymentPlan) datas[0];
        }

        private string GetCities()
        {
            List<string> citiesName = new List<string>();
            return citiesName.Aggregate((x, y) => $"{x}, {y}");
        }

        private float GetShare()
        {
            return 0.235f;
        }
    }
}