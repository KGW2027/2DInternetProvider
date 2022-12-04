using System;
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

        private bool _isEditMode;
        private PaymentPlan _editingPlan;
        
        public void UpdateUI()
        {
            ClearChild();
            ResetPlan();
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
            PaymentPlan plan = new PaymentPlan(GameManager.Instance.Company);
            plan.Name = planName.text;
            plan.Budget = Convert.ToInt64(planBudget.text);
            plan.Bandwidth = Convert.ToInt64(planBandwidth.text) * StaticFunctions.Bytes.GB;
            plan.Upload = Convert.ToInt64(planUpload.text);
            plan.Download = Convert.ToInt64(planDownload.text);
            
            if (_isEditMode)
            {
                _editingPlan.Cities.ForEach(city => plan.Service(city));
                GameManager.Instance.Company.DeletePlan(_editingPlan);
                
                _isEditMode = false;
                _editingPlan = null;
            }
            
            GameManager.Instance.Company.AddPlan(plan);

            UpdateUI();
        }

        public void Edit(PaymentPlan plan)
        {
            planName.text = plan.Name;
            planBudget.text = plan.Budget.ToString();
            planBandwidth.text = (plan.Bandwidth / StaticFunctions.Bytes.GB).ToString();
            planUpload.text = plan.Upload.ToString();
            planDownload.text = plan.Download.ToString();
            _isEditMode = true;
            _editingPlan = plan;
        }

        public void Open(PaymentPlan plan)
        {
            
        }

        public void ResetPlan()
        {
            _isEditMode = false;
            _editingPlan = null;
            planName.text = "";
            planBudget.text = "";
            planBandwidth.text = "";
            planUpload.text = "";
            planDownload.text = "";
        }
    }
}