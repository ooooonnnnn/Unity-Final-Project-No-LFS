using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


namespace ScriptsMilana
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private LevelData[] levels;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform playerTarget;
        [SerializeField] private GameObject levelCompleteUI;

        private int enemiesSpawned;
        private int enemiesAlive;
        private WaveData wave;
        
       
    
        public static event Action OnWaveCompleted;
    
        public static event Action<int, int, int> OnEnemyCountChanged; // for the ui
    
        public int EnemiesAlive => enemiesAlive;
        public int EnemiesSpawned => enemiesSpawned;
        public int TotalEnemies => wave.totalEnemies;

        private void Start()
        {
            int levelIndex = LevelManager.Instance.CurrentLevelIndex;

            if (levelIndex >= levels.Length)
                levelIndex = 0;

            StartCoroutine(SpawnRoutine(levels[levelIndex]));
        }
        private void OnEnable()
        {
            EnemyBase.OnEnemyKilled += HandleEnemyKilled;
            OnWaveCompleted += HandleLevelComplete;
        }

        private void OnDisable()
        {
            EnemyBase.OnEnemyKilled -= HandleEnemyKilled;
            OnWaveCompleted -= HandleLevelComplete;
        }

        private IEnumerator SpawnRoutine(LevelData level)
        {
            foreach (var waveData in level.waves)
            {
                wave = waveData;

                enemiesSpawned = 0;

                NotifyEnemyCountChanged();
                
                while (enemiesSpawned < wave.totalEnemies)
                {
                    SpawnBatch();

                    yield return new WaitForSeconds(wave.spawnInterval);
                }
                
                while (enemiesAlive > 0)
                {
                    yield return null;
                }
                
                if (wave.delayAfterWave > 0)
                {
                    yield return new WaitForSeconds(wave.delayAfterWave);
                }
            }

           
            Debug.Log("Level Complete!");
            OnWaveCompleted?.Invoke();
        }

        private void SpawnBatch()
        {
            int remaining = wave.totalEnemies - enemiesSpawned;
            int spawnCount = Mathf.Min(wave.batchSize, remaining);

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
            Vector3 offset = Random.insideUnitSphere * 3f;
            offset.y = 0;

            Vector3 spawnPos = spawnPoint.position + offset;
        
            EnemyBase prefab =
                wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];

            EnemyBase enemy = Instantiate(
                prefab,
                spawnPos,
                Quaternion.identity
            );

            enemy.Initialize(playerTarget);

            enemiesSpawned++;
            enemiesAlive++;
            NotifyEnemyCountChanged();
        }
        private void HandleEnemyKilled(EnemyBase enemy)
        {
            enemiesAlive--;
            NotifyEnemyCountChanged();
        }
        
        private void NotifyEnemyCountChanged()
        {
            OnEnemyCountChanged?.Invoke(enemiesAlive, enemiesSpawned, wave.totalEnemies);
        }
        private void HandleLevelComplete()
        {
            int levelIndex = LevelManager.Instance.CurrentLevelIndex;

            SaveSystem.UnlockNextLevel(levelIndex);

            levelCompleteUI.SetActive(true);
        }
    }
}