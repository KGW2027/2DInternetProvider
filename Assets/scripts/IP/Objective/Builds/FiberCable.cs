using UnityEngine;

namespace IP.Objective.Builds
{
    public class FiberCable : BuildBase
    {
        public override string GetName()
        {
            return "광섬유 케이블";
        }

        public override float GetMaintenance()
        {
            return 5;
        }

        public override float GetBuildDate()
        {
            return 1 / 8f;
        }

        public override float GetBudget()
        {
            return 40;
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