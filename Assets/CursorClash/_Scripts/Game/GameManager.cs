using System;
using UnityEngine;

namespace MichiTheDev
{
   public class GameManager : MonoBehaviour
   {
      public static event Action<GameState> OnGameStateChanged;
      
      public static GameManager Instance { private set; get; }

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
         _mainThemeSourceObject.Play(_mainTheme);
         
         _gameThemeSourceObject = new GameObject("Game Theme [Audio]").AddComponent<AudioSourceObject>();
         _gameThemeSourceObject.Play(_gameTheme);
         _gameThemeSourceObject.ChangeVolume(0f);
      }

      private void OnEnable()
      {
         OnGameStateChanged += GameStateChanged;
      }

      public void SetGameState(GameState newGameState)
      {
         _gameState = newGameState;
         OnGameStateChanged?.Invoke(_gameState);
      }

      private void GameStateChanged(GameState newGameState)
      {
         switch(newGameState)
         {
            case GameState.Idle:
               _mainThemeSourceObject.StartFade(_mainTheme.Volume, 1.5f);
               _gameThemeSourceObject.StartFade(0f, 1.5f);
               break;
            case GameState.Playing:
               _mainThemeSourceObject.StartFade(0f, 1.5f);
               _gameThemeSourceObject.StartFade(_gameTheme.Volume, 1.5f);
               break;   
         }
      }
   }
}