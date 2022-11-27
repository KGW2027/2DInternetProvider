using TMPro;
using UnityEngine;

namespace IP
{
    public static class StaticFunctions
    {
        public static void SetUIText2(this GameObject obj, string text)
        {
            obj.GetComponent<TextMeshProUGUI>().text = text;
        }
        public static void SetUIText(GameObject obj, string text)
        {
            obj.GetComponent<TextMeshProUGUI>().text = text;
        }
        
        public static void SetText(GameObject obj, string text)
        {
            obj.GetComponent<TextMeshPro>().text = text;
        }

        public static void SetUITextColor(GameObject obj, Color color)
        {
            obj.GetComponent<TextMeshProUGUI>().color = color;
        }
    }
}