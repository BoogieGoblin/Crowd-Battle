using UnityEngine;

public class PrimaryAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float attackRadius = 6f;
    [SerializeField] string enemyTag = "Enemy";
    [SerializeField] int damage = 20;

    public void Attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if(distanceToEnemy <= attackRadius)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }
        
}
