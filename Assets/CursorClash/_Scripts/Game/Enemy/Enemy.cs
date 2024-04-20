using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MichiTheDev
{
    public class Enemy : MonoBehaviour
    {
        public event Action<Enemy> OnDeath; 
        
        [SerializeField] private float _health;
        [SerializeField] private float _hitCooldown;
        [SerializeField] private SpriteRenderer _srBody;
        [SerializeField] private SpriteRenderer _srFace;
        [SerializeField] private Color _defaultColor;
        
        [Header("Audio")]
        [SerializeField] private AudioClipInfo[] _hitAudioClipInfos;
        [SerializeField] private AudioClipInfo _deathAudioClipInfo;

        private bool _hitable;
        private Animator _anim;
        private AudioSourceObject _sfxAudioSource;

        private void Awake()
        {
            _anim = GetComponent<Animator>();

            _sfxAudioSource = new GameObject(name + " [Audio]").AddComponent<AudioSourceObject>();
        }

        private void Start()
        {
            StartCoroutine(HitCooldown());
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke(this);
            Destroy(_sfxAudioSource.gameObject);
        }

        public void Hit(float damage)
        {
            if(!_hitable) return;
            
            _health -= damage;
            _anim.SetTrigger("Hit");
            StartCoroutine(HitCooldown());
            
            ParticleManager.SpawnParticle("Hit_VFX", new Vector3(transform.position.x, transform.position.y, -1));
            
            if(_health <= 0)
            {
                ParticleManager.SpawnParticle("Death_VFX", transform.position);
                _deathAudioClipInfo.Pitch = Random.Range(_deathAudioClipInfo.Pitch - 0.25f, _deathAudioClipInfo.Pitch + 0.25f);
                _sfxAudioSource.PlayOneShot(_deathAudioClipInfo);
                Destroy(gameObject);
                return;
            }
            
            _sfxAudioSource.PlayOneShot(_hitAudioClipInfos[Random.Range(0, _hitAudioClipInfos.Length)]);
        }

        private IEnumerator HitCooldown()
        {
            _hitable = false;
            yield return new WaitForSeconds(_hitCooldown);
            _hitable = true;
        }

        private void OnValidate()
        {
            if(_srBody == null || _defaultColor == null) return;
            _srBody.color = _defaultColor;
        }
    }
}
