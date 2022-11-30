using UnityEngine;

namespace IP.Objective.Builds
{
    public class UndergroundCoaxialCable : BuildBase
    {
        public override string GetName()
        {
            return "지중 동축 케이블";
        }

        public override float GetMaintenance()
        {
            return 2.5f;
        }

        public override float GetBuildDate()
        {
            return 1/10f;
        }

        public override float GetBudget()
        {
            return 15;
        }

        public override void CompleteAction()
        {
            Debug.Log("Complete " + GetName());
        }

        public override bool IsWire()
        {
            return true;
        }
    }
}