using System.Collections;
using UnityEngine;

namespace MichiTheDev
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _health;
        [SerializeField] private float _hitCooldown;

        private bool _hitable;
        private Animator _anim;
        private AudioClipInfo _hitAudioClipInfo;

        private void Awake()
        {
            _anim = GetComponent<Animator>();

            _hitAudioClipInfo = new AudioClipInfo();
            _hitAudioClipInfo.Name = "Hit_SFX";
            _hitAudioClipInfo.Volume = 0.33f;
            _hitAudioClipInfo.Category = "SFX";
        }

        private void Start()
        {
            StartCoroutine(HitCooldown());
        }

        public void Hit(float damage)
        {
            if(!_hitable) return;
            
            _health -= damage;
            _anim.SetTrigger("Hit");

            _hitAudioClipInfo.Pitch = Random.Range(1.5f, 1.6f);
            AudioManager.Play(_hitAudioClipInfo);
            StartCoroutine(HitCooldown());
            
            if(_health <= 0)
            {
                ParticleManager.SpawnParticle("Death_VFX", transform.position);
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
