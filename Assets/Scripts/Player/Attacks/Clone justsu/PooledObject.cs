using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPooler originPool;

    public void Initialize(ObjectPooler pool)
    {
        originPool = pool;
    }

    private void OnEnable()
    {
        // Restore state here (e.g., reset HP, stop particle systems, zero velocity)
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void Despawn()
    {
        if (originPool != null)
        {
            originPool.ReturnToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}