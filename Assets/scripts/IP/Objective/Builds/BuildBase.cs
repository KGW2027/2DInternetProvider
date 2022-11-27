using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;

namespace IP.Objective.Builds
{
    public abstract class BuildBase : MonoBehaviour
    {
        private bool _isComplete = false;
        private City _constructCity;
        private Dictionary<string, Texture> _textureMap;

        private static List<BuildBase> _buildBases;

        public abstract string GetName();
        public abstract int GetBuildDate();
        public abstract long GetBudget();
        public abstract void CompleteAction();
        public abstract bool IsWire();

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
            if (_textureMap == null)
            {
                _textureMap = new Dictionary<string, Texture>();
                Texture2D[] textures = Resources.LoadAll<Texture2D>("BuildsThumbnails");
                foreach (var texture2D in textures)
                {
                    _textureMap[texture2D.name] = texture2D;
                }
            }

            string key = $"Build_{GetType().Name}";
            return _textureMap[key];
        }

        public bool IsComplete()
        {
            return _isComplete;
        }

        public void Complete()
        {
            _isComplete = true;
            CompleteAction();
        }
        
        // Static Methods

        public static List<BuildBase> GetBuildInfos()
        {
            if (_buildBases == null)
            {
                var type = typeof(BuildBase);
                var infos = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && p != type)
                    .ToList();

                List<BuildBase> list = new List<BuildBase>();
                infos.ForEach(t => { list.Add((BuildBase) Activator.CreateInstance(t)); });
                _buildBases = list;
            }

            return _buildBases;
        }

    }
}