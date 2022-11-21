using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WorldMapInteractor : MonoBehaviour
{
    public GameObject cities;
    public GameObject countries;

    private List<string> _cityNames;
    private List<string> _countryNames;
    
    // Start is called before the first frame update
    void Start()
    {
        _cityNames = new List<string>();
        _countryNames = new List<string>();
        
        foreach (Transform city in cities.transform)
        {
            if (city.CompareTag("WorldMapButton"))
            {
                _cityNames.Add(city.transform.GetChild(0).GetComponent<TextMeshPro>().text);
            }
        }

        foreach (Transform country in countries.transform)
        {
            if (country.CompareTag("WorldMapCountryButton"))
            {
                _countryNames.Add(country.transform.GetChild(0).GetComponent<TextMeshPro>().text);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeVisibleMode(bool visibleCity)
    {
        if (visibleCity)
        {
            cities.SetActive(true);
            countries.SetActive(false);
        }
        else
        {
            cities.SetActive(false);
            countries.SetActive(true);
        }
    }

    public string ClickMap(Vector2 location)
    {
        RaycastHit2D hit = Physics2D.Raycast(location, Vector2.zero, 0f);
        if (hit.collider != null)
        {
            GameObject clicked = hit.transform.gameObject;
            if (clicked.CompareTag("WorldMapButton"))
            {
                string cityName = clicked.transform.GetChild(0).GetComponent<TextMeshPro>().text;
                return $"City {cityName}";
            }
            else if (clicked.CompareTag("WorldMapCountryButton"))
            {
                string countryName = clicked.transform.GetChild(0).GetComponent<TextMeshPro>().text;
                return $"Country {countryName}";
            }
        }

        return null;
    }
}
