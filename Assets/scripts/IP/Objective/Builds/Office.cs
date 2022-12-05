using UnityEngine;

namespace IP.Objective.Builds
{
    public class Office : BuildBase
    {
        public override string GetName()
        {
            return "사무실";
        }

        public override float GetMaintenance()
        {
            return 300;
        }

        public override float GetBuildDate()
        {
            return 1;
        }

        public override float GetBudget()
        {
            return 300;
        }

        protected override void CompleteAction(Company owner)
        {
            
        }

        public override bool IsWire()
        {
            return false;
        }
    }
}