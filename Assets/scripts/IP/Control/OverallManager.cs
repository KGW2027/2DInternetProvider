using IP.Objective.Builds;
using IP.UIFunc.Builder;
using UnityEngine;

namespace IP.Control
{
    public class OverallManager : MonoBehaviour, ISubUI
    {
        [Header("회사 정보 표시")]
        public GameObject infoTextsParent;
        
        [Header("건설중인 건물 목록")]
        public GameObject buildsInfo;
        public GameObject buildInfoPrefab;

        void Start()
        {
            UpdateUI();
        }

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
            foreach (BuildBase build in GameManager.Instance.Company.GetUnderConstructBuilds())
            {
                if (build.GetType() == typeof(HeadOffice)) continue;
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
