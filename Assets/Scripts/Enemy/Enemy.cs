using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
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
                agent.SetDestination(player.transform.position);
            }
        }
    }

        
}
