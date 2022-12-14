using System.Collections.Generic;
using System.Linq;
using IP.Control;
using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class PaymentPlanInfo : MonoBehaviour, IUIBuilder
    {
        public TextMeshProUGUI planName;
        public TextMeshProUGUI planInfo;
        public TextMeshProUGUI planDesc;

        private PaymentPlan _plan;
        private PlanManager _manager;
        
        public void Build()
        {
            planName.text = _plan.Name;
            planInfo.text = $"서비스 중인 국가 : {GetCities()}\n한 달 요금 : {_plan.Budget}$\n판매 비중 : {GetShare():F2}%";
            planDesc.text = $"{StaticFunctions.Bytes.ToByteString(_plan.Bandwidth)} 대역폭, {_plan.Upload*1000:N0} Mbps, {_plan.Download*1000:N0} Mbps";
        }

        public void SendData(params object[] datas)
        {
            _plan = (PaymentPlan) datas[0];
            _manager = (PlanManager) datas[1];
        }

        public void EditPlan()
        {
            _manager.Edit(_plan);
        }

        public void OpenStat()
        {
            _manager.Open(_plan);
        }

        public void DeletePlan()
        {
            GameManager.Instance.Company.DeletePlan(_plan);
            _manager.DeletePlan(_plan);
        }

        private string GetCities()
        {
            List<string> citiesName = new List<string>();
            _plan.Cities.ForEach(city => citiesName.Add(city.Name));

            if (citiesName.Count == 0) return "서비스 지역 없음";
            if (citiesName.Count == 1) return citiesName[0];
            if (citiesName.Count > 4)
                return $"{citiesName[0]}, {citiesName[1]}, {citiesName[2]} 외 {citiesName.Count - 3}개";
            return citiesName.Aggregate((x, y) => $"{x}, {y}");
        }

        private float GetShare()
        {
            long planCustomers = 0;
            _plan.Cities.ForEach(city => planCustomers += city.GetCustomer(_plan));
            return (float) ((double) planCustomers / GameManager.Instance.Company.GetTotalCustomers() * 100);
        }
    }
}