using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RanksManager : MonoBehaviour
{
    class CompanyInfo
    {
        public int rank;
        string _name;
        float _marketShare;
        private int _builds;
        private int _connectedCountries;
        private int _connectedCities;
        private string _traffics;
    }
    
    public GameObject panel;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateRanks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ClearChilds()
    {
        foreach (Transform child in transform)
        {
            Destroy(child);
        }
    }

    private void UpdateRanks()
    {
        ClearChilds();
        float x = 960;
        float y = -60;

        for (int panelKey = 0; panelKey < 15; panelKey++)
        {
            GameObject newPanel = Instantiate(panel, transform, true);
            CompanyInfo cInfo = new CompanyInfo();
            cInfo.rank = panelKey + 1;
            UpdatePanelDisplay(newPanel, cInfo);
        }
    }

    private void SetText(Transform textObject, string text)
    {
        textObject.GetComponent<TextMeshProUGUI>().text = text;
    }

    private void UpdatePanelDisplay(GameObject target, CompanyInfo cInfo)
    {
        foreach (Transform textField in target.transform)
        {
            string newText = "";
            switch (textField.name)
            {
                case "Text_Rank":
                    newText = $"{cInfo.rank:00}";
                    break;
                case "Text_CompanyName":
                    newText = "CompName";
                    break;
                case "Text_MarketShare":
                    newText = $"{Random.Range(.0f, 99.99f):N2}%";
                    break;
                case "Text_Buildings":
                    newText = $"{Random.Range(0, 999)}";
                    break;
                case "Text_Traffics":
                    newText = $"99.99 GB";
                    break;
                case "Text_Cities":
                    newText = $"1 / 50";
                    break;
                case "Text_Countries":
                    newText = $"1 / 5";
                    break;
            }
            if(!newText.Equals("")) SetText(textField, newText);
        }
    }
}
