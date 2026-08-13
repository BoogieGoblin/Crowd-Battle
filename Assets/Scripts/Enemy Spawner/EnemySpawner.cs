using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pool & Spawn Settings")]
    [SerializeField] private ObjectPooler enemyPool;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxActiveEnemies = 5;
    [SerializeField] private float spawnDelay = 1.0f;

    // Track active enemies to know when one despawns
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        // Reusable yield to avoid garbage collection allocation
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);

        while (true)
        {
            // 1. Clean up inactive/despawned enemy references
            PruneInactiveEnemies();

            // 2. If under limit, spawn ONE enemy and wait
            if (activeEnemies.Count < maxActiveEnemies)
            {
                SpawnEnemy();
                yield return wait;
            }
            else
            {
                // Pool is full; check back on the next frame without garbage allocation
                yield return null;
            }
        }
    }

    private void PruneInactiveEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null || !activeEnemies[i].activeSelf)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPool == null)
        {
            Debug.LogWarning("EnemySpawner: ObjectPooler reference is missing!", this);
            return;
        }

        // Fetch from pool
        GameObject enemy = enemyPool.GetFromPool(spawnPoint.position, Quaternion.identity);

        // Link originPool so PooledObject knows how to return itself on Despawn()
        if (enemy.TryGetComponent<PooledObject>(out var pooledObj))
        {
            pooledObj.Initialize(enemyPool);
        }

        activeEnemies.Add(enemy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
    }
}