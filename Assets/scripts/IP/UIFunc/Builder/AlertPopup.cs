using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class AlertPopup : MonoBehaviour, IUIBuilder
    {
        public TextMeshProUGUI main;
        public TextMeshProUGUI sub;

        private string _mainText;
        private string _subText;
        public void Build()
        {
            main.text = _mainText;
            sub.text = _subText;
        }

        public void SendData(params object[] datas)
        {
            _mainText = (string) datas[0];
            _subText = (string) datas[1];
        }
    }
}