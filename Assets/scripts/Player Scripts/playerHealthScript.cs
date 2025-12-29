using UnityEngine;
using UnityEngine.UI;
public class playerHealthScript : MonoBehaviour
{

    public float maxHealth = 150f;
    public float flashTime = 0.25f; // to make the enemy flash red (will be removed when sprites are added)

    public Sprite emptyHeart;
    public Sprite heart1;
    public Sprite heart2;
    public Sprite fullHeart;
    public Image[] hearts;

    private float flashTimer; // to make the enemy flash red (will be removed when sprites are added)
    private bool isFlash = false; // to make the enemy flash red (will be removed when sprites are added)
    private float currentHealth;
    private Color originalColor; // to make the enemy flash red (will be removed when sprites are added)
    private SpriteRenderer spriterenderer;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        spriterenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = spriterenderer.color; // to make the enemy flash red (will be removed when sprites are added)
        flashTimer = flashTime; // to make the enemy flash red (will be removed when sprites are added)
    }

    void Update() //0 1 2 3 4
    {
        float healthPerSegment = maxHealth / hearts.Length; // 30

        for (int i = 0; i < hearts.Length; i++)
        {
            float segmentHealth = currentHealth - (i * healthPerSegment);

            if (segmentHealth >= healthPerSegment)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (segmentHealth >= healthPerSegment * 2 / 3)
            {
                hearts[i].sprite = heart2;
            }
            else if (segmentHealth >= healthPerSegment / 3)
            {
                hearts[i].sprite = heart1;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }



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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        isFlash = true; // to make the enemy flash red (will be removed when sprites are added)

        if(currentHealth <= 0 && currentHealth > -100)
        {
            Die();
            currentHealth = -300;
        }
    }

    void Die()
    {
        GameObject respawnmanager = GameObject.FindGameObjectWithTag("respawnmanager");
        respawnmanager.GetComponent<RespawnManager>().isDead = true;
        respawnmanager.GetComponent<RespawnManager>().PlayerDied();
    }
    public void Respawn()
    {
        currentHealth = maxHealth;
    }
}
