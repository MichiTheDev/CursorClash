using TMPro;
using UnityEngine;

namespace MichiTheDev
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private float _timeUntilComboRunsOut = 1f;
        [SerializeField] private float _score;
        [SerializeField] private float _scoreMultiplierPerCombo = .1f;
        [SerializeField] private float _scorePerKill = 10f;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Animator _scoreAnimator;

        private float _comboTimer;
        private bool _inCombo;
        private int _combo;

        private void OnEnable()
        {
            WaveManager.OnEnemyDied += OnEnemyDied;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            _inCombo = true;
            _comboTimer = _timeUntilComboRunsOut;
            ++_combo;

            _score += _scorePerKill * (1 + _scoreMultiplierPerCombo * _combo);
            _scoreText.text = $"Score: {Mathf.RoundToInt(_score).ToString("N0")}";
            _scoreAnimator.SetTrigger("AddScore");
        }

        private void Update()
        {
            if(!_inCombo) return;

            _comboTimer -= Time.deltaTime;
            if(_comboTimer <= 0)
            {
                _inCombo = false;
                _combo = 0;
            }
        }
    }
}
