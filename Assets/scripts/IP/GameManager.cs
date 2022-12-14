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
        public static readonly int PenaltyTrust = -50;

        public bool IsGameEnd { get; private set; }

        private const int StartYear = 1;
        private const int StartMonth = 1;
        private const long StartMoney = 1000L;

        private WorldMapInteraction _wmi;
        private WorldMapCameraController _wmcc;
        private LottoManager _lotto;
        private bool _executedLateStart;
        private bool _initMonth;

        public Company Company { get; private set; }

        void Start()
        {
            Instance = this;
            _initMonth = true;
            IsGameEnd = false;
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
            } while (startCity.People > 200_000 || startCity.People < 100_000);

            PaymentPlan plan = new PaymentPlan(Company);
            plan.Name = "Default plan";
            plan.Bandwidth = 1000 * StaticFunctions.Bytes.GB;
            plan.Budget = 8;
            plan.Upload = .1 * StaticFunctions.Bytes.GB;
            plan.Download = .1 * StaticFunctions.Bytes.GB;

            AIManager.Instance.Initialize(_wmi);
            
            Company.AddBuild(startCity, new HeadOffice());
            Company.AddBuild(startCity, new IDCSmall());
            Company.AddBuild(startCity, new CoaxialCable());
            Company.AddPlan(plan, startCity);
            
            _wmi.ChangeVisibleMode(true);
            AudioManager.Instance.RunBGM();
            SetCameraFocus(startCity);
            ExecuteMonthlyEvent();
        }

        /**
         * 월별 이벤트
         */
        public void ExecuteMonthlyEvent()
        {
            // 인구 성장
            if (AppBarManager.Instance.GetDate()[1] == 1)
            {
                _wmi.Cities.ForEach(city => city.PopulationGrowth());
            }
            
            // 복권 회차 넘기기
            _lotto.Next();

            if (_initMonth)
            {
                _wmi.Cities.ForEach(city => city.PlanSelect());
            }
            
            // 수익금 정산
            UserEarn();
            AIManager.Instance.Earn();

            // 회사 신뢰도 평가
            Company.CalcTrust();
            AIManager.Instance.CheckTrust();
            
            // AI 회사 운영
            AIManager.Instance.ExecuteStrategy();

            LoseCheck();
            
            // 도시들의 요금제 재선택
            _wmi.Cities.ForEach(city => city.PlanSelect());

            if (_initMonth)
            {
                _initMonth = false;
                TutorialManager.Instance.PrintMapTutorial();
                Company.ResetMoney();
            }

            // Appbar UI 새로고침
            AppBarManager.Instance.Refresh();
        }

        private void UserEarn()
        {
            long planRevenue = Company.CalcRevenue() / 1000;
            long used = Company.GetUsingMoney();
            Company.Earn(planRevenue - used);
        }

        private void LoseCheck()
        {
            NoMoneyOver();
            LowTrustOver();
        }

        private void NoMoneyOver()
        {
            // 최근 3달 평균 수익이 적자면서, 보유 자금도 0인 경우
            if (Company.RecentRevenue(3) < 0 && Company.Money < 0)
            {
                GameOverSeq("연속된 적자로 파산");
            }
        }

        private void LowTrustOver()
        {
            // 신뢰도가 너무 많이 하락한 경우
            if (Company.Trust < PenaltyTrust * 4)
            {
                GameOverSeq("낮은 신뢰도로 운영 불가능");
            }
        }

        private void GameOverSeq(string reason)
        {
            IsGameEnd = true;
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlayOneShot(AudioManager.Audios.GameOver);
            AppBarManager.Instance.KillTimer();
            AppBarManager.Instance.OpenGameOverScreen(reason);
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

        /**
         * 특정 도시로부터 연결된 다른 도시들의 목록을 가져온다.
         */
        public List<Connection> GetConnections(City city)
        {
            return _wmi.Connections[city];
        }

        /**
         * 도시의 이름으로부터 도시 객체를 얻는다.
         */
        public City FindCity(string name)
        {
            foreach (City city in _wmi.Cities)
            {
                if (city.Name.Equals(name)) return city;
            }

            return null;
        }
    }
}
