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
        private int _money = 100;
        private int _changeMoney = 0;
        private string _name;

        private LottoManager _lottoManager;
        private ScreenManager _screenManager;
    
        // Start is called before the first frame update
        void Start()
        {
            updateDateText(true); // Initial Set
            _lottoManager = FindObjectOfType<LottoManager>();
            _screenManager = screenManager.GetComponent<ScreenManager>();
            Timing.RunCoroutine(RunTimer());
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
                updateDateText(true);
                _lottoManager.NextMonth();
            }
            else
            {
                updateDateText(false);
            }


            yield return Timing.WaitForSeconds(1.0f);
            Timing.RunCoroutine(RunTimer());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void updateDateText(bool dateChanged)
        {
            if(dateChanged) datePrint.GetComponent<TextMeshProUGUI>().text = $"{_year}Y {_month:00}M";
            remainDatePrint.GetComponent<TextMeshProUGUI>().text = $"~ {_remainNextMonth / 60:00}:{_remainNextMonth % 60:00}";
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
