using System.Collections.Generic;
using IP.UIFunc;
using MEC;
using TMPro;
using UnityEngine;

namespace IP.Screen
{
    public class AppBarManager : MonoBehaviour
    {
        public GameObject datePrint;
        public GameObject remainDatePrint;
        public GameObject moneyPrint;
        public GameObject changeMoneyPrint;
        public GameObject companyNamePrint;
        public GameObject screenManager;

        private int _year = 22;
        private int _month = 11;
        private int _remainNextMonth = 10;
        private long _changeMoney = 0;

        private TextMeshProUGUI _moneyText;
        private TextMeshProUGUI _changeMoneyText;
        private TextMeshProUGUI _companyNameText;
        
        private LottoManager _lottoManager;
        private ScreenManager _screenManager;
    
        // Start is called before the first frame update
        void Start()
        {
            UpdateDateText(true); // Initial Set
            _lottoManager = FindObjectOfType<LottoManager>();
            _screenManager = screenManager.GetComponent<ScreenManager>();
            Timing.RunCoroutine(RunTimer());

            _moneyText = moneyPrint.GetComponent<TextMeshProUGUI>();
            _changeMoneyText = changeMoneyPrint.GetComponent<TextMeshProUGUI>();
            _companyNameText = companyNamePrint.GetComponent<TextMeshProUGUI>();

            _companyNameText.text = GameManager.Instance.GetCompanyName();
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

        // Update is called once per frame
        void Update()
        {
        
        }

        private void UpdateDateText(bool dateChanged)
        {
            if(dateChanged) datePrint.GetComponent<TextMeshProUGUI>().text = $"{_year}Y {_month:00}M";
            remainDatePrint.GetComponent<TextMeshProUGUI>().text = $"~ {_remainNextMonth / 60:00}:{_remainNextMonth % 60:00}";
        }

        private void UpdateMoneyText()
        {
            _moneyText.text = $"{GameManager.Instance.GetHaveMoney():C0}";
            _changeMoneyText.text = $"{(_changeMoney < 0 ? "- " : "+ ")}{_changeMoney:C0}";
            if (_changeMoney < 0) _changeMoneyText.color = new Color(255, 0, 0);
            else _changeMoneyText.color = new Color(0, 255, 0);
        }

        public void MoveHome()
        {
            screenManager.GetComponent<ScreenManager>().MoveCamara("MAP");
        }

        public void MoveScreen(string type) {
            _screenManager.MoveCamara(type.ToUpper());
        }
    }
}
