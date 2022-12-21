using System;
using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.UIFunc;

namespace IP.AI
{
    public class UpgradeStrategy : Strategies
    {
        protected override void Stage1(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 10) Next(comp);
        }

        protected override void Stage2(Company comp)
        {
            City noIDCSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noIDCSmall != null)
            {
                bool succ = ConstructBuild(comp, noIDCSmall, new IDCSmall());
                if (!succ && comp.GetConnectedCities().Count <= 20) Expand(comp);
                return;
            }
            Next(comp);
        }

        protected override void Stage3(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 20) Next(comp);
        }

        protected override void Stage4(Company comp)
        {
            City noIDCSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noIDCSmall != null)
            {
                bool succ = ConstructBuild(comp, noIDCSmall, new IDCSmall());
                if (!succ && comp.GetConnectedCities().Count <= 20) Expand(comp);
                return;
            }
            Next(comp);
        }

        protected override void Stage5(Company comp)
        {
            int cntMedium = GetBuildCount(comp, typeof(IDCMedium));
            if (cntMedium >= 10)
            {
                Next(comp);
                return;
            }

            City noIDCMedium = FindHasBuild(comp, typeof(IDCSmall), typeof(IDCMedium));
            bool succ = ConstructBuild(comp, noIDCMedium, new IDCMedium());
            if(!succ && comp.GetConnectedCities().Count <= 30) Expand(comp);
        }

        protected override void Stage6(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 30) Next(comp);
        }

        protected override void Stage7(Company comp)
        {
            City noIDCSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noIDCSmall != null)
            {
                bool succ = ConstructBuild(comp, noIDCSmall, new IDCSmall());
                if (!succ && comp.GetConnectedCities().Count <= 20) Expand(comp);
                return;
            }
            Next(comp);
        }

        protected override void Stage8(Company comp)
        {
            int cntMedium = GetBuildCount(comp, typeof(IDCMedium));
            if (cntMedium >= 20)
            {
                Next(comp);
                return;
            }

            City noIDCMedium = FindHasBuild(comp, typeof(IDCSmall), typeof(IDCMedium));
            bool succ = ConstructBuild(comp, noIDCMedium, new IDCMedium());
            if(!succ && comp.GetConnectedCities().Count <= 40) Expand(comp);
        }

        protected override void Stage9(Company comp)
        {
            int cntCache = GetBuildCount(comp, typeof(CacheServer));
            if (cntCache >= 10)
            {
                Next(comp);
                return;
            }

            City noCache = FindHasBuild(comp, typeof(IDCMedium), typeof(CacheServer));
            bool succ = ConstructBuild(comp, noCache, new CacheServer());
            if(!succ && comp.GetConnectedCities().Count <= 40) Expand(comp);
        }

        protected override void Stage10(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 40) Next(comp);
        }

        protected override void Stage11(Company comp)
        {
            City noIDCSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noIDCSmall != null)
            {
                bool succ = ConstructBuild(comp, noIDCSmall, new IDCSmall());
                if (!succ && comp.GetConnectedCities().Count <= 20) Expand(comp);
                return;
            }
            Next(comp);
        }

        protected override void Stage12(Company comp)
        {
            int cntMedium = GetBuildCount(comp, typeof(IDCMedium));
            if (cntMedium >= 30)
            {
                Next(comp);
                return;
            }

            City noIDCMedium = FindHasBuild(comp, typeof(IDCSmall), typeof(IDCMedium));
            bool succ = ConstructBuild(comp, noIDCMedium, new IDCMedium());
            if(!succ && comp.GetConnectedCities().Count <= 51) Expand(comp);
        }

        protected override void Stage13(Company comp)
        {
            int cntCache = GetBuildCount(comp, typeof(CacheServer));
            if (cntCache >= 20)
            {
                Next(comp);
                return;
            }

            City noCache = FindHasBuild(comp, typeof(IDCMedium), typeof(CacheServer));
            bool succ = ConstructBuild(comp, noCache, new CacheServer());
            if(!succ && comp.GetConnectedCities().Count <= 51) Expand(comp);
        }

        protected override void Stage14(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 51) Next(comp);
        }

        protected override void Stage15(Company comp)
        {
            City noSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noSmall != null)
            {
                ConstructBuild(comp, noSmall, new IDCSmall());
                return;
            }

            City noMedium = FindNoBuild(comp, typeof(IDCMedium));
            if (noMedium != null)
            {
                ConstructBuild(comp, noMedium, new IDCMedium());
                return;
            }

            City noCache = FindNoBuild(comp, typeof(CacheServer));
            if (noCache != null)
            {
                ConstructBuild(comp, noCache, new CacheServer());
                return;
            }

            City noLarge = FindNoBuild(comp, typeof(IDCLarge));
            if (noLarge != null)
            {
                ConstructBuild(comp, noLarge, new IDCLarge());
                return;
            }
            
            Next(comp);
        }

        protected override int GetMaxStage()
        {
            return 15;
        }
    }
}