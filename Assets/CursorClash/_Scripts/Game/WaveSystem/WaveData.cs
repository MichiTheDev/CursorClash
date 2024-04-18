using UnityEngine;

namespace MichiTheDev
{
    [CreateAssetMenu(menuName = "CursorClash/WaveData")]
    public class WaveData : ScriptableObject
    {
        public int[] EnemyTiers => _enemyTiers;
        
        [SerializeField] private int[] _enemyTiers;
    }
}
