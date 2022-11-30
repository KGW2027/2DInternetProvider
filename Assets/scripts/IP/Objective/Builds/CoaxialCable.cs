using UnityEngine;

namespace IP.Objective.Builds
{
    public class CoaxialCable : BuildBase
    {
        public override string GetName()
        {
            return "동축 케이블";
        }

        public override float GetMaintenance()
        {
            return 1;
        }

        public override float GetBuildDate()
        {
            return 1/15f;
        }

        public override float GetBudget()
        {
            return 10;
        }

        protected override void CompleteAction()
        {
            Debug.Log($"{GetName()}이 완성되었어요!");
        }

        public override bool IsWire()
        {
            return true;
        }
    }
}