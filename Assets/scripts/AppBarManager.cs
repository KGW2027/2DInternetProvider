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

    private int _year = 22;
    private int _month = 11;
    private int _remainNextMonth = 10;
    private int _money = 100;
    private int _changeMoney = 0;
    private string _name;
    
    // Start is called before the first frame update
    void Start()
    {
        updateDateText(true); // Initial Set
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
        screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.MAP);
    }

    public void MoveScreen(string type) {
        switch(type) {
            case "overall":
                screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.OVERALL);
                break;
            case "infra":
                screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.INFRA);
                break;
            case "plan":
                screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.PLAN);
                break;
            case "shop":
                screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.SHOP);
                break;
            case "bank":
                screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.BANK);
                break;
            case "rank":
                screenManager.GetComponent<ScreenManager>().MoveCamara(ScreenManager.ScreenType.RANK);
                break;
        }
    }
}
