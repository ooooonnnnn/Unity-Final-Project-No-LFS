using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveData wave;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform playerTarget;

    private int enemiesSpawned;
    private int enemiesAlive;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
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

        EnemyBase enemy = Instantiate(
            wave.enemyPrefab,
            spawnPos,
            Quaternion.identity
        );

        enemy.Initialize(playerTarget);

        enemiesSpawned++;
        enemiesAlive++;
    }
}