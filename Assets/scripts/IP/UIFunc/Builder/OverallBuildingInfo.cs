using IP.Objective;
using IP.Objective.Builds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class OverallBuildingInfo : MonoBehaviour, IUIBuilder
    {
        public GameObject thumbnail;
        public GameObject buildName;
        public GameObject spend;
        public GameObject complete;
        public GameObject cityName;
        
        private BuildBase _buildInfo;
        
        public void Build()
        {
            thumbnail.GetComponent<RawImage>().texture = _buildInfo.GetTexture();
            StaticFunctions.SetUIText(buildName, _buildInfo.GetName());
            StaticFunctions.SetUIText(spend, $"1달 소비 비용 : {_buildInfo.GetBudget() / _buildInfo.GetBuildDate():n0F}k$");
            StaticFunctions.SetUIText(complete, $"예정 완공일 : {_buildInfo.GetBuildDate()}");
            StaticFunctions.SetUIText(cityName, $"건설중인 도시 명 : {_buildInfo.GetCity().Name}");
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