using System.Collections.Generic;
using IP.Objective.Builds;
using IP.UIFunc.Builder;
using UnityEngine;

namespace IP.Control
{
    public class InfraManager : MonoBehaviour, ISubUI
    {
        private static bool _needUpdate = true;
        
        public GameObject scrollContent;
        public GameObject buildPrefab;
        public GameObject infraConfirmPopup;

        void Start()
        {
            PopupManager.Instance.RegisterPopup("ConstructConfirm", infraConfirmPopup);
        }
        
        public void UpdateUI()
        {
            if (_needUpdate)
            {
                foreach(Transform child in scrollContent.transform) Destroy(child);
                _needUpdate = false;
                
                List<BuildBase> builds = BuildBase.GetBuildInfos();
                foreach (BuildBase buildBase in builds)
                {
                    InfraBuildInfo uiBuilder = Instantiate(buildPrefab, scrollContent.transform)
                        .GetComponent<InfraBuildInfo>();
                    uiBuilder.SetBuildInfo(buildBase);
                    uiBuilder.Build();
                }
            }
        }

        public static void ReserveUpdate()
        {
            _needUpdate = true;
        }
    }
}