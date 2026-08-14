using UnityEngine;

public class AllyHealth : MonoBehaviour
{
    int maxHealth = 100;
    int currentHealth;
    ObjectPooler allyPool;
    [SerializeField] HealthBar healthBar;

    void Start()
    {
        GameObject poolObject = GameObject.Find("Object Pool Manager");

        if (poolObject != null)
        {
            allyPool = poolObject.GetComponent<ObjectPooler>();
        }
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            allyPool.ReturnToPool(gameObject);
        }
    }
}
