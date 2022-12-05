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
            spend.text = $"소모 비용(1 month) : {_buildInfo.GetUseBudget():F2}k$";
            complete.text = $"예정 완공날짜 : {_buildInfo.GetEndDate()[0]:00}Y {_buildInfo.GetEndDate()[1]:00}M";
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