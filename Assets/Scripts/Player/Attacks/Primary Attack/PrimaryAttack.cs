using System.Collections;
using UnityEngine;

public class PrimaryAttack : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string enemyTag = "Enemy";
    [SerializeField] float attackRadius = 6f;
    [SerializeField] int damage = 20;

    [Header("Rotation Settings")]
    [SerializeField] float rotationSpeed = 15f; // Speed of rotation
    [SerializeField] bool lockYAxis = true;      // Keeps character upright

    private Coroutine rotateCoroutine;

    public void Attack()
    {
        anim.SetTrigger("IsAttacking");

        // 1. Find the closest enemy in attack range
        GameObject closestEnemy = GetClosestEnemyInRange();

        // 2. If an enemy is found within attackRadius, rotate and damage
        if (closestEnemy != null)
        {
            // Stop any ongoing rotation before starting a new one
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }

            // Smoothly rotate towards the enemy in range
            rotateCoroutine = StartCoroutine(SmoothLookAt(closestEnemy.transform));

            // Deal damage
            if (closestEnemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }

    // Helper method to filter enemies strictly by attackRadius and distance
    private GameObject GetClosestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // Strict distance check against attackRadius
            if (distanceToEnemy <= attackRadius && distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closest = enemy;
            }
        }

        return closest; // Returns null if no enemy is within attackRadius
    }

    private IEnumerator SmoothLookAt(Transform target)
    {
        if (target == null) yield break;

        Vector3 direction = target.position - transform.position;

        if (lockYAxis)
        {
            direction.y = 0f;
        }

        if (direction == Vector3.zero) yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly turn towards the target over multiple frames
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            if (target == null) yield break;

            direction = target.position - transform.position;
            if (lockYAxis) direction.y = 0f;
            
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    // Visualizes the attack radius in the Scene View to help debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}