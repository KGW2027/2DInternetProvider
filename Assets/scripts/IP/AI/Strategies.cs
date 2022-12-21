using System.Collections.Generic;
using IP.Objective;
using IP.Objective.Builds;
using IP.Screen;

namespace IP.AI
{
    public abstract class Strategies
    {
        protected Dictionary<Company, int> _stages;
        protected const long WireOfficeLeast = 1000L;
        protected const long SmallIDCLeast = 3000L;

        protected abstract void Stage1(Company comp);
        protected abstract void Stage2(Company comp);
        protected abstract void Stage3(Company comp);
        protected abstract void Stage4(Company comp);
        protected abstract void Stage5(Company comp);
        protected abstract void Stage6(Company comp);
        protected abstract void Stage7(Company comp);
        protected abstract void Stage8(Company comp);
        protected abstract void Stage9(Company comp);
        protected abstract void Stage10(Company comp);
        protected abstract void Stage11(Company comp);
        protected abstract void Stage12(Company comp);
        protected abstract void Stage13(Company comp);
        protected abstract void Stage14(Company comp);
        protected abstract void Stage15(Company comp);
        protected abstract int GetMaxStage();

        public void SetStrategy(Company comp)
        {
            if(_stages == null)
                _stages = new Dictionary<Company, int>();
            _stages[comp] = 1;
        }

        public void Do(Company comp)
        {
            if (_stages[comp] > GetMaxStage()) return;
            switch (_stages[comp])
            {
                case 1:
                    Stage1(comp);
                    break;
                case 2:
                    Stage2(comp);
                    break;
                case 3:
                    Stage3(comp);
                    break;
                case 4:
                    Stage4(comp);
                    break;
                case 5:
                    Stage5(comp);
                    break;
                case 6:
                    Stage6(comp);
                    break;
                case 7:
                    Stage7(comp);
                    break;
                case 8:
                    Stage8(comp);
                    break;
                case 9:
                    Stage9(comp);
                    break;
                case 10:
                    Stage10(comp);
                    break;
                case 11:
                    Stage11(comp);
                    break;
                case 12:
                    Stage12(comp);
                    break;
                case 13:
                    Stage13(comp);
                    break;
                case 14:
                    Stage14(comp);
                    break;
                case 15:
                    Stage15(comp);
                    break;
            }
        }

        public void Next(Company comp)
        {
            _stages[comp]++;
        }


        protected int[] CalcEndDate(int duration)
        {
            int[] nowDate = AppBarManager.Instance.GetDate();
            nowDate[1] += duration;
            while (nowDate[1] > 12)
            {
                nowDate[1] -= 12;
                nowDate[0]++;
            }

            return nowDate;
        }
        
        // 회사 수익금에 따른 티어 구분
        protected BuildBase GetRecommendWire(long rev)
        {
            if (rev >= 500_000) return new UndergroundFiberCable();
            if (rev >= 200_000) return new FiberCable();
            if (rev >= 50_000) return new UndergroundCoaxialCable();
            return new CoaxialCable();
        }
    }
}