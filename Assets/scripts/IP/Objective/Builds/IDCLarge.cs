using UnityEngine;

namespace IP.Objective.Builds
{
    public class IDCLarge : BuildBase
    {
        public override string GetName()
        {
            return "대형 IDC";
        }

        public override float GetMaintenance()
        {
            return 20000;
        }

        public override float GetBuildDate()
        {
            return 24;
        }

        public override float GetBudget()
        {
            return 15000;
        }

        protected override void CompleteAction()
        {
            Debug.Log($"Complete {GetName()}");
        }

        public override bool IsWire()
        {
            return false;
        }
    }
}