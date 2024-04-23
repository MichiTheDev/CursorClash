using System;
using TMPro;
using UnityEngine;

namespace MichiTheDev
{
    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private TMP_Text _coinText;

        private int _coins;
        private bool _upgradesOpen;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            WaveManager.OnWaveStarted += DisableUpgradeScreen;
            WaveManager.OnWaveEnded += EnableUpgradeScreen;
        }

        public void DisableUpgradeScreen()
        {
            if(!_upgradesOpen) return;
            
            _animator.SetTrigger("Close");
            _upgradesOpen = false;
        }

        public void EnableUpgradeScreen()
        {
            if(_upgradesOpen || !_animator) return;
            
            _animator.SetTrigger("Open");
            _upgradesOpen = true;
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            _coinText.text = _coins.ToString("N0");
        }
    }
}