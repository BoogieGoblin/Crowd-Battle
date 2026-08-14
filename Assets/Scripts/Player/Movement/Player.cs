using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int currentHealth;
    ObjectPooler allyPooler;
    [SerializeField] HealthBar healthBar;

    void Start()
    {
        GameObject poolObject = GameObject.Find("Object Pool Manager");

        if (poolObject != null)
        {
            allyPooler = poolObject.GetComponent<ObjectPooler>();
        }

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0 && maxHealth <= 20)
        {
            allyPooler.ReturnToPool(gameObject);
        }
    }
}
