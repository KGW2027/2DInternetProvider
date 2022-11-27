using UnityEngine;

namespace IP.Objective.Builds
{
    public class CoaxialCable : BuildBase
    {
        public override string GetName()
        {
            return "동축 케이블";
        }

        public override int GetBuildDate()
        {
            return 1;
        }

        public override long GetBudget()
        {
            return 100;
        }

        public override void CompleteAction()
        {
            Debug.Log($"{GetName()}이 완성되었어요!");
        }

        public override bool IsWire()
        {
            return true;
        }
    }
}