using UnityEngine;

namespace MichiTheDev
{
    [CreateAssetMenu(menuName = "CursorClash/WaveData")]
    public class WaveData : ScriptableObject
    {
        public Enemy[] Enemies => _enemies;
        
        [SerializeField] private Enemy[] _enemies;
    }
}
