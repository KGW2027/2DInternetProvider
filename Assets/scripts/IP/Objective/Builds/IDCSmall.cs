using UnityEngine;

namespace IP.Objective.Builds
{
    public class IDCSmall : BuildBase
    {
        public override string GetName()
        {
            return "소형 IDC";
        }

        public override float GetMaintenance()
        {
            return 500;
        }

        public override float GetBuildDate()
        {
            return 12;
        }

        public override float GetBudget()
        {
            return 500;
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