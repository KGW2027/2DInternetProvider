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
            bankName.GetComponent<TextMeshProUGUI>().text = $"'{_targetBank.Name}'";
            bankMaxLoan.GetComponent<TextMeshProUGUI>().text = $"(한도 : {_targetBank.MaxLoanSize:n0})";
        }

        public void ConfirmLoan()
        {
            int loanSize;
            if (Int32.TryParse(inputLoan.GetComponent<TMP_InputField>().text.Trim(), out loanSize))
            {
                if (loanSize > _targetBank.MaxLoanSize) return;
                
                GameManager.Instance.AddLoan(_targetBank, loanSize);
                PopupManager.Instance.ClosePopup();
            }
        }

        public void SetBank(Bank bank)
        {
            _targetBank = bank;
        }
    }
}