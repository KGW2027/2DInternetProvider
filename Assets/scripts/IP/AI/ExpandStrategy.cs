using System;
using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.UIFunc;

namespace IP.AI
{
    public class ExpandStrategy : Strategies
    {
        protected override void Stage1(Company comp)
        {
            Expand(comp);   
            if(comp.GetConnectedCities().Count >= 20) Next(comp);
        }

        protected override void Stage2(Company comp)
        {
            City noIDCSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noIDCSmall == null)
            {
                Next(comp);
                return;
            }

            ConstructBuild(comp, noIDCSmall, new IDCSmall());
        }

        protected override void Stage3(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 25) Next(comp);
        }

        protected override void Stage4(Company comp)
        {
            int cntMedium = GetBuildCount(comp, typeof(IDCMedium));
            if (cntMedium >= 10)
            {
                Next(comp);
                return;
            }

            City noIDCMedium = FindNoBuild(comp, typeof(IDCMedium));
            bool buildSuccess = ConstructBuild(comp, noIDCMedium, new IDCMedium());
            if (!buildSuccess && comp.GetConnectedCities().Count <= 30) Expand(comp);
        }

        protected override void Stage5(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 30) Next(comp);
        }

        protected override void Stage6(Company comp)
        {
            int cntMedium = GetBuildCount(comp, typeof(IDCMedium));
            if (cntMedium >= 20)
            {
                Next(comp);
                return;
            }

            City noIDCMedium = FindHasBuild(comp, typeof(IDCSmall), typeof(IDCMedium));
            bool buildSuccess = ConstructBuild(comp, noIDCMedium, new IDCMedium());
            if (!buildSuccess && comp.GetConnectedCities().Count <= 40) Expand(comp);
        }

        protected override void Stage7(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 40) Next(comp);
        }

        protected override void Stage8(Company comp)
        {
            City noIDCSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noIDCSmall != null)
            {
                ConstructBuild(comp, noIDCSmall, new IDCSmall());
                return;
            }

            City cacheTarget = FindHasBuild(comp, typeof(IDCMedium), typeof(CacheServer));
            if (cacheTarget != null)
            {
                ConstructBuild(comp, cacheTarget, new CacheServer());
                return;
            }
            
            Next(comp);
        }

        protected override void Stage9(Company comp)
        {
            City noIDCLarge = FindHasBuild(comp, typeof(IDCMedium), typeof(IDCLarge));
            if (noIDCLarge != null)
            {
                bool buildSuccess = ConstructBuild(comp, noIDCLarge, new IDCLarge());
                if(!buildSuccess) Expand(comp);
                return;
            }

            Next(comp);
        }

        protected override void Stage10(Company comp)
        {
            Expand(comp);
            if(comp.GetConnectedCities().Count >= 51) Next(comp); 
        }

        protected override void Stage11(Company comp)
        {
            City noIDCSmall = FindNoBuild(comp, typeof(IDCSmall));
            if (noIDCSmall != null)
            {
                ConstructBuild(comp, noIDCSmall, new IDCSmall());
                return;
            }
            Next(comp);
        }

        protected override void Stage12(Company comp)
        {
            City noIDCMedium = FindHasBuild(comp, typeof(IDCSmall), typeof(IDCMedium));
            if (noIDCMedium != null)
            {
                ConstructBuild(comp, noIDCMedium, new IDCMedium());
                return;
            }
            Next(comp);
        }

        protected override void Stage13(Company comp)
        {
            City noCache = FindHasBuild(comp, typeof(IDCMedium), typeof(CacheServer));
            if (noCache != null)
            {
                ConstructBuild(comp, noCache, new CacheServer());
                return;
            }
            Next(comp);
        }

        protected override void Stage14(Company comp)
        {
            City noLarge = FindHasBuild(comp, typeof(IDCMedium), typeof(IDCLarge));
            if (noLarge != null)
            {
                ConstructBuild(comp, noLarge, new IDCLarge());
                return;
            }
            Next(comp);
        }

        protected override void Stage15(Company comp)
        {
            
        }

        protected override int GetMaxStage()
        {
            return 15;
        }
    }
}