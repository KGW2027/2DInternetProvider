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

        public override void CompleteAction()
        {
            Debug.Log($"{GetName()} Complete");
        }

        public override bool IsWire()
        {
            return false;
        }
    }
}