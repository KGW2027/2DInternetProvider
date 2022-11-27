using IP.Objective;
using IP.UIFunc.Builder;
using TMPro;
using UnityEngine;

namespace IP.Control
{
    public class RanksManager : MonoBehaviour
    {
    
        public GameObject panel;
    
        // Start is called before the first frame update
        void Start()
        {
            UpdateRanks();
        }

        // Update is called once per frame

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
            }
        }

        private void SetText(Transform textObject, string text)
        {
            textObject.GetComponent<TextMeshProUGUI>().text = text;
        }

        private void UpdatePanelDisplay(GameObject target, Company company)
        {
            RankPanelInfo rpi = target.GetComponent<RankPanelInfo>();
            rpi.SendData(company, 1);
            rpi.Build();
        }
    }
}
