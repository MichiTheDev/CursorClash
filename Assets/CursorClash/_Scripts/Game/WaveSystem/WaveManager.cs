using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MichiTheDev
{
   public class WaveManager : MonoBehaviour
   {
      public static event Action<Enemy> OnEnemyDied;
      
      public static event Action OnWaveStarted;
      public static event Action OnWaveEnded;
      
      private const float HorizontalBorderOffset = 1.5f;
      private const float VerticalBorderOffset = 1.5f;
      
      public int CurrentWave => _currentWave;

      [SerializeField] private float _enemySpawnDelay = 0.1f;
      [SerializeField] private AudioClipInfo _enemySpawnClipInfo;
      [SerializeField] private TMP_Text _waveInfoText;
      
      private int _currentWave = -1;
      private int _enemiesAlive;
      private Camera _cam;
      private float _camWidth, _camHeight;
      private WaveData[] _waveDatas;
      private AudioSourceObject _audioSourceObject;
      private Enemy[] _enemyTiers;
      private Coroutine _spawnRoutine;

      private void Awake()
      {
         _waveDatas = Resources.LoadAll<WaveData>("Waves");
         _enemyTiers = Resources.LoadAll<Enemy>("Enemies");
         
         _cam = Camera.main;
         _camHeight = 2f * _cam.orthographicSize;
         _camWidth = _camHeight * _cam.aspect;

         _audioSourceObject = new GameObject("Wave Manager [Audio]").AddComponent<AudioSourceObject>();
      }

      private void OnEnable()
      {
         GameManager.OnGameStateChanged += GameStateChanged;
      }

      private void GameStateChanged(GameState gameState)
      {
         switch (gameState)
         {
            case GameState.Idle:
               break;
            case GameState.Playing:
               break;
            case GameState.GameOver:
               GameManager.OnGameStateChanged -= GameStateChanged;
               StopCoroutine(_spawnRoutine);
               break;
         }
      }

      private void Start()
      {
         StartWave();
      }

      public void StartWave()
      {
         _currentWave++;
         _waveInfoText.text = $"Wave: {_currentWave + 1}";
         GameManager.Instance.SetGameState(GameState.Playing);
         OnWaveStarted?.Invoke();
         _spawnRoutine = StartCoroutine(EnemySpawner());
      }

      private void EnemyDied(Enemy enemy)
      {
         enemy.OnDeath -= EnemyDied;
         OnEnemyDied?.Invoke(enemy);
         --_enemiesAlive;
         
         if(_enemiesAlive <= 0)
         {
            OnWaveEnded?.Invoke();
            GameManager.Instance.SetGameState(GameState.Idle);
         }
      }
      
      private IEnumerator EnemySpawner()
      {
         yield return new WaitForSeconds(2f);
         foreach(int enemyTier in _waveDatas[_currentWave].EnemyTiers)
         {
            bool validSpawn = false;
            Vector2 spawnPosition = new Vector2();
            while(!validSpawn)
            {
               float randomX = Random.Range(_camWidth / 2 * -1f + HorizontalBorderOffset, _camWidth / 2 - HorizontalBorderOffset);
               float randomY = Random.Range(_camHeight / 2 * -1f + VerticalBorderOffset, _camHeight / 2 - VerticalBorderOffset);
               RaycastHit2D hit = Physics2D.CircleCast(new Vector2(randomX, randomY), 0.1f, Vector2.zero);
               
               if(hit.collider) continue;
               
               validSpawn = true;
               spawnPosition = new Vector2(randomX, randomY);
            }
            
            Enemy spawnedEnemy = Instantiate(_enemyTiers[enemyTier - 1], spawnPosition, Quaternion.identity);
            spawnedEnemy.OnDeath += EnemyDied;
            ++_enemiesAlive;
            _audioSourceObject.PlayOneShot(_enemySpawnClipInfo);
            yield return new WaitForSeconds(_enemySpawnDelay);
         }
      }
   }
}