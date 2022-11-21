using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapInteractor : MonoBehaviour
{
    public GameObject cities;
    public GameObject countries;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            
        }
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
}
