using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private Animator _gameOverAnim;
        [SerializeField] private Animator _transitionAnim;
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private TMP_Text _gameOverScoreText;
        
        private bool _inSettings;
        private bool _inAnimation;
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += GameStateChanged;
        }

        private void Start()
        {
            if (GameManager.Instance.FadeOut)
            {
                _transitionAnim.gameObject.SetActive(true);
                _transitionAnim.SetTrigger("FadeOut");
            }
            else
            {
                _transitionAnim.gameObject.SetActive(false);
            }
        }

        private void GameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Idle:
                    break;
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    GameManager.OnGameStateChanged -= GameStateChanged;
                    OnGameOver();
                    break;
            }
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

        private void OnGameOver()
        {
            _baseAnim.SetTrigger("Hide");
            StartCoroutine(StartLater());

            IEnumerator StartLater()
            {
                yield return new WaitForSeconds(0.75f);
                _gameOverAnim.SetTrigger("Open");
                _gameOverScoreText.text = $"{Mathf.RoundToInt(ScoreManager.Instance.Score).ToString("N0")}";
            }
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

        public void TriggerTryAgain()
        {
            _transitionAnim.gameObject.SetActive(true);
            _transitionAnim.SetTrigger("FadeInTryAgain");
            GameManager.Instance.FadeOut = true;
            _backToMenuButton.interactable = false;
            _tryAgainButton.interactable = false;
        }

        public void TriggerBackToMenu()
        {
            _transitionAnim.gameObject.SetActive(true);
            _transitionAnim.SetTrigger("FadeInBackToMenu");
            GameManager.Instance.FadeOut = true;
            _backToMenuButton.interactable = false;
            _tryAgainButton.interactable = false;
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
