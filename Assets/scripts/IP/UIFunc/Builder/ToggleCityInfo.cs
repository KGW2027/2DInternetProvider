using IP.Objective;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class ToggleCityInfo : MonoBehaviour, IUIBuilder
    {
        [Header("체크박스")]
        public Toggle toggle;
        [Header("도시 이름 텍스트")]
        public TextMeshProUGUI cityName;

        private City _city;
        
        public void Build()
        {
            toggle.isOn = false;
            toggle.interactable = false;
            cityName.text = _city.Name;
        }

        public void SendData(params object[] datas)
        {
            _city = (City) datas[0];
        }

        public void SetEnabled()
        {
            toggle.isOn = true;
            toggle.interactable = true;
        }
    }
}