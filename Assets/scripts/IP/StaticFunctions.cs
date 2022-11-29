using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IP
{
    public static class StaticFunctions
    {
        private static Dictionary<GameObject, TextMeshProUGUI> _uiTexts = new();
        private static Dictionary<GameObject, TextMeshPro> _texts = new();

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