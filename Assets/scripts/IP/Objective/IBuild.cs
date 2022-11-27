using UnityEngine;

namespace IP.Objective
{
    public interface IBuild
    {
        public string GetName();
        public int GetBuildDate();
        public long GetBudget();
        public City GetCity();
        public void SetCity(City city);
        public Texture GetTexture();
        public bool IsComplete();
        public void Complete();
    }
}