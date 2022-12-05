using UnityEngine;

namespace IP.Objective.Builds
{
    public class HeadOffice : BuildBase
    {
        public override string GetName()
        {
            return "본사";
        }

        public override float GetMaintenance()
        {
            return 0;
        }

        public override float GetBuildDate()
        {
            return 0;
        }

        public override float GetBudget()
        {
            return 0;
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