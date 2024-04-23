using System;
using System.Collections;
using UnityEngine;

namespace MichiTheDev
{
   public class GameManager : MonoBehaviour
   {
      public static event Action<GameState> OnGameStateChanged;
      
      public static GameManager Instance { private set; get; }

      public bool FadeOut { set; get; }
      public GameState GameState => _gameState;

      [SerializeField] private AudioClipInfo _mainTheme;
      [SerializeField] private AudioClipInfo _gameTheme;
      
      private AudioSourceObject _mainThemeSourceObject;
      private AudioSourceObject _gameThemeSourceObject;
      private GameState _gameState;
      
      private void Awake()
      {
         if(Instance != null)
         {
            Destroy(gameObject);
            return;
         }
         Instance = this;
         DontDestroyOnLoad(gameObject);

         _mainThemeSourceObject = new GameObject("Main Theme [Audio]").AddComponent<AudioSourceObject>();
         _mainThemeSourceObject.transform.SetParent(transform);
         _mainThemeSourceObject.Play(_mainTheme);
         
         _gameThemeSourceObject = new GameObject("Game Theme [Audio]").AddComponent<AudioSourceObject>();
         _gameThemeSourceObject.transform.SetParent(transform);
         _gameThemeSourceObject.Play(_gameTheme);
         _gameThemeSourceObject.ChangeVolume(0f);
      }

      private void OnEnable()
      {
         OnGameStateChanged += GameStateChanged;
      }

      public void FreezeGameForSeconds(float seconds)
      {
         StartCoroutine(Freeze(seconds));
      }
      
      public void SetGameState(GameState newGameState)
      {
         _gameState = newGameState;
         OnGameStateChanged?.Invoke(_gameState);
      }

      private IEnumerator Freeze(float seconds)
      {
         yield return new WaitForSeconds(0.1f);
         Time.timeScale = 0.05f;
         yield return new WaitForSeconds(seconds * 0.05f);
         Time.timeScale = 1f;
      }
      
      private void GameStateChanged(GameState newGameState)
      {
         switch(newGameState)
         {
            case GameState.Idle:
               _mainThemeSourceObject.StartFade(_mainTheme.Volume * 0.75f, 1.25f);
               _gameThemeSourceObject.StartFade(0f, 1.25f);
               Invoke("CollectAllCoins", 0.1f);
               break;
            case GameState.Playing:
               _mainThemeSourceObject.StartFade(0f, 1.25f);
               _gameThemeSourceObject.StartFade(_gameTheme.Volume, 1.25f);
               break;   
            case GameState.GameOver:
               _gameThemeSourceObject.StartFade(0f, 2f);
               _mainThemeSourceObject.StartFade(0f, 2f);
               Invoke("KillAllEnemies", 0.25f);
               KillAllCoins();
               break;
         }
      }

      public void PlayMenuMukke()
      {
         _mainThemeSourceObject.StartFade(_mainTheme.Volume, 1.25f);
      }
      
      private void KillAllEnemies()
      {
         Enemy[] enemies = FindObjectsOfType<Enemy>();
         foreach (Enemy enemy in enemies)
         {
            Destroy(enemy.gameObject);
         }
      }

      private void CollectAllCoins()
      {
         Coin[] coins = FindObjectsOfType<Coin>();
         foreach (Coin coin in coins)
         {
            coin.Collect();
         }
      }

      private void KillAllCoins()
      {
         Coin[] coins = FindObjectsOfType<Coin>();
         foreach (Coin coin in coins)
         {
            Destroy(coin.gameObject);
         }
      }
   }
}