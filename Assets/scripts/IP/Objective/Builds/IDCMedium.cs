using UnityEngine;

namespace IP.Objective.Builds
{
    public class IDCMedium : BuildBase
    {
        public override string GetName()
        {
            return "중형 IDC";
        }

        public override float GetMaintenance()
        {
            return 6000;
        }

        public override float GetBuildDate()
        {
            return 18;
        }

        public override float GetBudget()
        {
            return 4000;
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