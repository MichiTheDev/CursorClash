using System.Collections;
using UnityEngine;

namespace MichiTheDev
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _hitCooldown;

        private bool _hitable = true;
        
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void Hit(float damage)
        {
            if(!_hitable) return;
            
            _health -= damage;
            _anim.SetTrigger("Hit");
            StartCoroutine(HitCooldown());
            
            if(_health <= 0)
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator HitCooldown()
        {
            _hitable = false;
            yield return new WaitForSeconds(_hitCooldown);
            _hitable = true;
        }
    }
}
