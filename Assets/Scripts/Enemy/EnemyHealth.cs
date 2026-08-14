using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;
    ObjectPooler enemyPool;
    [SerializeField] HealthBar healthBar;

    void Start()
    {
        GameObject poolObject = GameObject.Find("Enemy Pool");

        if (poolObject != null)
        {
            enemyPool = poolObject.GetComponent<ObjectPooler>();
        }
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0 && gameObject.tag != "Spawner")
        {
            enemyPool.ReturnToPool(gameObject);
        }
        if(currentHealth <= 0 && gameObject.tag == "Spawner")
        {
            Destroy(gameObject);
        }
    }
}
