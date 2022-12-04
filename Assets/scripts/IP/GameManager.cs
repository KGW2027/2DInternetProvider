using System;
using System.Collections.Generic;
using IP.AI;
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

            City startCity;
            do
            {
                startCity = _wmi.Cities[Random.Range(0, _wmi.Cities.Count)];
            } while (startCity.IsCity);

            PaymentPlan plan = new PaymentPlan(Company);
            plan.Name = "Default plan";
            plan.Bandwidth = 1000 * StaticFunctions.Bytes.GB;
            plan.Budget = 8;
            plan.Upload = 100 * StaticFunctions.Bytes.MB;
            plan.Download = 100 * StaticFunctions.Bytes.MB;
            plan.Service(startCity);

            AIManager.Instance.Initialize(_wmi);
            
            Company.AddBuild(startCity, new HeadOffice());
            Company.AddBuild(startCity, new IDCSmall());
            Company.AddBuild(startCity, new CoaxialCable());
            Company.AddPlan(plan, startCity);
            
            SetCameraFocus(startCity);
            ExecuteMonthlyEvent();
        }

        /**
         * 월별 이벤트
         */
        public void ExecuteMonthlyEvent()
        {
            // 복권 회차 넘기기
            _lotto.Next();
            
            // 수익금 정산
            UserEarn();
            AIManager.Instance.Earn();
            
            // 회사 신뢰도 평가
            Company.CalcTrust();
            AIManager.Instance.CheckTrust();

            // 도시들의 요금제 재선택
            _wmi.Cities.ForEach(city => city.PlanSelect());
            
            // Appbar UI 새로고침
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

            int[] date = AppBarManager.Instance.GetDate();
            loan.StartYear = date[0];
            loan.StartMonth = date[1];
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

        /**
         * 게임 총 인구수
         */
        public long GetTotalPeopleCount()
        {
            long count = 0L;
            _wmi.Cities.ForEach(city =>
            {
                count += city.People;
            });
            return count;
        }
    }
}
