using UnityEngine;

namespace MichiTheDev
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Coin"))
            {
                UpgradeManager.Instance.AddCoins(1);
                Destroy(other.gameObject);
            }
        }
    }
}
