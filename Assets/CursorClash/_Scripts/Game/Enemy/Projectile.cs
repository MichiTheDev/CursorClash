using System;
using UnityEngine;

namespace MichiTheDev
{
   [RequireComponent(typeof(Rigidbody2D))]
   public class Projectile : MonoBehaviour
   {
      private Rigidbody2D _rb;

      private void Awake()
      {
         _rb = GetComponent<Rigidbody2D>();
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
               GameManager.OnGameStateChanged -= GameStateChanged;
               Destroy(gameObject);
               break;
            case GameState.Playing:
               break;
            case GameState.GameOver:
               GameManager.OnGameStateChanged -= GameStateChanged;
               Destroy(gameObject);
               break;
         }
      }

      private void Start()
      {
         Destroy(gameObject, 5f);
      }

      public void Shoot(Vector2 direction, float speed)
      {
         _rb.velocity = direction * speed;
      }

      private void OnDestroy()
      {
         GameManager.OnGameStateChanged -= GameStateChanged;
      }

      private void OnTriggerExit2D(Collider2D other)
      {
         if(other.CompareTag("Player"))
         {
            other.GetComponent<PlayerCursor>().TakeDamage();
            GameManager.OnGameStateChanged -= GameStateChanged;
            Destroy(gameObject);
         }
      }
   }
}