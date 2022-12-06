using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace IP.Control
{
    public class SceneManager : MonoBehaviour
    {

        [Header("회사 이름 입력필드")]
        public TMP_InputField inputName;

        public void StartGame()
        {
            string name = inputName.text.Trim();
            string pattern = @"[^a-zA-Z0-9가-힣 ]";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(name)) return;
            GameManager.Instance.InitGame(name);
            UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
        }
    }
}
