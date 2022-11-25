using UnityEngine;

namespace IP.Objective
{
    public interface IBuild
    {
        public string GetName();
        public int GetBuildDate();
        public long GetBudget();
        public City GetCity();
        public Texture GetTexture();
        public bool IsComplete();
    }
}