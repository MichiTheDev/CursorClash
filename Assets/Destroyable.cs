using UnityEngine;

namespace MichiTheDev
{
    public class Destroyable : MonoBehaviour
    {
        public void DestroySelf()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
