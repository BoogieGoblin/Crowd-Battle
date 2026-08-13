using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpawnRadiusVisual : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [Header("Visual Settings")]
    public int segments = 50;
    [Range(0.01f, 0.5f)] public float lineWidth = 0.05f;

    [Header("Timing Settings")]
    public float activeDuration = 3f;
    public float cooldownTime = 5f;

    [Header("Spawning & Pooling")]
    public ObjectPooler objectPooler; // Reference to your pool script
    public int spawnCount = 5;

    private bool isOnCooldown = false;
    private Coroutine activeRoutine;

    private WaitForSeconds activeWait;
    private WaitForSeconds cooldownWait;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false; 
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Collider col = GetComponent<Collider>();
        if (col != null) Destroy(col);

        if (lineRenderer.sharedMaterial == null || lineRenderer.sharedMaterial.shader.name == "Hidden/InternalErrorShader")
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

        lineRenderer.enabled = false;
        activeWait = new WaitForSeconds(activeDuration);
        cooldownWait = new WaitForSeconds(cooldownTime);
    }

    public void TryActivate(float radius)
    {
        if (isOnCooldown) return;

        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(ActivationRoutine(radius));
    }

    private IEnumerator ActivationRoutine(float radius)
    {
        isOnCooldown = true;

        DrawLocalCircle(radius);
        lineRenderer.enabled = true;

        SpawnFromPoolInRadius(radius, spawnCount);

        yield return activeWait;

        lineRenderer.enabled = false;

        yield return cooldownWait;

        isOnCooldown = false;
        activeRoutine = null;
    }

    private void DrawLocalCircle(float radius)
    {
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = (i == segments) ? 0f : (i * angleStep);
            float rad = Mathf.Deg2Rad * currentAngle;

            float x = Mathf.Sin(rad) * radius;
            float z = Mathf.Cos(rad) * radius;
            float y = 0.05f;

            lineRenderer.SetPosition(i, new Vector3(x, y, z));
        }
    }

    private void SpawnFromPoolInRadius(float radius, int count)
    {
        if (objectPooler == null) return;

        for (int i = 0; i < count; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            Vector3 localOffset = new Vector3(randomPoint.x, 0f, randomPoint.y);
            Vector3 spawnPosition = transform.TransformPoint(localOffset);

            // Fetch from object pool instead of Instantiate
            GameObject spawnedObj = objectPooler.GetFromPool(spawnPosition, Quaternion.identity);

            // Optional: link pool reference to object so it knows where to return
            if (spawnedObj.TryGetComponent<PooledObject>(out var pooledObj))
            {
                pooledObj.Initialize(objectPooler);
            }
        }
    }
}