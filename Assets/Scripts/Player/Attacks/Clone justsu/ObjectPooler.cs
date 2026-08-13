using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;
    public int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        // Pre-warm the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Retrieves an object from the pool and places it in world space.
    /// </summary>
    public GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            // Expand pool dynamically if empty
            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            pool.Enqueue(newObj);
        }

        GameObject objToSpawn = pool.Dequeue();
        objToSpawn.transform.SetPositionAndRotation(position, rotation);
        objToSpawn.SetActive(true);

        return objToSpawn;
    }

    /// <summary>
    /// Deactivates an object and returns it back to the available pool.
    /// </summary>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        
        // Optional: Reset object's parent or state here if needed
        obj.transform.SetParent(transform);

        pool.Enqueue(obj);
    }
}