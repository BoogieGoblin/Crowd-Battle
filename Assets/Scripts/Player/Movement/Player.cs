using UnityEngine;

public class Player : MonoBehaviour
{
    int maxHealth = 100;
    int currentHealth;
    [SerializeField] HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
