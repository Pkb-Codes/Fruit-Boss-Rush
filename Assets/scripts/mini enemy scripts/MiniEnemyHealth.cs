using UnityEngine;

public class MiniEnemyHealth : MonoBehaviour
{
    public int maxHealth = 40;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeKnockback()
    {
        Debug.Log("taking knockback");
    }

    void Die()
    {
        
    }
}
