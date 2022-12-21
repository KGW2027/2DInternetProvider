﻿using System;
using System.Collections.Generic;
using System.Linq;
using IP.Objective;
using IP.Objective.Builds;
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
        [Header("요금제 수정 - 도시 선택")]
        public GameObject serviceCities;
        public GameObject togglePrefab;

        private Dictionary<City, ToggleCityInfo> _cityToggles;
        private Dictionary<PaymentPlan, PaymentPlanInfo> _plans;

        private bool _isEditMode;
        private PaymentPlan _editingPlan;

        public void UpdateUI()
        {
            if(_cityToggles == null) InitializeCities();
            
            ResetPlan();
            RefreshToggles();

            if (_plans == null || _plans.Count == 0)
            {
                _plans = new Dictionary<PaymentPlan, PaymentPlanInfo>();
                foreach (PaymentPlan plan in GameManager.Instance.Company.PlanList)
                {
                    var planPanel = Instantiate(planPrefab, planList.transform, true);
                    IUIBuilder builder = planPanel.GetComponent<PaymentPlanInfo>();
                    builder.SendData(plan, this);
                    builder.Build();
                    _plans[plan] = (PaymentPlanInfo) builder;
                }
            }
            else
            {
                MonthRefresh();
            }
        }

        public void MonthRefresh()
        {
            if (_plans == null) return;
            foreach (PaymentPlanInfo ppi in _plans.Values)
            {
                ppi.Build();
            }
        }

        public void Rebuild()
        {
            Dictionary<PaymentPlan, bool> isAdded = new Dictionary<PaymentPlan, bool>();
            foreach (PaymentPlan plan in GameManager.Instance.Company.PlanList) isAdded[plan] = false;
            
            foreach (KeyValuePair<PaymentPlan, PaymentPlanInfo> pair in _plans)
            {
                if (!isAdded.ContainsKey(pair.Key))
                {
                    Destroy(pair.Value.gameObject);
                    continue;
                }
                isAdded[pair.Key] = true;
                pair.Value.Build();
            }

            foreach (PaymentPlan newPlan in isAdded.Keys)
            {
                if (isAdded[newPlan]) continue;
                
                var planPanel = Instantiate(planPrefab, planList.transform, true);
                IUIBuilder builder = planPanel.GetComponent<PaymentPlanInfo>();
                builder.SendData(newPlan, this);
                builder.Build();
                _plans[newPlan] = (PaymentPlanInfo) builder;
            }
        }

        public void DeletePlan(PaymentPlan plan)
        {
            Destroy(_plans[plan].gameObject);
            _plans.Remove(plan);
            Rebuild();
        }

        private void InitializeCities()
        {
            _cityToggles = new Dictionary<City, ToggleCityInfo>();
            GameManager.Instance.GetCountries().ForEach(country =>
            {
                country.GetCities().ForEach(city =>
                {
                    GameObject toggle = Instantiate(togglePrefab, serviceCities.transform, true);
                    _cityToggles[city] = toggle.GetComponent<ToggleCityInfo>();
                    _cityToggles[city].SendData(city);
                    _cityToggles[city].Build();
                });
            });
            
        }

        private void RefreshToggles()
        {
            foreach (City city in GameManager.Instance.Company.GetConnectedCities())
            {
                if (!_cityToggles.ContainsKey(city)) continue; // 연결된 도시가 아닌경우 스킵
                
                // 전선이 연결된 경우만 영업 가능
                bool wireConnected = false;
                foreach (BuildBase builds in GameManager.Instance.Company.GetBuilds(city))
                {
                    if (builds.IsWire())
                    {
                        wireConnected = true;
                        break;
                    }
                }
                if(wireConnected) _cityToggles[city].SetEnabled();
            }
        }

        private List<City> GetSelectCities()
        {
            List<City> cities = new List<City>();

            foreach (KeyValuePair<City, ToggleCityInfo> pair in _cityToggles)
            {
                if(pair.Value.toggle.isOn) cities.Add(pair.Key);
            }
            
            return cities;
        }

        private void ResetChecks()
        {
            foreach (ToggleCityInfo tci in _cityToggles.Values) tci.toggle.isOn = false;
        }

        private void EnableChecks(List<City> cities)
        {
            cities.ForEach(city => _cityToggles[city].toggle.isOn = true);
        }

        public void CreatePlan()
        {
            PaymentPlan plan = new PaymentPlan(GameManager.Instance.Company);
            plan.Name = planName.text;
            plan.Budget = Convert.ToInt64(planBudget.text);
            plan.Bandwidth = (ulong) (Convert.ToInt64(planBandwidth.text) * (long) StaticFunctions.Bytes.GB);
            plan.Upload = (ulong) Convert.ToInt64(planUpload.text);
            plan.Download = (ulong) Convert.ToInt64(planDownload.text);
            
            if (_isEditMode)
            {
                GameManager.Instance.Company.DeletePlan(_editingPlan);
                
                _isEditMode = false;
                _editingPlan = null;
            }
            
            GameManager.Instance.Company.AddPlan(plan, GetSelectCities());

            Rebuild();
        }

        public void Edit(PaymentPlan plan)
        {
            planName.text = plan.Name;
            planBudget.text = plan.Budget.ToString();
            planBandwidth.text = (plan.Bandwidth / StaticFunctions.Bytes.GB).ToString();
            planUpload.text = plan.Upload.ToString();
            planDownload.text = plan.Download.ToString();
            ResetChecks();
            EnableChecks(plan.Cities);
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
            ResetChecks();
            planName.text = "";
            planBudget.text = "";
            planBandwidth.text = "";
            planUpload.text = "";
            planDownload.text = "";
        }
    }
}