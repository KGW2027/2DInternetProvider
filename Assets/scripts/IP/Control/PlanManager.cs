using IP.Objective;
using IP.UIFunc.Builder;
using TMPro;
using UnityEngine;

namespace IP.Control
{
    public class PlanManager : MonoBehaviour, ISubUI
    {
        [Header("요금제 설명용 프리팹")] public GameObject planPrefab;
        [Header("요금제 리스트")] public GameObject planList;
        [Header("요금제 수정 필드")] 
        public TMP_InputField planName;
        public TMP_InputField planBudget;
        public TMP_InputField planBandwidth;
        public TMP_InputField planUpload;
        public TMP_InputField planDownload;
        
        public void UpdateUI()
        {
            ClearChild();
            foreach (PaymentPlan plan in GameManager.Instance.Company.PlanList)
            {
                var planPanel = Instantiate(planPrefab, planList.transform, true);
                IUIBuilder builder = planPanel.GetComponent<PaymentPlanInfo>();
                builder.SendData(plan, this);
                builder.Build();
            }
        }

        private void ClearChild()
        {
            foreach (Transform child in planList.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void CreatePlan()
        {
            
        }

        public void Edit(PaymentPlan plan)
        {
            
        }

        public void Open(PaymentPlan plan)
        {
            
        }

        public void ResetPlan()
        {
            planName.text = "";
            planBudget.text = "";
            planBandwidth.text = "";
            planUpload.text = "";
            planDownload.text = "";
        }
    }
}