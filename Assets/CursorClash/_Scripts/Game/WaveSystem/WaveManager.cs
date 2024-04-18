using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MichiTheDev
{
   public class WaveManager : MonoBehaviour
   {
      public event Action WaveStarted;
      public event Action WaveEnded;
      
      private const float HorizontalBorderOffset = 2.5f;
      private const float VerticalBorderOffset = 2.5f;
      
      public int CurrentWave => _currentWave;

      [SerializeField] private float _enemySpawnDelay = 0.1f;
      [SerializeField] private AudioClipInfo _enemySpawnClipInfo;
      
      private int _currentWave = 0;
      private int _enemiesAlive;
      private Camera _cam;
      private float _camWidth, _camHeight;
      private WaveData[] _waveDatas;
      private AudioSourceObject _audioSourceObject;
      private Enemy[] _enemyTiers;

      private void Awake()
      {
         _waveDatas = Resources.LoadAll<WaveData>("Waves");
         _enemyTiers = Resources.LoadAll<Enemy>("Enemies");
         
         _cam = Camera.main;
         _camHeight = 2f * _cam.orthographicSize;
         _camWidth = _camHeight * _cam.aspect;

         _audioSourceObject = new GameObject("Wave Manager [Audio]").AddComponent<AudioSourceObject>();
      }

      private void Start()
      {
         StartWave();
      }

      public void StartWave()
      {
         GameManager.Instance.SetGameState(GameState.Playing);
         WaveStarted?.Invoke();
         StartCoroutine(EnemySpawner());
      }

      private void EnemyDied(Enemy enemy)
      {
         enemy.OnDeath -= EnemyDied;
         --_enemiesAlive;
         
         if(_enemiesAlive <= 0)
         {
            WaveEnded?.Invoke();
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
               RaycastHit2D hit = Physics2D.CircleCast(new Vector2(randomX, randomY), 0.75f, Vector2.zero);
               
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