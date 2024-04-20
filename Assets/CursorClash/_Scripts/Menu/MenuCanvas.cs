using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MichiTheDev
{
    public class MenuCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private Animator _anim;

        public void PlayButtonClicked()
        {
            _anim.SetTrigger("Play");
            DisableTitleScreen();
        }

        public void SettingsButtonClicked()
        {
            _anim.SetTrigger("OpenSettings");
        }

        public void SettingsBackButtonClicked()
        {
            _anim.SetTrigger("CloseSettings");
        }

        public void EnableTitleScreen()
        {
            Button[] buttons = _titleScreen.GetComponentsInChildren<Button>();
            foreach(Button button in buttons)
            {
                button.interactable = true;
            }
        }

        public void DisableTitleScreen()
        {
            Button[] buttonsTitle = _titleScreen.GetComponentsInChildren<Button>();
            foreach(Button button in buttonsTitle)
            {
                button.interactable = false;
            }
        }

        public void EnableSettingsScreen()
        {
            Button[] buttonsSettings = _settingsScreen.GetComponentsInChildren<Button>();
            foreach(Button button in buttonsSettings)
            {
                button.interactable = true;
            }
        }

        public void DisableSettingsScreen()
        {
            Button[] buttonsSettings = _settingsScreen.GetComponentsInChildren<Button>();
            foreach(Button button in buttonsSettings)
            {
                button.interactable = false;
            }
        }
        
        public void ExitButtonClicked()
        {
            Application.Quit();
        }
        
        public void PlayAnimationEnded()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
