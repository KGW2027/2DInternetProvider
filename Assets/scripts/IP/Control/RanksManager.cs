using System.Collections.Generic;
using System.Linq;
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
                Destroy(child.gameObject);
            }
        }

        private void UpdateRanks()
        {
            ClearChilds();
            float x = 960;
            float y = -60;

            // 등수 매기기
            Dictionary<Company, long> rank = new Dictionary<Company, long>();
            
            // 유저 회사 등록
            Company userCompany = GameManager.Instance.Company;
            rank[userCompany] = userCompany.GetTotalCustomers();

            // AI 회사 등록
            foreach (Company aiCompany in AIManager.Instance.Companies)
            {
                rank[aiCompany] = aiCompany.GetTotalCustomers();
            }

            // 정렬 후 프린트
            using var sortedDict = (from entry in rank orderby entry.Value descending select entry).GetEnumerator();
            int rankNum = 1;
            while (sortedDict.MoveNext())
            {
                Debug.Log($"[{rankNum}] {sortedDict.Current.Key.Name} -> 수익 : {sortedDict.Current.Key.CalcRevenue()} / 소비자 수 : {sortedDict.Current.Key.GetTotalCustomers()}");
                GameObject rankPanel = Instantiate(panel, transform, true);
                UpdatePanelDisplay(rankPanel, sortedDict.Current.Key, rankNum++);
            }
        }

        private void UpdatePanelDisplay(GameObject target, Company company, int rank)
        {
            RankPanelInfo rpi = target.GetComponent<RankPanelInfo>();
            rpi.SendData(company, rank);
            rpi.Build();
        }
    }
}
