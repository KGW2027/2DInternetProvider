using UnityEngine;

namespace IP.Objective.Builds
{
    public class Starlink : BuildBase
    {
        public override string GetName()
        {
            return "스타링크";
        }

        public override float GetMaintenance()
        {
            return 150000;
        }

        public override float GetBuildDate()
        {
            return 24;
        }

        public override float GetBudget()
        {
            return 50000;
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