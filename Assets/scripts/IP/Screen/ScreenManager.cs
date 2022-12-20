using System;
using System.Collections.Generic;
using IP.Control;
using TMPro;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace IP.Screen
{
    public class ScreenManager : MonoBehaviour
    {

        [Header("메인 카메라")]
        public GameObject mainCamera;
        [Header("카메라 속성")]
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

        /**
         * Control Screen들을 표시하기 위한 메인카메라를 조종하는 함수
         */
        public void MoveCamara(String name)
        {
            if (GameManager.Instance.IsGameEnd) return;
            
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

        /**
         * WorldMap에서 작동하는 InfraUI를 Toggle하는 함수
         */
        public void ToggleInfraUI()
        {
            MoveCamara("MAP");
            _infraUI.SetActive(!_infraUI.activeSelf);
            _infraUI.GetComponent<ISubUI>()?.UpdateUI();
        }

        public void Refresh()
        {
            foreach (var subUI in _screenSubUIs.Values)
            {
                subUI.GetComponent<ISubUI>()?.MonthRefresh();
            }
        }

        public void EnableGameOverScreen(string reason)
        {
            _screenSubUIs["GAMEOVER"].SetActive(true);
            foreach (Transform child in _screenSubUIs["GAMEOVER"].transform)
            {
                if (child.name.Equals("Reason"))
                {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = $"사유 : {reason}";
                }
            }
        }

        public void BackToLobby()
        {
            SceneManager.LoadScene("TitleMenu");
        }

    }
}