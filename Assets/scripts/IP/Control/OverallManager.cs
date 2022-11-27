using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.UIFunc;
using IP.UIFunc.Builder;
using TMPro;
using UnityEngine;

namespace IP.Control
{
    public class OverallManager : MonoBehaviour, ISubUI
    {
        public GameObject infoTextsParent;
        public GameObject buildsInfo;
        public GameObject buildInfoPrefab;

        private Dictionary<string, TextMeshProUGUI> _infoTexts;

        void Start()
        {
            _infoTexts = new Dictionary<string, TextMeshProUGUI>();
            
            foreach(Transform tf in infoTextsParent.transform)
            {
                _infoTexts[tf.name.Split("_")[1].ToUpper()] = tf.GetComponent<TextMeshProUGUI>();
            }
            
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpdateUI()
        {
            UpdateInfoTexts();
            UpdateBuildsInfo();
        }

        private void UpdateInfoTexts()
        {
            _infoTexts["NAME"].text = GameManager.Instance.GetCompanyName();
            _infoTexts["STARTDATE"].text = $"창립 일자 : {GameManager.Instance.GetStartDate()[0]:000}Y {GameManager.Instance.GetStartDate()[1]:00}M";
            _infoTexts["CONNCOUNTRY"].text = $"연결된 국가 : {GameManager.Instance.GetConnectCountries()} / 5";
            _infoTexts["CONNCITY"].text = $"연결된 도시 : {GameManager.Instance.GetConnectCities()} / 50";
            _infoTexts["BUILDS"].text = $"설치한 건물 수 : {GameManager.Instance.GetBuilds():n0}";
            _infoTexts["CUSTOMERS"].text = $"총 소비자 수 : {GameManager.Instance.GetCustomers():n0}";
            _infoTexts["MONEY"].text = $"보유 자금 : {GameManager.Instance.GetHaveMoney():n0}";
            _infoTexts["MONTHMONEY"].text = $"월 수익 : -";
            _infoTexts["DEBT"].text = $"총 부채 : {GameManager.Instance.GetDebt():n0}";
            _infoTexts["USEMONEY"].text = "월 지출 : -";
            _infoTexts["CHANGEMONEY"].text = "총 변동 : -";
        }

        private void UpdateBuildsInfo()
        {
            foreach (BuildBase build in GameManager.Instance.GetUnderConstructBuilds())
            {
                GameObject info = Instantiate(buildInfoPrefab, buildsInfo.transform, true);
                BuildsInfoBuilder(info, build);
            }
        }

        private void BuildsInfoBuilder(GameObject obj, BuildBase build)
        {
            OverallBuildingInfo obi = obj.GetComponent<OverallBuildingInfo>();
            obi.SetBuildInfo(build);
            obi.Build();
        }
    }
}
