using System;
using System.Collections.Generic;
using IP.Control;
using UnityEngine;

namespace IP.Screen
{
    public class ScreenManager : MonoBehaviour
    {

        public GameObject mainCamera;
        public int cameraMoveSpeed = 10;

        private Dictionary<string, Transform> _screens;
        private Dictionary<string, GameObject> _screenSubUIs;
        private GameObject _infraUI;

        private bool _isLerping;
        private Vector3 _moveToVector;
        private string _movedScreenType;

        // Start is called before the first frame update
        void Start()
        {
            _screens = new Dictionary<string, Transform>();
            _screenSubUIs = new Dictionary<string, GameObject>();
        
            foreach (Transform tf in transform)
            {
                if (tf.gameObject.CompareTag("Screen"))
                {
                    string screenName = tf.name.ToUpper();
                    _screens[screenName] = tf;

                    foreach (Transform ctf in tf.transform)
                    {
                        if (ctf.gameObject.CompareTag("SubUI")) _screenSubUIs[screenName] = ctf.gameObject;
                    }
                }
            }
        
            CloseAllSubUI();
        
            _infraUI = _screenSubUIs["INFRA"];
            _screenSubUIs.Remove("INFRA");
            _screens.Remove("INFRA");
        
            _screenSubUIs["MAP"].SetActive(true);

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

        public void MoveCamara(String name)
        {
            PopupManager.Instance.ClosePopup();
            CloseAllSubUI();
            if (!name.Equals("MAP"))
            {
                _infraUI.SetActive(false);
            }

            Vector3 moveToVector = _screens.ContainsKey(name) 
                ? _screens[name].transform.position 
                : _screens["MAP"].transform.position;

            moveToVector.z = -10;
            _moveToVector = moveToVector;
            _movedScreenType = name;
            _isLerping = true;
        }

        public void ToggleInfraUI()
        {
            MoveCamara("MAP");
            _infraUI.SetActive(!_infraUI.activeSelf);
            _infraUI.GetComponent<ISubUI>()?.UpdateUI();
        }

        private void OpenSubUI()
        {
            if (_screenSubUIs.ContainsKey(_movedScreenType))
            {
                _screenSubUIs[_movedScreenType].SetActive(true);
                _screenSubUIs[_movedScreenType].GetComponent<ISubUI>()?.UpdateUI();
            }
        }

        private void CloseAllSubUI()
        {
            foreach(GameObject subUI in _screenSubUIs.Values) subUI.SetActive(false);
        }

    }
}