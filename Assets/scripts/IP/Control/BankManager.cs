using System.Collections.Generic;
using IP.Objective;
using IP.UIFunc;
using IP.UIFunc.Builder;
using UnityEngine;

namespace IP.Control
{
    public class BankManager : MonoBehaviour, ISubUI
    {
        public GameObject loanPopup;

        private const string LoanPopupName = "Loan";

        private List<BankInfo> _bankInfos;
        
        
        void Start()
        {
            _bankInfos = new List<BankInfo>();

            Bank bank1 = new Bank("밀 항회", 2.5f, 2.7f);
            Bank bank2 = new Bank("어디든 대출", 4.0f, 4.3f);
            Bank bank3 = new Bank("얼마든 OKash", 7.0f, 9.9f);

            Bank[] banks = {bank1, bank2, bank3};
            int used = 0;
            
            foreach (Transform tf in transform)
            {
                if (tf.CompareTag("BankButton"))
                {
                    BankInfo bi = tf.GetComponent<BankInfo>();
                    bi.SetBank(banks[used++]);
                    _bankInfos.Add(bi);
                    
                }
            }
            
            PopupManager.Instance.RegisterPopup(LoanPopupName, loanPopup);
        }

        void Update()
        {
            if (gameObject.activeInHierarchy && !PopupManager.Instance.IsPopupOpen() && Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main!.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
                if (hit.transform != null && hit.transform.CompareTag("BankButton"))
                {
                    Bank bank = hit.transform.GetComponent<BankInfo>().GetBank();
                    loanPopup.GetComponent<BankLoanPopup>().SetBank(bank);
                    PopupManager.Instance.OpenPopup(LoanPopupName);
                }
            }
        }

        public void UpdateUI()
        {
            _bankInfos.ForEach(bi => bi.Build());
        }
    }
}