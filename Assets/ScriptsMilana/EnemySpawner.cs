using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


namespace ScriptsMilana
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private WaveData wave;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private Transform playerTarget;

        private int enemiesSpawned;
        private int enemiesAlive;
    
        public static event Action OnWaveCompleted;
    
        public static event Action<int, int, int> OnEnemyCountChanged; // for the ui
    
        public int EnemiesAlive => enemiesAlive;
        public int EnemiesSpawned => enemiesSpawned;
        public int TotalEnemies => wave.totalEnemies;

        private void Start()
        {
            NotifyEnemyCountChanged(); 
            StartCoroutine(SpawnRoutine());
        }
        private void OnEnable()
        {
            EnemyBase.OnEnemyKilled += HandleEnemyKilled;
        }

        private void OnDisable()
        {
            EnemyBase.OnEnemyKilled -= HandleEnemyKilled;
        }

        private IEnumerator SpawnRoutine()
        {
            while (enemiesSpawned < wave.totalEnemies)
            {
                SpawnBatch();
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }

        private void SpawnBatch()
        {
            int spawnCount = Mathf.Min(wave.batchSize, wave.totalEnemies - enemiesSpawned);

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
        private void HandleEnemyKilled(EnemyBase enemy) // tracks how many enemies are alive
        {
            enemiesAlive--;
            NotifyEnemyCountChanged();

            if (enemiesAlive == 0 && enemiesSpawned == wave.totalEnemies) // once all enemies die, wave completed event fires
            {
                Debug.Log("Wave Complete!");
                OnWaveCompleted?.Invoke();
            }
        
        }
        private void NotifyEnemyCountChanged()
        {
            OnEnemyCountChanged?.Invoke(enemiesAlive, enemiesSpawned, wave.totalEnemies);
        }
        private void HandleLevelComplete()
        {
            int levelIndex = LevelManager.Instance.CurrentLevelIndex;

            SaveSystem.UnlockNextLevel(levelIndex);

          //  levelCompleteUI.SetActive(true);
        }
    }
}