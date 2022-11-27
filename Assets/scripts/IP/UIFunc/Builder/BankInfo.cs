using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class BankInfo : MonoBehaviour, IUIBuilder
    {
        public GameObject bankName;
        public GameObject bankInfo;

        private Bank _bank;

        public Bank GetBank()
        {
            return _bank;
        }

        public void Build()
        {
            bankName.GetComponent<TextMeshPro>().text = $"{_bank.Name}";
            bankInfo.GetComponent<TextMeshPro>().text = $"금리 : 연 {_bank.Interest:F2}%\n 한도 : {_bank.MaxLoanSize:n0}";
        }

        public void SendData(params object[] datas)
        {
            _bank = (Bank) datas[0];
        }
    }
}