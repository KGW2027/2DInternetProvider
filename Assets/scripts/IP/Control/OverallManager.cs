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

        public void UpdateUI()
        {
            UpdateInfoTexts();
            UpdateBuildsInfo();
        }

        private void UpdateInfoTexts()
        {
            infoTextsParent.GetComponent<OverallInfo>().Build();
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