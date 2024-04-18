using UnityEngine;

namespace MichiTheDev
{
    public class MenuNavigation : MonoBehaviour
    {
        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private GameObject _settingsScreen;

        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void PlayButtonClicked()
        {
            _anim.SetTrigger("Play");
        }

        public void SettingsButtonClicked()
        {
            _anim.SetTrigger("OpenSettings");
        }

        public void SettingsBackButtonClicked()
        {
            _anim.SetTrigger("CloseSettings");
        }

        public void ExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
