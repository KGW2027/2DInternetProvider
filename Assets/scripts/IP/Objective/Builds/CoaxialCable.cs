using UnityEngine;

namespace IP.Objective.Builds
{
    public class CoaxialCable : MonoBehaviour, IBuild
    {
        public Texture thumbnail;

        private City _city;
        private bool _complete = false;
        public string GetName()
        {
            return "동축 케이블";
        }

        public int GetBuildDate()
        {
            return 1;
        }

        public long GetBudget()
        {
            return 100;
        }

        public City GetCity()
        {
            return _city;
        }

        public void SetCity(City city)
        {
            _city = city;
        }

        public Texture GetTexture()
        {
            return thumbnail;
        }

        public bool IsComplete()
        {
            return _complete;
        }

        public void Complete()
        {
            _complete = true;
        }
    }
}