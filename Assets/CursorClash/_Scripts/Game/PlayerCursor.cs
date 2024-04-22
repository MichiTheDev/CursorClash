using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MichiTheDev
{
   public class PlayerCursor : MonoBehaviour
   {
      [SerializeField] private Sprite _cursorSprite;
      [SerializeField] private float _hitRange = 0.1f;
      [SerializeField] private GameObject _comboPrefab;

      [Header("Audio")]
      [SerializeField] private AudioClipInfo _spawnClipInfo;
      
      private GameInput _gameInput;
      private SpriteRenderer _sr;
      private Camera _cam;
      private Animator _anim;
      private bool _hasFocus = true;
      private Vector2 _previousMouseLocation;
      private AudioSourceObject _audioSourceObject;
      private bool _hitable = true;

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
         }
      }

      private void TogglePauseInput(InputAction.CallbackContext context)
      {
         if(GameManager.Instance.GameState == GameState.Playing || GameHUD.Instance.InAnimation) return;

         if(GameHUD.Instance.InSettings) GameHUD.Instance.CloseSettings();
         else GameHUD.Instance.OpenSettings();
      }

      private void Start()
      {
         _previousMouseLocation = _cam.ScreenToWorldPoint(Mouse.current.position.value);
         _audioSourceObject.PlayOneShot(_spawnClipInfo);
         ParticleManager.SpawnParticle("Death_VFX", transform.position);
      }

      private void Update()
      {
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
         
         ParticleManager.SpawnParticle("Player_Hit_VFX", new Vector3(0, -1f));
         _anim.SetTrigger("Hit");
         GameManager.Instance.FreezeGameForSeconds(.1f);
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
         if (GameManager.Instance.GameState == GameState.Idle) return;
         
         _hasFocus = hasFocus;
         ShowHardwareCursor(!hasFocus);
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
   }
}