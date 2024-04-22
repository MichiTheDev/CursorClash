using UnityEngine;

namespace MichiTheDev
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private bool _upgradesOpen;
        
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
            if(_upgradesOpen) return;
            
            _animator.SetTrigger("Open");
            _upgradesOpen = true;
        }
    }
}