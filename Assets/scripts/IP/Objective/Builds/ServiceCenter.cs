using UnityEngine;

namespace IP.Objective.Builds
{
    public class ServiceCenter : BuildBase
    {
        public override string GetName()
        {
            return "서비스 센터";
        }

        public override float GetMaintenance()
        {
            return 100;
        }

        public override float GetBuildDate()
        {
            return 1;
        }

        public override float GetBudget()
        {
            return 1000;
        }

        public override void CompleteAction()
        {
            Debug.Log($"Complete {GetName()}");
        }

        public override bool IsWire()
        {
            return false;
        }
    }
}