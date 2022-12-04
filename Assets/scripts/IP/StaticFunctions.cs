using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IP
{
    
    public static class StaticFunctions
    {
        public struct Bytes
        {
            private long InternalValue { get; set; }

            public static readonly long MB = 1L << 0;
            public static readonly long GB = 1L << 10;
            public static readonly long TB = 1L << 20;
            public static readonly long PB = 1L << 30;
            public static readonly long EB = 1L << 40;
            public static readonly long ZB = 1L << 50;
            public static readonly long YB = 1L << 60;

            public static long operator *(Bytes b, long n)
            {
                return b.InternalValue * n;
            }

            public static implicit operator Bytes(long value)
            {
                return new Bytes {InternalValue = value};
            }

            public static string ToByteString(long data)
            {
                if (data / YB > 0) return $"{(double) data / YB:F} YB";
                if (data / ZB > 0) return $"{(double) data / ZB:F} ZB";
                if (data / EB > 0) return $"{(double) data / EB:F} EB";
                if (data / PB > 0) return $"{(double) data / PB:F} PB";
                if (data / TB > 0) return $"{(double) data / TB:F} TB";
                if (data / GB > 0) return $"{(double) data / GB:F} GB";
                return $"{(double) data / MB:F} MB";
            }
        }
        
        private static Dictionary<GameObject, TextMeshProUGUI> _uiTexts = new();
        private static Dictionary<GameObject, TextMeshPro> _texts = new();

        /**
         * TMP Text들을 일괄적으로 수정하기 위한 함수, Dictionary에 caching하여 접근 속도를 높임.
         */
        public static void SetUIText(this GameObject obj, string text)
        {
            if (!_uiTexts.ContainsKey(obj)) _uiTexts[obj] = obj.GetComponent<TextMeshProUGUI>();
            _uiTexts[obj].text = text;
        }
        
        public static void SetText(this GameObject obj, string text)
        {
            if (!_texts.ContainsKey(obj)) _texts[obj] = obj.GetComponent<TextMeshPro>();
            _texts[obj].text = text;
        }

        public static void SetUITextColor(this GameObject obj, Color color)
        {
            if (!_uiTexts.ContainsKey(obj)) _uiTexts[obj] = obj.GetComponent<TextMeshProUGUI>();
            _uiTexts[obj].color = color;
        }
    }
}