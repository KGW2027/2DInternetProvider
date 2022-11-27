using System.Collections.Generic;
using IP.Control;
using MEC;
using TMPro;
using UnityEngine;

namespace IP.Screen
{
    public class AppBarManager : MonoBehaviour
    {
        public static AppBarManager Instance;
        
        public GameObject datePrint;
        public GameObject remainDatePrint;
        public GameObject moneyPrint;
        public GameObject changeMoneyPrint;
        public GameObject companyNamePrint;
        public GameObject screenManager;

        private int _year;
        private int _month;
        private int _remainNextMonth = 10;
        private long _changeMoney = 0;

        private LottoManager _lottoManager;
        private ScreenManager _screenManager;
    
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            _year = GameManager.Instance.GetStartDate()[0];
            _month = GameManager.Instance.GetStartDate()[1];
            
            UpdateDateText(true); // Initial Set
            _lottoManager = FindObjectOfType<LottoManager>();
            _screenManager = screenManager.GetComponent<ScreenManager>();
            Timing.RunCoroutine(RunTimer());

            StaticFunctions.SetUIText(companyNamePrint, GameManager.Instance.GetCompanyName());
            UpdateMoneyText();
        }

        IEnumerator<float> RunTimer()
        {
            if (--_remainNextMonth == 0)
            {
                _remainNextMonth = 10;
                if (++_month == 13)
                {
                    _year++;
                    _month = 1;
                }
                UpdateDateText(true);
                _lottoManager.NextMonth();
            }
            else
            {
                UpdateDateText(false);
            }


            yield return Timing.WaitForSeconds(1.0f);
            Timing.RunCoroutine(RunTimer());
        }

        private void UpdateDateText(bool dateChanged)
        {
            if(dateChanged) StaticFunctions.SetUIText(datePrint, $"{_year}Y {_month:00}M");
            StaticFunctions.SetUIText(remainDatePrint, $"~ {_remainNextMonth / 60:00}:{_remainNextMonth % 60:00}");
        }

        private void UpdateMoneyText()
        {
            _changeMoney = GameManager.Instance.GetDebtInterest();
            StaticFunctions.SetUIText(moneyPrint, $"{GameManager.Instance.GetHaveMoney():n0}");
            StaticFunctions.SetUIText(changeMoneyPrint, $"{(_changeMoney < 0 ? "- " : "+ ")}{_changeMoney:n0}");
            Color textColor = _changeMoney < 0
                ? new Color(255, 0, 0)
                : new Color(0, 255, 0);
            StaticFunctions.SetUITextColor(changeMoneyPrint, textColor);
        }

        public void MoveHome()
        {
            screenManager.GetComponent<ScreenManager>().MoveCamara("MAP");
        }

        public void MoveScreen(string type) {
            _screenManager.MoveCamara(type.ToUpper());
        }

        public string GetDate()
        {
            return $"{_year}Y{_month}M";
        }

        public void Refresh()
        {
            UpdateMoneyText();
        }
    }
}
