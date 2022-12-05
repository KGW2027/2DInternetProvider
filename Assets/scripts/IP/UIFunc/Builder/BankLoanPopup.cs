using System;
using IP.Control;
using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class BankLoanPopup : MonoBehaviour, IUIBuilder
    {
        [Header("은행 정보")]
        public TextMeshProUGUI bankName;
        public TextMeshProUGUI bankMaxLoan;
        [Header("입력")]
        public TMP_InputField inputLoan;
        
        private Bank _targetBank;
        private BankManager _bankManager;
        
        public void Build()
        {
            bankName.text = $"'{_targetBank.Name}'";
            bankMaxLoan.text = $"(한도 : {_targetBank.MaxLoanSize:n0})";
        }

        public void SendData(params object[] datas)
        {
            _targetBank = (Bank) datas[0];
        }

        public void ConfirmLoan()
        {
            int loanSize;
            if (Int32.TryParse(inputLoan.text.Trim(), out loanSize))
            {
                if (loanSize > _targetBank.MaxLoanSize) return;
                
                GameManager.Instance.AddLoan(_targetBank, loanSize);
                _targetBank.Loaned(loanSize);
                if (_bankManager == null) _bankManager = FindObjectOfType<BankManager>();
                _bankManager.UpdateUI();
                PopupManager.Instance.ClosePopup();
            }
        }
    }
}