using TMPro;
using UnityEngine;

namespace IP
{
    public static class StaticFunctions
    {
        public static void SetUIText(this GameObject obj, string text)
        {
            obj.GetComponent<TextMeshProUGUI>().text = text;
        }
        
        public static void SetText(this GameObject obj, string text)
        {
            obj.GetComponent<TextMeshPro>().text = text;
        }

        public static void SetUITextColor(this GameObject obj, Color color)
        {
            obj.GetComponent<TextMeshProUGUI>().color = color;
        }
    }
}