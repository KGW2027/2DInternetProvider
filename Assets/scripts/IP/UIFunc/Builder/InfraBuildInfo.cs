using IP.Control;
using IP.Objective.Builds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class InfraBuildInfo : MonoBehaviour, IUIBuilder
    {
        public RawImage thumbnail;
        public TextMeshProUGUI buildName;
        public TextMeshProUGUI spend;
        public TextMeshProUGUI complete;
        
        private BuildBase _buildInfo;
        private const string ConfirmPopup = "ConstructConfirm";
        
        public void Build()
        {
            thumbnail.texture = _buildInfo.GetTexture();
            buildName.text = _buildInfo.GetName();
            if (_buildInfo.IsWire()) spend.text = $"건설 예산 : 거리 1당 {_buildInfo.GetBudget()}k$";
            else spend.text = $"건설 예산 : {_buildInfo.GetBudget():n0}k$";

            if (_buildInfo.IsWire()) complete.text = $"예정 건설 기한 : 거리 {1 / _buildInfo.GetBuildDate():N0}당 1개월";
            else complete.text = $"예정 건설 기한 : {_buildInfo.GetBuildDate()}개월";
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