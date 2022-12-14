using System;
using System.Collections.Generic;
using System.Linq;
using IP.Objective.Builds;
using IP.UIFunc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IP.Objective
{
    public class City
    {
        enum CitizenCharacter
        {
            TrustFirst,
            SpeedFirst,
            ValueFirst,
            MaxBandwidthFirst
        }
        
        private long GeneratePeople(bool isCity)
        {
            return isCity ? Random.Range(1_000_000, 5_000_000) : Random.Range(10_000, 500_000);
        }
        
        public string Name;
        public long People;
        public List<PaymentPlan> ServicingPlans;
        public GameObject Button;
        public float ActiveRate;
        public bool IsCity;
        public MapCityInfo IconInfo;

        private readonly Dictionary<CitizenCharacter, PaymentPlan> _selectedPlans;
        private readonly Dictionary<CitizenCharacter, long> _peopleCharacters;

        private const double DownloadGuarantee = 0.6d;
        private const double UploadGuarantee = 0.4d;
        private const double SpeedGuarantee = 8.0d;

        public City(string cityName, bool isCity, GameObject button)
        {
            Name = cityName;
            People = GeneratePeople(isCity);
            Button = button;
            ActiveRate = Random.Range(0.001f, 0.020f);
            IsCity = isCity;
            
            IconInfo = button.GetComponent<MapCityInfo>();

            ServicingPlans = new List<PaymentPlan>();
            _selectedPlans = new Dictionary<CitizenCharacter, PaymentPlan>();
            _peopleCharacters = new Dictionary<CitizenCharacter, long>();

            long p1 = GetRandomCount(), p2 = GetRandomCount(), p3 = GetRandomCount();
            _peopleCharacters[CitizenCharacter.TrustFirst] = p1;
            _peopleCharacters[CitizenCharacter.SpeedFirst] = p2;
            _peopleCharacters[CitizenCharacter.MaxBandwidthFirst] = p3;
            _peopleCharacters[CitizenCharacter.ValueFirst] = People - (p1 + p2 + p3);

            foreach (CitizenCharacter character in Enum.GetValues(typeof(CitizenCharacter)))
                _selectedPlans[character] = null;
        }

        private long GetRandomCount()
        {
            float f = Random.Range(20.0f, 30.0f) / 100.0f;
            return (long) (People * f);
        }

        public long GetCustomer(Company company)
        {
            long customer = 0L;
            foreach (KeyValuePair<CitizenCharacter, PaymentPlan> item in _selectedPlans)
            {
                if (item.Value == null) continue;
                if (item.Value.OwnerCompany == company)
                {
                    customer += _peopleCharacters[item.Key];
                }
            }

            return customer;
        }

        public long GetCustomer(PaymentPlan plan)
        {
            long customer = 0L;
            foreach (KeyValuePair<CitizenCharacter, PaymentPlan> item in _selectedPlans)
            {
                if (item.Value == plan)
                {
                    customer += _peopleCharacters[item.Key];
                }
            }

            return customer;
        }

        public void Deservice(PaymentPlan plan)
        {
            if (ServicingPlans.Contains(plan))
            {
                ServicingPlans.Remove(plan);
                foreach (CitizenCharacter character in Enum.GetValues(typeof(CitizenCharacter)))
                {
                    if (_selectedPlans[character] == plan) _selectedPlans[character] = null;
                }
            }
        }

        public void ActiveIcon(BuildBase build)
        {
            if(IconInfo != null) IconInfo.ActiveIcon(build);
        }

        public void ActiveIdcIcon(BuildBase build, Texture2D icon)
        {
            if(IconInfo != null) IconInfo.ActiveIcon(build, icon);
        }

        public void PlanSelect()
        {
            Dictionary<PaymentPlan, double> trustDict = new Dictionary<PaymentPlan, double>();
            Dictionary<PaymentPlan, double> speedDict = new Dictionary<PaymentPlan, double>();
            Dictionary<PaymentPlan, double> bandDict = new Dictionary<PaymentPlan, double>();
            Dictionary<PaymentPlan, double> valueDict = new Dictionary<PaymentPlan, double>();
            long nearMaxPrice = CheckMaxPrice();
            
            foreach (PaymentPlan plan in ServicingPlans)
            {
                if (plan.Budget > nearMaxPrice) continue;
                
                trustDict[plan] = plan.OwnerCompany.Trust;
                speedDict[plan] = plan.Download * DownloadGuarantee + plan.Upload * UploadGuarantee;
                bandDict[plan] = plan.Bandwidth;
                valueDict[plan] = (speedDict[plan] * SpeedGuarantee + bandDict[plan]) / plan.Budget;
            }
            
            ChangePlanTest(CitizenCharacter.TrustFirst, trustDict);
            ChangePlanTest(CitizenCharacter.SpeedFirst, speedDict);
            ChangePlanTest(CitizenCharacter.MaxBandwidthFirst, bandDict);
            ChangePlanTest(CitizenCharacter.ValueFirst, valueDict);
        }

        public void PopulationGrowth()
        {
            People = (long) (People * 1.1d);
            long p1 = GetRandomCount(), p2 = GetRandomCount(), p3 = GetRandomCount();
            _peopleCharacters[CitizenCharacter.TrustFirst] = p1;
            _peopleCharacters[CitizenCharacter.SpeedFirst] = p2;
            _peopleCharacters[CitizenCharacter.MaxBandwidthFirst] = p3;
            _peopleCharacters[CitizenCharacter.ValueFirst] = People - (p1 + p2 + p3);
        }
        

        private void ChangePlanTest(CitizenCharacter character, Dictionary<PaymentPlan, double> dict)
        {
            if (dict.Count == 0) return;


            var dictIterator = dict.GetEnumerator();
            dictIterator.MoveNext();
            if (_selectedPlans[character] == null || (dict.Count == 1 && dictIterator.Current.Key != _selectedPlans[character]))
            {
                _selectedPlans[character] = dictIterator.Current.Key;
            }
            else
            {
                using var sort = dict.OrderByDescending(pair => pair.Value).Take(2).GetEnumerator();
                sort.MoveNext();
                if (sort.Current.Key != _selectedPlans[character])
                {
                    PaymentPlan newPlan = sort.Current.Key;
                    double newValue = sort.Current.Value;
                    sort.MoveNext();
                    double weight = (newValue / sort.Current.Value) - 1;
                    if (Random.Range(0.000f, 1.000f) <= weight)
                    {
                        _selectedPlans[character] = newPlan;
                    }
                }
            }
        }

        private long CheckMaxPrice()
        {
            List<Connection> connections = GameManager.Instance.GetConnections(this);
            long maxPrice = 0L;
            foreach (Connection conn in connections)
            {
                conn.EndCity.ServicingPlans.ForEach(plan =>
                {
                    if (plan.Budget > maxPrice) maxPrice = plan.Budget;
                });
            }

            return Math.Min(133L, maxPrice*2);
        }
        
    }
}