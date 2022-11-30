using System.Collections.Generic;
using IP.Control;
using MEC;
using UnityEngine;

namespace IP.Screen
{
    public class AppBarManager : MonoBehaviour
    {
        public static AppBarManager Instance;
        
        [Header("게임 내 시간 표시")]
        public GameObject datePrint;
        public GameObject remainDatePrint;
        public GameObject realDatePrint;
        [Header("회사 자산 표시")]
        public GameObject moneyPrint;
        public GameObject changeMoneyPrint;
        [Header("회사 이름 표시")]
        public GameObject companyNamePrint;
        [Header("스크린 관리")]
        public GameObject screenManager;

        private int _year;
        private int _month;
        private int _remainNextMonth = 10;
        private long _changeMoney = 0;

        private ScreenManager _screenManager;
        private CoroutineHandle _timerHandle;
    
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            _year = GameManager.Instance.GetStartDate()[0];
            _month = GameManager.Instance.GetStartDate()[1];
            
            UpdateDateText(true); // Initial Set
            _screenManager = screenManager.GetComponent<ScreenManager>();

            companyNamePrint.SetUIText(GameManager.Instance.Company.Name);
            UpdateMoneyText();
            _timerHandle = Timing.RunCoroutine(RunTimer());
        }

        private IEnumerator<float> RunTimer()
        {
            while (true)
            {
                if (--_remainNextMonth == 0)
                {
                    NextMonth();
                }
                else
                {
                    UpdateDateText(false);
                }

                yield return Timing.WaitForSeconds(1.0f);
            }
        }
        
        private void NextMonth()
        {
            _remainNextMonth = 10;
            if (++_month == 13)
            {
                _year++;
                _month = 1;
            }

            GameManager.Instance.ExecuteMonthlyEvent();
            UpdateDateText(true);
        }

        private void UpdateDateText(bool dateChanged)
        {
            if(dateChanged) datePrint.SetUIText($"{_year}Y {_month:00}M");
            remainDatePrint.SetUIText($"~ {_remainNextMonth / 60:00}:{_remainNextMonth % 60:00}");
        }

        private void UpdateMoneyText()
        {
            _changeMoney = GameManager.Instance.Company.GetTotalDebtInterest();
            moneyPrint.SetUIText($"{GameManager.Instance.Company.Money:n0}");
            changeMoneyPrint.SetUIText($"{(_changeMoney < 0 ? "- " : "+ ")}{_changeMoney:n0}");
            Color textColor = _changeMoney < 0
                ? new Color(255, 0, 0)
                : new Color(0, 255, 0);
            changeMoneyPrint.SetUITextColor(textColor);
        }

        /**
         * 강제로 다음 달로 넘기기 위한 함수
         */
        public void SkipMonth()
        {
            NextMonth();
        }

        /**
         * 월드맵 화면으로 복귀시키는 함수
         */
        public void MoveHome()
        {
            screenManager.GetComponent<ScreenManager>().MoveCamara("MAP");
        }

        /**
         * 메인 UI에서 아래쪽 버튼을 눌러 화면을 전환시키는 함수
         */
        public void MoveScreen(string type) {
            _screenManager.MoveCamara(type.ToUpper());
        }

        /**
         * 현재 날짜를 반환하는 함수
         */
        public string GetDate()
        {
            return $"{_year}Y{_month}M";
        }

        /**
         * 외부 변경으로 인해 UI를 업데이트 해야할 때 사용하는 함수
         */
        public void Refresh()
        {
            UpdateMoneyText();
        }
    }
}
