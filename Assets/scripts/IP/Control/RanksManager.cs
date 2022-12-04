using IP.AI;
using IP.Objective;
using IP.UIFunc.Builder;
using UnityEngine;

namespace IP.Control
{
    public class RanksManager : MonoBehaviour
    {
    
        [Header("회사 정보 프리팹")]
        public GameObject panel;
    
        void Start()
        {
            UpdateRanks();
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

            GameObject userPanel = Instantiate(panel, transform, true);
            UpdatePanelDisplay(userPanel, GameManager.Instance.Company);

            foreach (Company aiCompany in AIManager.Instance.Companies)
            {
                GameObject aiPanel = Instantiate(panel, transform, true);
                UpdatePanelDisplay(aiPanel, aiCompany);
            }
        }

        private void UpdatePanelDisplay(GameObject target, Company company)
        {
            RankPanelInfo rpi = target.GetComponent<RankPanelInfo>();
            rpi.SendData(company, 1);
            rpi.Build();
        }
    }
}
