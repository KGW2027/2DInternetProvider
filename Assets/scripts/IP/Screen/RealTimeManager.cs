using System;
using System.Collections.Generic;
using MEC;
using TMPro;

namespace IP.Screen
{
    public class RealTimeManager
    {

        private TextMeshProUGUI _textUI;

        public RealTimeManager(TextMeshProUGUI text)
        {
            _textUI = text;
        }

        public void Run()
        {
            Timing.RunCoroutine(RealTimeTimerCoroutine());
        }

        private IEnumerator<float> RealTimeTimerCoroutine()
        {
            int initSec = DateTime.Now.Second;
            PrintNowDate();
            yield return Timing.WaitForSeconds(60 - initSec);
            while (true)
            {
                PrintNowDate();
                yield return Timing.WaitForSeconds(60.0f);
            }
        }

        private void PrintNowDate()
        {
            _textUI.text = DateTime.Now.ToString("tt hh:mm");
        }

    }
}