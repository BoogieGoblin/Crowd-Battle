using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpawnRadiusVisual : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    [Header("Visual Settings")]
    [Tooltip("Number of segments to make the circle look smooth.")]
    public int segments = 50;
    [Range(0.01f, 0.5f)] public float lineWidth = 0.05f;

    [Header("Timing Settings")]
    public float activeDuration = 3f;
    public float cooldownTime = 5f;

    private bool isOnCooldown = false;
    private Coroutine activeRoutine;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        // Ensure position count accounts for a closed loop (+1 to connect start and end)
        lineRenderer.positionCount = segments + 1;
        
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Ensure there is no collider attached to this object that could block movement
        Collider col = GetComponent<Collider>();
        if (col != null) Destroy(col);

        if (lineRenderer.sharedMaterial == null || lineRenderer.sharedMaterial.shader.name == "Hidden/InternalErrorShader")
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

        lineRenderer.enabled = false;
    }

    public void TryActivate(Vector3 centerPosition, float radius)
    {
        if (isOnCooldown) return;

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
        }

        activeRoutine = StartCoroutine(ActivationRoutine(centerPosition, radius));
    }

    private IEnumerator ActivationRoutine(Vector3 centerPosition, float radius)
    {
        isOnCooldown = true;
        lineRenderer.enabled = true;

        float timer = 0f;

        while (timer < activeDuration)
        {
            DrawCircle(centerPosition, radius);
            timer += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;

        float cooldownTimer = 0f;
        while (cooldownTimer < cooldownTime)
        {
            cooldownTimer += Time.deltaTime;
            yield return null;
        }

        isOnCooldown = false;
        activeRoutine = null;
    }

    public void DrawCircle(Vector3 centerPosition, float radius)
    {
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            // For the last point, loop back to 0 degrees to seamlessly close the circle
            float currentAngle = (i == segments) ? 0f : (i * angleStep);
            float rad = Mathf.Deg2Rad * currentAngle;
            
            float x = centerPosition.x + (Mathf.Sin(rad) * radius);
            float z = centerPosition.z + (Mathf.Cos(rad) * radius);
            float y = centerPosition.y + 0.05f; 

            lineRenderer.SetPosition(i, new Vector3(x, y, z));
        }
    }
}