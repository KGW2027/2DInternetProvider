using System;
using System.Collections.Generic;
using IP.Control;
using IP.Objective;
using IP.Objective.Builds;
using IP.Screen;
using IP.UIFunc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IP
{
    /**
     * 게임의 전체적 흐름 및 주요 변수에 접근 가능한 클래스
     */
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance { get; private set; }
        public static int PenaltyTrust = -50;

        private const int StartYear = 22;
        private const int StartMonth = 11;
        private const long StartMoney = 1000L;

        private WorldMapInteraction _wmi;
        private WorldMapCameraController _wmcc;
        private LottoManager _lotto;
        private bool _executedLateStart;

        public Company Company { get; private set; }

        void Start()
        {
            Instance = this;
        }

        /**
         * WorldMapInteractor에서 도시/국가 Load가 끝나면 실행되는 함수.
         * 본사 건물 및 AI회사 생성
         */
        public void LateStart()
        {
            _lotto = FindObjectOfType<LottoManager>();
            _wmi = FindObjectOfType<WorldMapInteraction>();
            _wmcc = FindObjectOfType<WorldMapCameraController>();
            
            City startCity = _wmi.Cities[Random.Range(0, _wmi.Cities.Count)];
            PaymentPlan plan = new PaymentPlan(Company);
            plan.Name = "Default plan";
            plan.Bandwidth = 1000;
            plan.Budget = 8;
            plan.Upload = 100;
            plan.Download = 100;
            plan.Service(startCity);
            
            Company.AddBuild(startCity, new HeadOffice());
            Company.AddBuild(startCity, new IDCSmall());
            Company.AddPlan(plan);
            
            SetCameraFocus(startCity);
            ExecuteMonthlyEvent();
        }

        /**
         * 월별 이벤트
         */
        public void ExecuteMonthlyEvent()
        {
            _lotto.Next();
            UserEarn();
            _wmi.Cities.ForEach(city => city.PlanSelect());
            AppBarManager.Instance.Refresh();
        }

        private void UserEarn()
        {
            long planRevenue = Company.CalcRevenue() / 1000;
            long used = Company.CalcInterest() + Company.CalcMaintenance();
            Company.Earn(planRevenue - used);
        }
        
        /**
         * TitleScene에서 회사 이름이 설정되면 유저 회사를 먼저 생성하는 함수
         */
        public void InitGame(String name)
        {
            Company = new Company(name, StartMoney);
        }

        /**
         * WorldMapCamera가 특정 도시에게  초점을 맞추도록 하는 함수
         */
        public void SetCameraFocus(City city)
        {
            _wmcc.SetCameraFocus(city.Button.transform.position);
        }

        /**
         * 회사 창립 연도/월을 알려주는 함수
         */
        public int[] GetStartDate()
        {
            return new[] {StartYear, StartMonth};
        }

        /**
         * 회사에 대출 이력을 추가하는 함수
         */
        public void AddLoan(Bank bank, long money)
        {
            Debt loan = new Debt();
            loan.Interest = bank.Interest;
            loan.Scale = money;
            loan.FromBank = bank;

            string[] date = AppBarManager.Instance.GetDate().Split("Y");
            loan.StartYear = Int32.Parse(date[0]);
            loan.StartMonth = Int32.Parse(date[1].Substring(0, date[1].Length - 1));
            Company.AddLoan(loan);

            AppBarManager.Instance.Refresh();
        }
        
        /**
         * 현재 게임에 로드된 국가의 리스트를 반환하는 함수
         */
        public List<Country> GetCountries()
        {
            return _wmi.Countries;
        }
    }
}
