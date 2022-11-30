using UnityEngine;

namespace IP.Objective.Builds
{
    public class UndergroundFiberCable : BuildBase

    {
        public override string GetName()
        {
            return "지중 광섬유 케이블";
        }

        public override float GetMaintenance()
        {
            return 6.5f;
        }

        public override float GetBuildDate()
        {
            return 1 / 5f;
        }

        public override float GetBudget()
        {
            return 75;
        }

        protected override void CompleteAction()
        {
            Debug.Log($"Complete {GetName()}");
        }

        public override bool IsWire()
        {
            return true;
        }
    }
}