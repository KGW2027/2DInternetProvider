using System.Collections.Generic;
using System.Linq;
using IP.AI;
using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class CityInfoPopup : MonoBehaviour, IUIBuilder
    {
        [Header("도시 이름")]
        public TextMeshPro cityName;

        [Header("도시 정보")]
        public TextMeshPro cityInternetUsage;
        public TextMeshPro cityPeople;
        public TextMeshPro cityUsingPlan;
        public TextMeshPro cityCompanies;
        public TextMeshPro cityPlans;

        [Header("도시 내 내 기업 정보")] 
        public TextMeshPro cityBuilds;
        public TextMeshPro cityTrafficAllows;
        public TextMeshPro citySpeedAllows;

        [Header("닫기 버튼")] public Collider2D closeButton;

        private City _city;
        public void Build()
        {
            cityName.text = $"[{_city.Name}]";
            cityInternetUsage.text = $"인터넷 사용량 : {_city.ActiveRate * 100:F2}%";
            cityPeople.text = $"총 인구수 : {_city.People:N0}명";
            cityUsingPlan.text = GetUsingPlanText();
            cityCompanies.text = $"활동중인 회사 : {GetCompaniesText()}";
            cityPlans.text = $"선택가능한 요금제 개수 : {_city.ServicingPlans.Count:N0}개";

            cityBuilds.text = $"설치한 건물 : {GetMyBuildsText()}";
            
            cityTrafficAllows.text = $"허용 트래픽 : {StaticFunctions.Bytes.ToByteString(GameManager.Instance.Company.BandwidthAllowance)}";
            citySpeedAllows.text = $"허용 대역 : {GameManager.Instance.Company.UpDownSpeed:N0}Mbps";
        }

        public Collider2D GetCloseButton()
        {
            return closeButton;
        }

        public void SendData(params object[] datas)
        {
            _city = (City) datas[0];
        }

        private string GetUsingPlanText()
        {;
            string usingPlanText = "";
            Dictionary<PaymentPlan, double> shareDict = new Dictionary<PaymentPlan, double>();
            _city.ServicingPlans.ForEach(plan =>
            {
                long customers = _city.GetCustomer(plan);
                if (customers > 0) shareDict[plan] = (customers / (double) _city.People) * 100;
            });
            using var sortedDict = (from entry in shareDict orderby entry.Value descending select entry).GetEnumerator();
            while (sortedDict.MoveNext())
            {
                usingPlanText +=
                    $"- {sortedDict.Current.Value:F2}%의 사람들이 [{sortedDict.Current.Key.OwnerCompany.Name}]의 {sortedDict.Current.Key.Name} 이용 중\n";
            }

            return usingPlanText;
        }

        private string GetCompaniesText()
        {
            List<string> companies = new List<string>();
            if(GameManager.Instance.Company.GetConnectedCities().Contains(_city)) companies.Add(GameManager.Instance.Company.Name);
            AIManager.Instance.Companies.ForEach(company =>
            {
                if(company.GetConnectedCities().Contains(_city)) companies.Add(company.Name);
            });

            if (companies.Count == 0) return "없음";
            if (companies.Count == 1) return companies[0];
            if (companies.Count >= 2)
            {
                string defText = $"{companies[0]}, {companies[1]}";
                if (companies.Count > 2)
                {
                    defText += $" 외 {companies.Count - 2}개";
                }
                return defText;
            }

            return null;
        }

        private string GetMyBuildsText()
        {
            List<string> builds = new List<string>();
            Company my = GameManager.Instance.Company;
            if (my.GetBuilds(_city) != null)
            {
                my.GetBuilds(_city).ForEach(build => builds.Add(build.GetName()));

                switch (builds.Count)
                {
                    case 0:
                        return "없음";
                    case 1:
                        return builds[0];
                    case 2:
                    case 3:
                    case 4:
                        return builds.Aggregate((x, y) => $"{x}, {y}");
                    default:
                        return $"{builds[0]}, {builds[1]}, {builds[2]} 외 {builds.Count - 3}개";
                }
            }

            return "없음";

        }
    }
}