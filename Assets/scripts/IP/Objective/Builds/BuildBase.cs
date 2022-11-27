using UnityEngine;

namespace IP.Objective.Builds
{
    public abstract class BuildBase : MonoBehaviour
    {
        public Texture thumbnail;
        
        private bool _isComplete = false;
        private City _constructCity;

        public abstract string GetName();
        public abstract int GetBuildDate();
        public abstract long GetBudget();

        public City GetCity()
        {
            return _constructCity;
        }

        public void SetCity(City city)
        {
            _constructCity = city;
        }

        public Texture GetTexture()
        {
            return thumbnail;
        }

        public bool IsComplete()
        {
            return _isComplete;
        }

        public void Complete()
        {
            _isComplete = true;
        }

    }
}