using System.Collections.Generic;
using IP.Objective.Builds;
using IP.UIFunc.Builder;
using UnityEngine;

namespace IP.Control
{
    public class InfraManager : MonoBehaviour, ISubUI
    {
        public GameObject scrollContent;
        public GameObject buildPrefab;

        public void UpdateUI()
        {
            List<BuildBase> builds = BuildBase.GetBuildInfos();
            foreach (BuildBase buildBase in builds)
            {
                InfraBuildInfo uiBuilder = Instantiate(buildPrefab, scrollContent.transform).GetComponent<InfraBuildInfo>();
                uiBuilder.SetBuildInfo(buildBase);
                uiBuilder.Build();
            }
        }
    }
}