using IP.Control;
using IP.Objective.Builds;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class ConstructConfirmPopup : MonoBehaviour, IUIBuilder
    {
        public GameObject thumbnail;
        public GameObject buildName;
        public GameObject buildInfo;

        private BuildBase _buildBase;
        
        public void Build()
        {
            thumbnail.GetComponent<RawImage>().texture = _buildBase.GetTexture();
            StaticFunctions.SetUIText(buildName, _buildBase.GetName());
            StaticFunctions.SetUIText(buildInfo, $"예정 완공일 : {_buildBase.GetBuildDate()}\n예상 월 소비 비용 : {_buildBase.GetBudget() / _buildBase.GetBuildDate()}k$");
        }

        public void SendData(params object[] datas)
        {
            _buildBase = (BuildBase) datas[0];
        }

        public void Confirm()
        {
            Debug.Log($"{_buildBase.GetName()} 건축 시작");
            PopupManager.Instance.ClosePopup();
        }
    }
}