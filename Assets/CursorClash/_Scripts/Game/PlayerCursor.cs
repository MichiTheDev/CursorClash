using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MichiTheDev
{
   public class PlayerCursor : MonoBehaviour
   {
      [SerializeField] private Sprite _cursorSprite;
      [SerializeField] private float _hitRange = 0.1f;
      
      private SpriteRenderer _sr;
      private Camera _cam;
      private bool _hasFocus;

      private Vector2 _previousMouseLocation;

      private void Awake()
      {
         _cam = Camera.main;
         
         UpdateCursorSprite(_cursorSprite);
         ShowHardwareCursor(false);
      }

      private void Start()
      {
         _previousMouseLocation = _cam.ScreenToWorldPoint(Mouse.current.position.value);
      }

      private void Update()
      {
         if(Mouse.current.leftButton.isPressed) GameManager.Instance.SetGameState(GameState.Playing);
         
         if(_hasFocus) transform.position = (Vector2) _cam.ScreenToWorldPoint(Mouse.current.position.value);

         Enemy[] enemies = GetEnemyCollisions(transform.position, _previousMouseLocation);
         foreach(Enemy enemy in enemies)
         {    
            enemy.Hit(1);
         }

         _previousMouseLocation = transform.position;
      }

      private void OnValidate()
      {
         if(_cursorSprite == null) return;
         
         UpdateCursorSprite(_cursorSprite);
      }
      
      
      private void OnApplicationFocus(bool hasFocus)
      {
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

      private void UpdateCursorSprite(Sprite sprite)
      {
         _sr = GetComponentInChildren<SpriteRenderer>();
         _sr.sprite = sprite;
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