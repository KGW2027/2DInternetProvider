using System;
using IP.Control;
using IP.Objective;
using TMPro;
using UnityEngine;

namespace IP.UIFunc.Builder
{
    public class BankLoanPopup : MonoBehaviour, IUIBuilder
    {
        public GameObject bankName;
        public GameObject bankMaxLoan;
        public GameObject inputLoan;
        
        private Bank _targetBank;
        
        public void Build()
        {
            Debug.Log($"TARGET BANK : {_targetBank.Name}");
            bankName.GetComponent<TextMeshProUGUI>().text = $"'{_targetBank.Name}'";
            bankMaxLoan.GetComponent<TextMeshProUGUI>().text = $"(한도 : {_targetBank.MaxLoanSize:n0})";
        }

        public void ConfirmLoan()
        {
            int loanSize;
            if (Int32.TryParse(inputLoan.GetComponent<TMP_InputField>().text.Trim(), out loanSize))
            {
                if (loanSize > _targetBank.MaxLoanSize) return;
                
                Debug.Log($"{loanSize} 대출에 성공");
                PopupManager.Instance.ClosePopup();
            }
        }

        public void SetBank(Bank bank)
        {
            _targetBank = bank;
        }
    }
}