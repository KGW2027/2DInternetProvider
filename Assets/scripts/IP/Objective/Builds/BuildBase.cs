using System;
using System.Collections.Generic;
using System.Linq;
using IP.UIFunc;
using UnityEngine;

namespace IP.Objective.Builds
{
    public abstract class BuildBase : MonoBehaviour
    {
        private bool _isComplete;
        private City _wireStartCity;
        private City _constructCity;
        private Dictionary<string, Texture> _textureMap;

        private static List<BuildBase> _buildBases;

        private readonly Color _wireConnectedColor = new(250, 248, 143);

        public abstract string GetName();
        public abstract float GetMaintenance();
        public abstract float GetBuildDate();
        public abstract float GetBudget();
        protected abstract void CompleteAction(Company owner);
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

        public void Complete(Company owner)
        {
            _isComplete = true;
            if (IsWire()) CompleteWireAction();
            CompleteAction(owner);
        }

        public void SetWireCities(City from, City to)
        {
            _wireStartCity = from;
            _constructCity = to;
        }

        private void CompleteWireAction()
        {
            foreach (Connection conn in GameManager.Instance.GetConnections(_wireStartCity))
            {
                if (conn.EndCity == _constructCity)
                {
                    conn.Line.startColor = _wireConnectedColor;
                    conn.Line.endColor = _wireConnectedColor;
                }
            }
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