using UnityEngine;

public class AllySpawner : MonoBehaviour
{
    [Header("Spawner References")]
    [SerializeField] private SpawnRadiusVisual spawnRadiusVisual;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private int allyCount = 5;

    public void TriggerAllySpawn()
    {
        if (spawnRadiusVisual == null)
        {
            Debug.LogWarning("[AllySpawner] SpawnRadiusVisual reference is missing!", this);
            return;
        }

        // Forward configured spawn count to the visual handler
        spawnRadiusVisual.spawnCount = allyCount;

        // Activate circle visualization and spawn pooled allies
        spawnRadiusVisual.TryActivate(spawnRadius);
    }
}