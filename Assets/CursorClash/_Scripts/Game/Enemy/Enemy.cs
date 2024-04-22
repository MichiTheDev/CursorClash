using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MichiTheDev
{
    public class Enemy : MonoBehaviour
    {
        public event Action<Enemy> OnDeath; 
        
        [SerializeField] protected float _health;
        [SerializeField] protected float _hitCooldown;
        [SerializeField] protected SpriteRenderer _srBody;
        [SerializeField] protected Color _defaultColor;
        [SerializeField] protected float _attackCooldown;
        [SerializeField] protected float _attackTriggerChance = 0.5f;
        [SerializeField] protected Collider2D _attackCollider;
        
        [Header("Audio")]
        [SerializeField] protected AudioClipInfo[] _hitAudioClipInfos;
        [SerializeField] protected AudioClipInfo _deathAudioClipInfo;

        protected bool _hitable;
        protected Animator _anim;
        protected AudioSourceObject _sfxAudioSource;
        private float _attackTimer;
        private bool _attackOnCooldown;
        private bool _gameOver;

        private void Awake()
        {
            _anim = GetComponent<Animator>();

            _sfxAudioSource = new GameObject(name + " [Audio]").AddComponent<AudioSourceObject>();
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += GameStateChanged;
        }

        private void GameStateChanged(GameState gameState)
        {
            switch(gameState)
            {
                case GameState.Idle:
                    break;
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    _anim.SetBool("GameOver", true);
                    _gameOver = true;
                    break;
            }
        }

        private void Start()
        {
            _attackCollider.enabled = false;
            StartCoroutine(HitCooldown());

            if (Random.Range(0f, 1f) <= 0.33f)
            {
                _attackOnCooldown = true;
                StartCoroutine(AttackCooldown());
            }
        }

        private void Update()
        {
            if(_gameOver) return;
            
            if (!_attackOnCooldown)
            {
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= 1f)
                {
                    if (Random.Range(0f, 1f) <= _attackTriggerChance)
                    {
                        _attackOnCooldown = true;
                        TriggerCharge();
                    }
                    _attackTimer = 0;
                }
            }
        }

        private void OnDestroy()
        {
            OnDeath?.Invoke(this);
            GameManager.OnGameStateChanged -= GameStateChanged;
            _sfxAudioSource.DestroySelf(2f);
        }

        public void Hit(float damage)
        {
            if(!_hitable || _gameOver) return;
            
            _health -= damage;
            _anim.SetTrigger("Hit");
            StartCoroutine(HitCooldown());
            
            ParticleManager.SpawnParticle("Hit_VFX", new Vector3(transform.position.x, transform.position.y, -1));
            
            if(_health <= 0)
            {
                ParticleManager.SpawnParticle("Death_VFX", transform.position);
                _deathAudioClipInfo.Pitch = Random.Range(_deathAudioClipInfo.Pitch, _deathAudioClipInfo.Pitch + 0.25f);
                _sfxAudioSource.PlayOneShot(_deathAudioClipInfo);
                Destroy(gameObject);
                return;
            }
            
            _sfxAudioSource.PlayOneShot(_hitAudioClipInfos[Random.Range(0, _hitAudioClipInfos.Length)]);
        }

        private void TriggerCharge()
        {
            if(_gameOver) return;
            
            _anim.SetBool("Attacking", true);
            _anim.SetTrigger("Charge");
        }
        
        protected virtual void Attack()
        {
            if(_gameOver) return;
            
            _attackCollider.enabled = true;
            _hitable = false;
            StartCoroutine(AttackCooldown());
        }

        private void StopAttack()
        {
            if(_gameOver) return;
            
            _hitable = true;
            _attackCollider.enabled = false;
            _anim.SetBool("Attacking", false);
        }

        private IEnumerator HitCooldown()
        {
            _hitable = false;
            yield return new WaitForSeconds(_hitCooldown);
            _hitable = true;
        }

        private IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(_attackCooldown);
            _attackOnCooldown = false;
        }
        
        private void OnValidate()
        {
            if(_srBody == null || _defaultColor == null) return;
            _srBody.color = _defaultColor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_gameOver) return;
            
            if (_attackCollider.enabled)
            {
                PlayerCursor playerCursor = other.GetComponent<PlayerCursor>();
                if (playerCursor)
                {
                    playerCursor.TakeDamage();
                    _attackCollider.enabled = false;
                }
            }
        }
    }
}
