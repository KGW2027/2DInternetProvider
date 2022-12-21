using System;
using UnityEngine;

namespace IP.Control
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }

        public GameObject tutMap;
        public GameObject tutInfra;
        public GameObject tutPlan;
        
        private void Start()
        {
            Instance = this;
            CloseTutorial();
        }

        public void PrintMapTutorial()
        {
            tutMap.SetActive(true);
        }

        public void PrintInfraTutorial()
        {
            tutInfra.SetActive(true);
        }

        public void PrintPlanTutorial()
        {
            tutPlan.SetActive(true);
        }

        public void CloseTutorial()
        {
            tutMap.SetActive(false);
            tutInfra.SetActive(false);
            tutPlan.SetActive(false);
        }
    }
}