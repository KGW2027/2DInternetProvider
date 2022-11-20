using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public enum ScreenType
    {
        MAP,
        INFRA,
        PLAN,
        SHOP,
        BANK,
        RANK,
        OVERALL
    }
    
    public GameObject mainCamera;
    public int cameraMoveSpeed = 10;

    private Dictionary<ScreenType, Transform> _screens;
    private Dictionary<ScreenType, GameObject> _screenSubUIs;

    private bool _isLerping;
    private Vector3 _moveToVector;
    private ScreenType _movedScreenType;

    // Start is called before the first frame update
    void Start()
    {
        _screens = new Dictionary<ScreenType, Transform>();
        _screenSubUIs = new Dictionary<ScreenType, GameObject>();
        
        foreach (Transform tf in transform)
        {
            ScreenType st;
            Enum.TryParse(tf.name, true, out st);
            _screens[st] = tf;

            foreach (Transform ctf in tf.transform)
            {
                if (ctf.gameObject.CompareTag("SubUI")) _screenSubUIs[st] = ctf.gameObject;
            }
        }
        
        CloseAllSubUI();
        _screenSubUIs[ScreenType.MAP].SetActive(true);

        _isLerping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLerping)
        {
            Vector3 cameraPos = mainCamera.transform.position;
            mainCamera.transform.position = Vector3.Lerp(cameraPos, _moveToVector, Time.deltaTime * cameraMoveSpeed);

            if (Vector3.Distance(cameraPos, _moveToVector) < .1f)
            {
                OpenSubUI();
                mainCamera.transform.position = _moveToVector;
                _isLerping = false;
            }
        }
    }

    public void MoveCamara(ScreenType type)
    {
        CloseAllSubUI();
        
        Vector3 moveToVector = _screens.ContainsKey(type) 
            ? _screens[type].transform.position 
            : _screens[ScreenType.MAP].transform.position;

        moveToVector.z = -10;
        _moveToVector = moveToVector;
        _movedScreenType = type;
        _isLerping = true;
    }

    private void OpenSubUI()
    {
        if(_screenSubUIs.ContainsKey(_movedScreenType)) _screenSubUIs[_movedScreenType].SetActive(true);
    }

    private void CloseAllSubUI()
    {
        foreach(GameObject subUI in _screenSubUIs.Values) subUI.SetActive(false);
    }

}