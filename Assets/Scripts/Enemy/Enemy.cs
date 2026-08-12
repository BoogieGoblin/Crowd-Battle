using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] string targetTag = "Player";
    [SerializeField] float chaseRange = 20f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void FixedUpdate()
    {
        EngageTarget();
    }

    void EngageTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);

        foreach(GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if(distanceToPlayer <= chaseRange)
            {
                anim.SetBool("IsAttacking", false);
                anim.SetBool("IsMoving", true);
                agent.SetDestination(player.transform.position);
            }

            if(distanceToPlayer <= agent.stoppingDistance)
            {
                Attack(player);
            }
        }
    }

    void Attack(GameObject target)
    {
        anim.SetBool("IsAttacking", true);
        anim.SetBool("IsMoving", false);
        Debug.Log("Attacking: " + target.name);
    }

        
}
