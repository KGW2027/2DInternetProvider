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
    }
}