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
        
        private int[] _endDate;
        private float _budget;
        private bool _overridden;

        private static List<BuildBase> _buildBases;
        private static readonly Color WireConnectedColor = new(250, 248, 143);
        private static Dictionary<string, Texture2D> _iconTextureMap;

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

        public Texture2D GetIconTexture()
        {
            if (_iconTextureMap == null)
            {
                _iconTextureMap = new Dictionary<string, Texture2D>();
                Sprite[] sprites = Resources.LoadAll<Sprite>("BuildsThumbnails/BuildIcon");
                foreach (var sprite in sprites)
                {
                    Rect rect = sprite.textureRect;
                    Texture2D texture = new Texture2D((int) rect.width, (int) rect.height);
                    texture.SetPixels(sprite.texture.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height));
                    texture.Apply();
                    _iconTextureMap[sprite.name] = texture;
                }
            }

            string key = $"BuildIcon_{GetType().Name}";
            return _iconTextureMap[key];
        }

        public bool IsComplete()
        {
            return _isComplete;
        }

        public void ForceComplete(Company owner, City city)
        {
            _isComplete = true;

            if (owner == GameManager.Instance.Company)
            {
                if (this is IDCSmall || this is IDCMedium || this is IDCLarge)
                    city.ActiveIdcIcon(this, GetIconTexture());
                else city.ActiveIcon(this);
                AlertBox.New(AlertBox.AlertType.BuildEnd, city.Name, GetName(), $"{GetMaintenance():F2}");
            }
            CompleteAction(owner);
        }

        public void Complete(Company owner)
        {
            _isComplete = true;
            if (owner == GameManager.Instance.Company)
            {
                AudioManager.Instance.PlayOneShot(AudioManager.Audios.ConstructEnd);
                if (IsWire())
                {
                    CompleteWireAction();
                    AlertBox.New(AlertBox.AlertType.WireEnd, _wireStartCity.Name, _constructCity.Name, $"{GetMaintenance():F2}");
                }
                else
                {
                    if (this is IDCSmall || this is IDCMedium || this is IDCLarge)
                        _constructCity.ActiveIdcIcon(this, GetIconTexture());
                    else _constructCity.ActiveIcon(this);
                    AlertBox.New(AlertBox.AlertType.BuildEnd, _constructCity.Name, GetName(), $"{GetMaintenance():F2}");
                }
            }

            CompleteAction(owner);
        }

        public void SetWireCities(City from, City to)
        {
            _wireStartCity = from;
            _constructCity = to;
        }

        public BuildBase Clone()
        {
            BuildBase clone = (BuildBase) Activator.CreateInstance(GetType());
            clone._constructCity = _constructCity;
            clone._wireStartCity = _wireStartCity;
            clone._isComplete = _isComplete;
            clone._textureMap = _textureMap;
            return clone;
        }

        public void OverrideValues(int[] endDate, float budget)
        {
            _overridden = true;
            _endDate = endDate;
            _budget = budget;
        }

        public int[] GetEndDate()
        {
            return _overridden ? _endDate : null;
        }

        public float GetUseBudget()
        {
            return _overridden ? _budget : -1f;
        }

        private void CompleteWireAction()
        {
            if (_wireStartCity == null || _constructCity == null) return; // AddBuild로 강제로 주는 경우엔 작동 X
            
            foreach (Connection conn in GameManager.Instance.GetConnections(_wireStartCity))
            {
                if (conn.EndCity == _constructCity)
                {
                    conn.Line.startColor = WireConnectedColor;
                    conn.Line.endColor = WireConnectedColor;
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