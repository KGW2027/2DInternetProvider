using System.Collections.Generic;
using IP.Objective;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc
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
            foreach (IBuild build in GameManager.Instance.GetUnderConstructBuilds())
            {
                GameObject info = Instantiate(buildInfoPrefab, buildsInfo.transform, true);
                BuildsInfoBuilder(info, build);
            }
        }

        private void BuildsInfoBuilder(GameObject obj, IBuild build)
        {
            foreach (Transform tf in obj.transform)
            {
                switch (tf.name)
                {
                    case "Image_BuildingThumbnail":
                        tf.GetComponent<RawImage>().texture = build.GetTexture();
                        break;
                    case "Text_Name":
                        tf.GetComponent<TextMeshProUGUI>().text = build.GetName();
                        break;
                    case "Text_Spend":
                        tf.GetComponent<TextMeshProUGUI>().text = $"1달 소비 비용 : {build.GetBudget() / build.GetBuildDate():n0F}k$";
                        break;
                    case "Text_Complete":
                        tf.GetComponent<TextMeshProUGUI>().text = $"예정 완공일 : {build.GetBuildDate()}";
                        break;
                    case "Text_CityName":
                        tf.GetComponent<TextMeshProUGUI>().text = $"건설중인 도시 명 : {build.GetCity().Name}";
                        break;
                }
            }
        }
    }
}
