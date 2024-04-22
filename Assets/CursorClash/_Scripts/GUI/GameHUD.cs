using System.Collections;
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
        [SerializeField] private Animator _upgradeAnim;
        [SerializeField] private Animator _baseAnim;
        
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

            StartCoroutine(TriggerSettingsAnimation("OpenSettings"));
            _upgradeAnim.SetTrigger("Close");
            _baseAnim.SetTrigger("Close");
            _inSettings = true;
            EnableSettings();
        }

        public void CloseSettings()
        {
            if(_inAnimation) return;
            
            _anim.SetTrigger("CloseSettings");
            _baseAnim.SetTrigger("Open");
            StartCoroutine(TriggerUpgradeAnimation("Open"));
            _inSettings = false;
            DisableSettings();
        }

        private IEnumerator TriggerUpgradeAnimation(string parameter)
        {
            yield return new WaitForSeconds(0.75f);
            _upgradeAnim.SetTrigger(parameter);
        }

        private IEnumerator TriggerSettingsAnimation(string parameter)
        {
            yield return new WaitForSeconds(0.25f);
            _anim.SetTrigger(parameter);
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
