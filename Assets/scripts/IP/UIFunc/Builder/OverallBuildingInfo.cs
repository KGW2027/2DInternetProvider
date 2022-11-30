﻿using IP.Objective.Builds;
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
            buildName.SetUIText(_buildInfo.GetName());
            spend.SetUIText($"1달 소비 비용 : {_buildInfo.GetBudget() / _buildInfo.GetBuildDate():n0F}k$");
            complete.SetUIText($"예정 완공일 : {_buildInfo.GetBuildDate()}");
            cityName.SetUIText($"건설중인 도시 명 : {_buildInfo.GetCity().Name}");
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