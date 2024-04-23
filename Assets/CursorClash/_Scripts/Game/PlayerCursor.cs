using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MichiTheDev
{
   public class PlayerCursor : MonoBehaviour
   {
      [SerializeField] private Sprite _cursorSprite;
      [SerializeField] private float _hitRange = 0.1f;
      [SerializeField] private GameObject _comboPrefab;
      [SerializeField] private int _health = 6;
      [SerializeField] private int _maxHealth = 6;
      [SerializeField] private GameObject _healthDisplay;
      [SerializeField] private GameObject _heart;
      [SerializeField] private Sprite[] _heartSprites;
      
      [Header("Audio")]
      [SerializeField] private AudioClipInfo _spawnClipInfo;
      [SerializeField] private AudioClipInfo _hitClipInfo;
      [SerializeField] private AudioClipInfo _deathClipInfo;
      
      private GameInput _gameInput;
      private SpriteRenderer _sr;
      private Camera _cam;
      private Animator _anim;
      private List<Image> _hearts;
      private bool _hasFocus = true;
      private Vector2 _previousMouseLocation;
      private AudioSourceObject _audioSourceObject;
      private bool _hitable = true;
      private bool _dead;

      private void Awake()
      {
         _cam = Camera.main;
         _anim = GetComponent<Animator>();
         _sr = GetComponentInChildren<SpriteRenderer>();
         
         ShowHardwareCursor(false);

         _audioSourceObject = new GameObject("Player Cursor [Audio]").AddComponent<AudioSourceObject>();
         _audioSourceObject.transform.SetParent(transform);

         _gameInput = new GameInput();
      }

      private void OnEnable()
      {
         GameManager.OnGameStateChanged += GameStateChanged;
         WaveManager.OnEnemyDied += EnemyDied;
         _gameInput.Player.TogglePause.started += TogglePauseInput;
         
         _gameInput.Enable();
      }
      
      private void EnemyDied(Enemy enemy)
      {
         if(this == null) return;
         
         Animator animator = Instantiate(_comboPrefab, transform.position, Quaternion.identity).GetComponentInChildren<Animator>();
         animator.gameObject.GetComponent<TMP_Text>().text = $"Combo: {ScoreManager.Instance.Combo}";
         animator.SetTrigger("Combo");
      }

      private void OnDisable()
      {
         GameManager.OnGameStateChanged -= GameStateChanged;
      }

      private void GameStateChanged(GameState newGameState)
      {
         switch (newGameState)
         {
            case GameState.Idle:
               ShowHardwareCursor(true);
               _sr.enabled = false;
               break;
            case GameState.Playing:
               ShowHardwareCursor(false);
               _sr.enabled = true;
               break;
            case GameState.GameOver:
               ShowHardwareCursor(true);
               _sr.enabled = false;
               break;
         }
      }

      private void TogglePauseInput(InputAction.CallbackContext context)
      {
         if(GameManager.Instance.GameState == GameState.Playing ||
            GameManager.Instance.GameState == GameState.GameOver || GameHUD.Instance.InAnimation) return;

         if(GameHUD.Instance.InSettings) GameHUD.Instance.CloseSettings();
         else GameHUD.Instance.OpenSettings();
      }

      private void Start()
      {
         _previousMouseLocation = _cam.ScreenToWorldPoint(Mouse.current.position.value);
         _audioSourceObject.PlayOneShot(_spawnClipInfo);
         ParticleManager.SpawnParticle("Death_VFX", transform.position);
         UpdateHealthDisplay();
      }

      private void Update()
      {
         if(_dead) return;
         
         if(_hasFocus) transform.position = (Vector2) _cam.ScreenToWorldPoint(Mouse.current.position.value);

         Enemy[] enemies = GetEnemyCollisions(transform.position, _previousMouseLocation);
         foreach(Enemy enemy in enemies)
         {    
            enemy.Hit(1);
         }

         _previousMouseLocation = transform.position;
      }

      public void TakeDamage()
      {
         if(!_hitable) return;

         _health--;
         UpdateHealthDisplay();

         if(_health <= 0)
         {
            _audioSourceObject.PlayOneShot(_deathClipInfo);
            GameManager.Instance.SetGameState(GameState.GameOver);
            ParticleManager.SpawnParticle("Player_Death_Skull_VFX", transform.position);
            ParticleManager.SpawnParticle("Player_Death_Souls_VFX", transform.position);
            ParticleManager.SpawnParticle("Magic_Poof_VFX", transform.position);
            _sr.enabled = false;
            _dead = true;
            return;
         }
         
         _audioSourceObject.PlayOneShot(_hitClipInfo);
         ParticleManager.SpawnParticle("Player_Hit_VFX", transform.position);
         _anim.SetTrigger("Hit");
         GameManager.Instance.FreezeGameForSeconds(.2f);
      }

      public void EnableGhost()
      {
         _hitable = false;
      }

      public void DisableGhost()
      {
         _hitable = true;
      }
      
      private void OnApplicationFocus(bool hasFocus)
      {
         if (GameManager.Instance.GameState != GameState.Playing) return;
         
         _hasFocus = hasFocus;
         ShowHardwareCursor(!hasFocus);
      }

      private void UpdateHealthDisplay()
      {
         _hearts = _healthDisplay.GetComponentsInChildren<Image>().ToList();
         int heartsNeeded = _maxHealth / 2 - _hearts.Count;
         if(heartsNeeded > 0)
         {
            for(int i = 0; i < heartsNeeded; i++)
            {
               Image spriteRenderer = Instantiate(_heart, _healthDisplay.transform).GetComponent<Image>();
               _hearts.Add(spriteRenderer);
            }
         }

         int currentHealth = _health;
         foreach(Image image in _hearts)
         {
            if(currentHealth >= 2)
            {
               image.sprite = _heartSprites[0];
               currentHealth -= 2;
               continue;
            }
            
            if(currentHealth == 1)
            {
               image.sprite = _heartSprites[1];
               currentHealth--;
               continue;
            }

            image.sprite = _heartSprites[2];
         }
      }
      
      public void ShowHardwareCursor(bool value)
      {
         if(value)
         {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
         }
         else
         {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
         }
      }
      
      private Enemy[] GetEnemyCollisions(Vector2 currentMousePosition, Vector2 previousMousePosition)
      {
         float castLength = Vector2.Distance(currentMousePosition, previousMousePosition);
         
         RaycastHit2D[] hits = Physics2D.CapsuleCastAll(
            currentMousePosition,
            new Vector2(_hitRange, castLength),
            CapsuleDirection2D.Vertical,
            0f, 
            (previousMousePosition - currentMousePosition).normalized,
            castLength
         );

         List<Enemy> hitEnemies = new List<Enemy>();
         foreach (RaycastHit2D hit in hits)
         {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
               hitEnemies.Add(enemy);
            }
         }
         return hitEnemies.ToArray();
      }

      private void OnTriggerEnter2D(Collider2D other)
      {
         if (other.CompareTag("Coin"))
         {
            other.GetComponent<Coin>().Collect();
         }
      }
   }
}