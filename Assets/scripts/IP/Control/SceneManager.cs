using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace IP.Control
{
    public class SceneManager : MonoBehaviour
    {

        public GameObject inputName;

        public void StartGame()
        {
            string name = inputName.GetComponent<TMP_InputField>().text.Trim();
            string pattern = @"[^a-zA-Z0-9가-힣]";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(name)) return;
            GameManager.Instance.InitGame(name);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }
    }
}
