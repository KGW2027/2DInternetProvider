using IP.Control;
using IP.Objective.Builds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IP.UIFunc.Builder
{
    public class ConstructConfirmPopup : MonoBehaviour, IUIBuilder
    {
        public GameObject thumbnail;
        public GameObject buildName;
        public GameObject buildInfo;
        public GameObject dropdown1;
        public GameObject dropdown2;

        private BuildBase _buildBase;

        private static readonly Vector3 WiremodeVector = new Vector3(-130.9f, -8.2f, .0f);
        private static readonly Vector3 DefaultVector = new Vector3(0, -8.2f, .0f);

        public void Build()
        {
            thumbnail.GetComponent<RawImage>().texture = _buildBase.GetTexture();
            buildName.SetUIText(_buildBase.GetName());
            buildInfo.SetUIText($"예정 완공일 : {_buildBase.GetBuildDate()}\n예상 월 소비 비용 : {_buildBase.GetBudget() / _buildBase.GetBuildDate()}k$");
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

        private string GetSelected(GameObject obj)
        {
            TMP_Dropdown dropdown = obj.GetComponent<TMP_Dropdown>();
            return dropdown.options[dropdown.value].text;
        }

        private void SetDropdownMode(bool isWire)
        {
            if (isWire)
            {
                dropdown1.transform.SetLocalPositionAndRotation(WiremodeVector, Quaternion.identity);
                dropdown2.SetActive(true);
            }
            else
            {
                dropdown2.SetActive(false);
                dropdown1.transform.SetLocalPositionAndRotation(DefaultVector, Quaternion.identity);
            }
        }
    }
}