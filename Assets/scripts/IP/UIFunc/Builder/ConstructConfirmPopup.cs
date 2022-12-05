using IP.Control;
using IP.Objective.Builds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class ConstructConfirmPopup : MonoBehaviour, IUIBuilder
    {
        public RawImage thumbnail;
        public TextMeshProUGUI buildName;
        public TextMeshProUGUI buildInfo;
        public TMP_Dropdown dropdown1;
        public TMP_Dropdown dropdown2;

        private BuildBase _buildBase;

        private static readonly Vector3 WiremodeVector = new(-130.9f, -8.2f, .0f);
        private static readonly Vector3 DefaultVector = new(0, -8.2f, .0f);

        public void Build()
        {
            thumbnail.texture = _buildBase.GetTexture();
            buildName.text = _buildBase.GetName();
            buildInfo.text = $"예정 완공일 : {_buildBase.GetBuildDate()}\n예상 월 소비 비용 : {_buildBase.GetBudget() / _buildBase.GetBuildDate()}k$";
        }

        public void SendData(params object[] datas)
        {
            _buildBase = (BuildBase) datas[0];
            SetDropdownMode(_buildBase.IsWire());
        }

        public void Confirm()
        {
            Debug.Log($"{_buildBase.GetName()} 건축 시작 => {GetSelected(dropdown1)}, {GetSelected(dropdown2)}");
            PopupManager.Instance.ClosePopup();
        }

        private string GetSelected(TMP_Dropdown obj)
        {
            return obj.options[obj.value].text;
        }

        private void SetDropdownMode(bool isWire)
        {
            if (isWire)
            {
                dropdown1.transform.SetLocalPositionAndRotation(WiremodeVector, Quaternion.identity);
                dropdown2.gameObject.SetActive(true);
            }
            else
            {
                dropdown2.gameObject.SetActive(false);
                dropdown1.transform.SetLocalPositionAndRotation(DefaultVector, Quaternion.identity);
            }
        }
    }
}