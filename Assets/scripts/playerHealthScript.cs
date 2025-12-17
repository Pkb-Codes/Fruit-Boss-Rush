using UnityEngine;

public class playerHealthScript : MonoBehaviour
{

    public int maxHealth = 100;
    public float flashTime = 0.25f; // to make the enemy flash red (will be removed when sprites are added)

    private float flashTimer; // to make the enemy flash red (will be removed when sprites are added)
    private bool isFlash = false; // to make the enemy flash red (will be removed when sprites are added)
    private int currentHealth;
    private Color originalColor; // to make the enemy flash red (will be removed when sprites are added)
    private SpriteRenderer spriterenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        spriterenderer = GetComponent<SpriteRenderer>();
        originalColor = spriterenderer.color; // to make the enemy flash red (will be removed when sprites are added)
        flashTimer = flashTime; // to make the enemy flash red (will be removed when sprites are added)
    }

    void Update()
    {
        // to make the enemy flash red (will be removed when sprites are added)
        if(isFlash)
        {
            spriterenderer.color = Color.red;
            flashTimer -= Time.deltaTime;
            if(flashTimer <= 0f)
            {
                spriterenderer.color = originalColor;
                isFlash = false;
                flashTimer = flashTime;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        isFlash = true; // to make the enemy flash red (will be removed when sprites are added)

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
    }
}
