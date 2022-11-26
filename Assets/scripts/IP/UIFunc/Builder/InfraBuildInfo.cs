using IP.Objective;
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
        
        private IBuild _buildInfo;
        
        public void Build()
        {
            thumbnail.GetComponent<RawImage>().texture = _buildInfo.GetTexture();
            buildName.GetComponent<TextMeshProUGUI>().text = _buildInfo.GetName();
            spend.GetComponent<TextMeshProUGUI>().text = $"건설 예산 : {_buildInfo.GetBudget():n0F}k$";
            complete.GetComponent<TextMeshProUGUI>().text = $"예정 건설 기한 : {_buildInfo.GetBuildDate()}";
        }

        public void SetBuildInfo(IBuild build)
        {
            _buildInfo = build;
        }
    }
}