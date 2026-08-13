using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private float chaseRange = 20f;
    [SerializeField] private int damage = 15;

    // Track the target as a class variable so AttackHitEvent can read it
    private GameObject currentTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Changed from FixedUpdate to Update (AI logic & animations perform best in Update)
    void Update()
    {
        EngageTarget();
    }

    void EngageTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);

        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        // 1. Find the single closest ally/player
        foreach (GameObject player in players)
        {
            if (player == null) continue;

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = player;
            }
        }

        // 2. Process actions based on the closest target
        if (closestPlayer != null && closestDistance <= chaseRange)
        {
            currentTarget = closestPlayer;

            // Attack Range
            if (closestDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true; // Stop moving while attacking
                anim.SetBool("IsAttacking", true);
                anim.SetBool("IsMoving", false);
            }
            // Chase Range
            else
            {
                agent.isStopped = false;
                agent.SetDestination(closestPlayer.transform.position);
                anim.SetBool("IsAttacking", false);
                anim.SetBool("IsMoving", true);
            }
        }
        else
        {
            // Reset state if no target is in chase range
            currentTarget = null;
            anim.SetBool("IsAttacking", false);
            anim.SetBool("IsMoving", false);
        }
    }

    // 3. Called strictly by Unity Animation Event at the hit frame
    public void AttackHitEnemy()
    {
        if (currentTarget == null) return;

        // Double-check target is still within attack range before applying damage
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distanceToTarget <= agent.stoppingDistance + 0.5f)
        {
            if (currentTarget.TryGetComponent<Player>(out Player playerScript))
            {
                playerScript.TakeDamage(damage);
            }
        }
    }
}