using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IP.UIFunc
{
    public class LottoNumbers : MonoBehaviour
    {
        public GameObject tNum1;
        public GameObject tNum2;
        public GameObject tNum3;
        public GameObject tNum4;
        public GameObject tNum5;
        public GameObject tNum6;
        public GameObject description;

        private TextMeshPro _textNumber1;
        private TextMeshPro _textNumber2;
        private TextMeshPro _textNumber3;
        private TextMeshPro _textNumber4;
        private TextMeshPro _textNumber5;
        private TextMeshPro _textNumber6;
        private TextMeshPro _description;

        private int _round;
        private int[] _nums;
    
        // Start is called before the first frame update
        void Start()
        {
            _textNumber1 = tNum1.GetComponent<TextMeshPro>();
            _textNumber2 = tNum2.GetComponent<TextMeshPro>();
            _textNumber3 = tNum3.GetComponent<TextMeshPro>();
            _textNumber4 = tNum4.GetComponent<TextMeshPro>();
            _textNumber5 = tNum5.GetComponent<TextMeshPro>();
            _textNumber6 = tNum6.GetComponent<TextMeshPro>();
            _description = description.GetComponent<TextMeshPro>();
            _round = 0;
        
            ResetText();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void PickNums()
        {
            int[] nums = new int[6];
            for (int i = 0; i < 6; i++)
            {
                int rand;
                do
                {
                    rand = Random.Range(1, 99);
                } while (nums.Contains(rand));

                nums[i] = rand;
            }
        
            Array.Sort(nums);
            SetNumbers(nums);
        }

        public void MigrateResult(int[] migration)
        {
            SetNumbers(migration);
            _description.text = $"Previous ({++_round}) RESULT";
        }

        public int[] GetNums()
        {
            return _nums;
        }

        public void ResetText()
        {
            _textNumber1.text = "??";
            _textNumber2.text = "??";
            _textNumber3.text = "??";
            _textNumber4.text = "??";
            _textNumber5.text = "??";
            _textNumber6.text = "??";
        }

        private void SetNumbers(int[] nums)
        {
            if (nums == null || nums.Length < 6) return;
            _nums = nums;
            SetNumbers(nums[0], nums[1], nums[2], nums[3], nums[4], nums[5]);
        }
    
        private void SetNumbers(int n1, int n2, int n3, int n4, int n5, int n6)
        {
            _textNumber1.text = $"{n1:00}";
            _textNumber2.text = $"{n2:00}";
            _textNumber3.text = $"{n3:00}";
            _textNumber4.text = $"{n4:00}";
            _textNumber5.text = $"{n5:00}";
            _textNumber6.text = $"{n6:00}";
        }
    
    }
}
