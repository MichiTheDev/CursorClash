using UnityEngine;

namespace MichiTheDev
{
    public class KillZone : MonoBehaviour
    {
        [SerializeField] private AudioClipInfo _coinClipInfo;
        
        private AudioSourceObject _audioSourceObject;

        private void Awake()
        {
            _audioSourceObject = new GameObject("KillZone [Audio]").AddComponent<AudioSourceObject>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Coin"))
            {
                UpgradeManager.Instance.AddCoins(1);
                _audioSourceObject.PlayOneShot(_coinClipInfo);
                Destroy(other.gameObject);
            }
        }
    }
}
