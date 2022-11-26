using System.Collections.Generic;
using IP.UIFunc.Builder;
using UnityEngine;

namespace IP.Control
{
    public class PopupManager : MonoBehaviour
    {
        private static PopupManager _instance;
        public static PopupManager Instance => _instance;

        private Dictionary<string, GameObject> _popups;
        private List<string> _openedPopups;

        // Start is called before the first frame update
        void Start()
        {
            _instance = this;
            _popups = new Dictionary<string, GameObject>();
            _openedPopups = new List<string>();
            gameObject.SetActive(false);
        }

        public void RegisterPopup(string name, GameObject popup)
        {
            popup.SetActive(false);
            _popups[name] = popup;
        }

        public void OpenPopup(string name)
        {
            if (_openedPopups.Contains(name)) return;
            if (_openedPopups.Count == 0) gameObject.SetActive(true);
            _popups[name].SetActive(true);
            _popups[name].GetComponent<IUIBuilder>()?.Build();
            _openedPopups.Add(name);
        }

        public void ClosePopup()
        {
            gameObject.SetActive(false);
            _openedPopups.ForEach(popupName => _popups[popupName].SetActive(false));
            _openedPopups.Clear();
        }

        public bool IsPopupOpen()
        {
            return _openedPopups.Count > 0;
        }

    }
}
