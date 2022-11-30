using UnityEngine;

namespace IP.Objective.Builds
{
    public class CacheServer : BuildBase
    {
        public override string GetName()
        {
            return "캐시 서버";
        }

        public override float GetMaintenance()
        {
            return 3000;
        }

        public override float GetBuildDate()
        {
            return 9;
        }

        public override float GetBudget()
        {
            return 5000;
        }

        public override void CompleteAction()
        {
            Debug.Log($"Complete {GetName()}");
        }

        public override bool IsWire()
        {
            return false;
        }
    }
}