using System.Collections.Generic;
using IP.Objective;
using IP.UIFunc.Builder;
using UnityEngine;

namespace IP.Control
{
    public class BankManager : MonoBehaviour, ISubUI
    {
        [Header("대출 팝업")]
        public GameObject loanPopup;

        private const string LoanPopupName = "Loan";

        private List<BankInfo> _bankInfos;
        private Bank[] _banks;
        
        void Start()
        {
            _bankInfos = new List<BankInfo>();

            Bank bank1 = new Bank("밀 항회", 0.3f, 2.7f);
            Bank bank2 = new Bank("어디든 대출", 0.5f, 4.3f);
            Bank bank3 = new Bank("얼마든 OKash", 0.7f, 9.9f);

            Bank[] banks = {bank1, bank2, bank3};
            _banks = banks;
            int used = 0;
            
            foreach (Transform tf in transform)
            {
                if (tf.CompareTag("BankButton"))
                {
                    BankInfo bi = tf.GetComponent<BankInfo>();
                    bi.SendData(banks[used++]);
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
                    PopupManager.Instance.OpenPopup(LoanPopupName, bank);
                }
            }
        }

        public void UpdateUI()
        {
            foreach (var bank in _banks)
            {
                bank.CalcNewLoanSize();
            }

            _bankInfos.ForEach(bi => bi.Build());
        }
    }
}