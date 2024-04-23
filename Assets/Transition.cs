using UnityEngine;
using UnityEngine.SceneManagement;

namespace MichiTheDev
{
    public class Transition : MonoBehaviour
    {
        public void BackToMenu()
        {
            GameManager.Instance.PlayMenuMukke();
            SceneManager.LoadScene("MenuScene");
        }

        public void TryAgain()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
