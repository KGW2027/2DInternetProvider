using IP.Control;
using UnityEngine;

namespace IP
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private string _cName;
        private long _money;
        
        void Start()
        {
            Instance = this;
            _money = 1000L;
        }

        void Update()
        {
            
        }

        public string GetCompanyName()
        {
            return _cName;
        }

        public void SetCompanyName(string name)
        {
            _cName = name;
        }

        public long GetHaveMoney()
        {
            return _money;
        }
        
    }
}
