using UnityEngine;
using UnityEngine.UI;

namespace MichiTheDev
{
    public class GameHUD : MonoBehaviour
    {
        public static GameHUD Instance { private set; get; }

        public bool InSettings => _inSettings;
        public bool InAnimation => _inAnimation;
        
        [SerializeField] private GameObject _settingsScreen;
        [SerializeField] private Animator _anim;
        
        private bool _inSettings;
        private bool _inAnimation;
        
        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void OpenSettings()
        {
            if(_inAnimation) return;
            
            _anim.SetTrigger("OpenSettings");
            _inSettings = true;
            EnableSettings();
        }

        public void CloseSettings()
        {
            if(_inAnimation) return;
            
            _anim.SetTrigger("CloseSettings");
            _inSettings = false;
            DisableSettings();
        }

        public void EnableSettings()
        {
            Button[] buttons = _settingsScreen.GetComponentsInChildren<Button>();
            foreach(Button button in buttons)
            {
                button.interactable = true;
            }
        }

        public void DisableSettings()
        {
            Button[] buttons = _settingsScreen.GetComponentsInChildren<Button>();
            foreach(Button button in buttons)
            {
                button.interactable = false;
            }
        }

        public void SetInAnimationTrue()
        {
            _inAnimation = true;
        }

        public void SetInAnimationFalse()
        {
            _inAnimation = false;
        }
    }
}
