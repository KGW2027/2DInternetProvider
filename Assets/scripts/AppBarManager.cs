using System.Collections.Generic;
using MEC;
using TMPro;
using UnityEngine;

public class AppBarManager : MonoBehaviour
{
    public GameObject datePrint;
    public GameObject remainDatePrint;
    public GameObject moneyPrint;
    public GameObject changeMoneyPrint;
    public GameObject companyNamePrint;
    public GameObject screenManager;

    private int _year = 2022;
    private int _month = 11;
    private int _remainNextMonth = 10;
    private int _money = 100;
    private int _changeMoney = 0;
    private string _name;
    
    // Start is called before the first frame update
    void Start()
    {
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

            datePrint.GetComponent<TextMeshProUGUI>().text = $"{_year}. {_month:00}";
        }

        remainDatePrint.GetComponent<TextMeshProUGUI>().text = $"{_remainNextMonth / 60:00}:{_remainNextMonth % 60:00}";

        yield return Timing.WaitForSeconds(1.0f);
        Timing.RunCoroutine(RunTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveHome()
    {
        screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.MAP);
    }
}
