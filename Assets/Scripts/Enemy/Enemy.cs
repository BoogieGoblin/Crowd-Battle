using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent enemy;
    [SerializeField] Transform target;

    void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
    }
    void LateUpdate()
    {
        enemy.SetDestination(target.position);
    }
}
