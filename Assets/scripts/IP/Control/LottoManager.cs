using System;
using System.Linq;
using IP.Objective;
using UnityEngine;
using Random = System.Random;

namespace IP.Control
{
    public class LottoManager : MonoBehaviour
    {
        public GameObject title;
        public GameObject myNumbers;
        public GameObject winNumbers;
        public GameObject temptText;
        public GameObject alertText;

        private Lotto _lotto;
        private long _cumulPrice;
        private long _yourPrice;
        private int _attempt;
        private int _lastBoughtAttempt;
        private byte[] _myNumbers;
        private byte[] _winNumbers;
        private byte[] _winners;
        private bool _bought;

        private const int People = 200000000;
        private const int Price = 3;
        
        void Start()
        {
            _lotto = new Lotto();
            _attempt = 0;
            _lastBoughtAttempt = 0;
            _winNumbers = new byte[6];
            Next();
        }
        
        public void Next()
        {
            long boughter = new Random().Next(People / 2, People);
            _cumulPrice += boughter * Price;
            _winNumbers = _lotto.NewWinNumbers();
            _yourPrice = 0;
            CalcWinners(boughter);
            
            if (_myNumbers != null)
            {
                int match = CheckWin();
                UpdateAlertText(match);
            }
            
            long buffer = _cumulPrice;
            if (_winners[0] > 0) buffer -= (long) (_cumulPrice * 0.8d);
            if (_winners[1] > 0) buffer -= (long) (_cumulPrice * 0.15d);
            if (_winners[2] > 0) buffer -= (long) (_cumulPrice * 0.05d);
            _cumulPrice = buffer;

            ResetUI();
            _attempt++;
            UpdateTemptText();
        }
        
        public void ResetUI()
        {
            _bought = false;
            title.SetUIText($"복권 제{_attempt}회차 당첨 번호 / 마지막 복권 구매 회차 : {_lastBoughtAttempt}");
            myNumbers.SetUIText($"복권을 구매하세요.");
            winNumbers.SetUIText($"{ParseNumbers(_winNumbers)}");
        }

        public void Buy()
        {
            if (!_bought && GameManager.Instance.UseMoney(3))
            {
                _myNumbers = _lotto.BoughtNumbers();
                _lastBoughtAttempt = _attempt;
                myNumbers.SetUIText($"{ParseNumbers(_myNumbers)}");
                _bought = true;
            }
        }

        public void GetMoney()
        {
            if (_yourPrice > 0)
            {
                Debug.Log($"{_yourPrice} 수령");
                _yourPrice = 0;
            }
        }

        private void CalcWinners(long boughter)
        {
            _winners = new byte[3];
            while (Math.Round(new Random().NextDouble(), 13) == 0.0000000000001d * boughter) _winners[0]++;
            while (Math.Round(new Random().NextDouble(), 11) == 0.00000000001d * boughter) _winners[1]++;
            while (Math.Round(new Random().NextDouble(), 9) == 0.000000001d * boughter) _winners[2]++;
        }

        private void UpdateTemptText()
        {
            temptText.SetUIText($"누적 금액 : {_cumulPrice:n0}$\n1등 상금 : 80% (지난 회차 당첨자 {_winners[0]}명)\n2등 상금 : 15% (지난 회차 당첨자 {_winners[1]}명)\n3등 상금 : 5% (지난 회차 당첨자 {_winners[2]}명)");
        }
        

        private void UpdateAlertText(int match)
        {
            long winMoney;
            int grade;
            if (match == 4)
            {
                grade = 3;
                _winners[2]++;
                winMoney = (long) (_cumulPrice * 0.05f / _winners[2]);
            }
            else if (match == 5)
            {
                grade = 2;
                _winners[1]++;
                winMoney = (long) (_cumulPrice * 0.15f / _winners[1]);
            }
            else if (match == 6)
            {
                grade = 1;
                _winners[0]++;
                winMoney = (long) (_cumulPrice * 0.8f / _winners[0]);
            }
            else
            {
                alertText.SetUIText($"제 {_attempt}회 추첨 결과는 꽝입니다.");
                return;
            }
            alertText.SetUIText($"제 {_attempt}회 추첨 결과는 {grade}등 입니다. 당첨금 : {winMoney:n0}\n이번 달이 끝나기 전에 반드시 왼쪽 아래 수령 버튼을 눌러주세요.");
            _yourPrice = winMoney;
        }

        private int CheckWin()
        {
            int win = 0;
            for (int key1 = 0; key1 < 6; key1++)
            {
                for (int key2 = key1 + 1; key2 < 6; key2++)
                {
                    if (_myNumbers[key1] == _myNumbers[key2]) win++;
                }
            }

            return win;
        }

        private string ParseNumbers(byte[] array)
        {
            if (array.Length == 0) return "결과가 없습니다.";
            return $"{array[0]} {array[1]} {array[2]} {array[3]} {array[4]} {array[5]}";
        }
        
        
    }
}
