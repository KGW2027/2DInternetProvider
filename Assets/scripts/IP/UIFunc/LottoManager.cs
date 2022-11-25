using UnityEngine;

namespace IP.UIFunc
{
    public class LottoManager : MonoBehaviour
    {
        public GameObject prev;
        public GameObject now;

        private LottoNumbers _lnPrev;
        private LottoNumbers _lnNow;
        private bool _pickChance;
    
        // Start is called before the first frame update
        void Start()
        {
            _lnPrev = prev.GetComponent<LottoNumbers>();
            _lnNow = now.GetComponent<LottoNumbers>();
            ResetPickChance();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void PickNumbers()
        {
            if (_pickChance)
            {
                _pickChance = false;
                _lnNow.PickNums();
            }
        }

        public void NextMonth()
        {
            _lnPrev.MigrateResult(_lnNow.GetNums());
            _lnNow.ResetText();
            ResetPickChance();
        }

        private void ResetPickChance()
        {
            _pickChance = true;
        }
    }
}
