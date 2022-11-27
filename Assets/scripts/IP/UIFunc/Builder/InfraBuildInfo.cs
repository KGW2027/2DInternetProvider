using IP.Control;
using IP.Objective;
using IP.Objective.Builds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class InfraBuildInfo : MonoBehaviour, IUIBuilder
    {
        public GameObject thumbnail;
        public GameObject buildName;
        public GameObject spend;
        public GameObject complete;
        
        private BuildBase _buildInfo;
        private const string ConfirmPopup = "ConstructConfirm";
        
        public void Build()
        {
            thumbnail.GetComponent<RawImage>().texture = _buildInfo.GetTexture();
            StaticFunctions.SetUIText(buildName,  _buildInfo.GetName());
            StaticFunctions.SetUIText(spend, $"건설 예산 : {_buildInfo.GetBudget():n0}k$");
            StaticFunctions.SetUIText(complete, $"예정 건설 기한 : {_buildInfo.GetBuildDate()}개월");
        }

        public void SendData(params object[] datas)
        {
            _buildInfo = (BuildBase) datas[0];
        }

        public void SetBuildInfo(BuildBase build)
        {
            _buildInfo = build;
        }

        public void OpenConstructPopup()
        {
            PopupManager.Instance.OpenPopup(ConfirmPopup, _buildInfo);
        }
    }
}