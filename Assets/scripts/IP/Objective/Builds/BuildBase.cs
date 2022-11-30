using System;
using System.Collections.Generic;
using System.Linq;
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
        public abstract float GetMaintenance();
        public abstract float GetBuildDate();
        public abstract float GetBudget();
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
                Sprite[] sprites = Resources.LoadAll<Sprite>("BuildsThumbnails/Builds");
                foreach (var sprite in sprites)
                {
                    Rect rect = sprite.textureRect;
                    Texture2D texture = new Texture2D((int) rect.width, (int) rect.height);
                    texture.SetPixels(sprite.texture.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height));
                    texture.Apply();
                    _textureMap[sprite.name] = texture;
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
                var headOffice = typeof(HeadOffice);
                var infos = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && p != type && p != headOffice)
                    .ToList();

                List<BuildBase> list = new List<BuildBase>();
                infos.ForEach(t => { list.Add((BuildBase) Activator.CreateInstance(t)); });
                _buildBases = list;
            }

            return _buildBases;
        }

    }
}