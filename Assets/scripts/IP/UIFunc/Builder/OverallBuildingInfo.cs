using IP.Objective.Builds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class OverallBuildingInfo : MonoBehaviour, IUIBuilder
    {
        public RawImage thumbnail;
        public TextMeshProUGUI buildName;
        public TextMeshProUGUI spend;
        public TextMeshProUGUI complete;
        public TextMeshProUGUI cityName;
        
        private BuildBase _buildInfo;
        
        public void Build()
        {
            thumbnail.texture = _buildInfo.GetTexture();
            buildName.text = _buildInfo.GetName();
            spend.text = $"1달 소비 비용 : {_buildInfo.GetBudget() / _buildInfo.GetBuildDate():n0F}k$";
            complete.text = $"예정 완공일 : {_buildInfo.GetBuildDate()}";
            cityName.text = $"건설중인 도시 명 : {_buildInfo.GetCity().Name}";
        }

        public void SendData(params object[] datas)
        {
            _buildInfo = (BuildBase) datas[0];
        }

        public void SetBuildInfo(BuildBase build)
        {
            _buildInfo = build;
        }
    }
}