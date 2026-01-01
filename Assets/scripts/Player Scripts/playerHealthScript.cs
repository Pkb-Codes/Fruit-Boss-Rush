using UnityEngine;
using UnityEngine.UI;
public class playerHealthScript : MonoBehaviour
{

    public AudioClip hurt;
    public AudioClip normalAttack;
    public AudioClip spinSlashAttack;

    private AudioSource audioSource;

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
    private bool fullHeal = false;
    private Color originalPlayerColor; // to make the enemy flash red (will be removed when sprites are added)
    private Color originalHealthbarColor;
    private SpriteRenderer spriterenderer;
    private Rigidbody2D rb;
    public bool hasreached = false;

    public GameObject healthbar;
    private Image[] healthsprite;
    private Color[] originalHealthbarColors;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        spriterenderer = GetComponent<SpriteRenderer>();
        healthsprite = healthbar.GetComponentsInChildren<Image>();
        rb = GetComponent<Rigidbody2D>();
        originalPlayerColor = spriterenderer.color; // to make the enemy flash red (will be removed when sprites are added)
        originalHealthbarColors = new Color[healthsprite.Length];

        for (int i = 0; i < healthsprite.Length; i++)
            originalHealthbarColors[i] = healthsprite[i].color;

        flashTimer = flashTime; // to make the enemy flash red (will be removed when sprites are added)
        
    }

    void Update()
    {
        float healthPerSegment = maxHealth / hearts.Length; // 30

        if(fullHeal)
        {
            currentHealth += 1;
            if(currentHealth >= maxHealth)
            {
                fullHeal = false;
                currentHealth = maxHealth;
            }
        }

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
            foreach (var img in healthsprite)
                img.color = Color.yellowGreen;

            flashTimer -= Time.deltaTime;
            if(flashTimer <= 0f)
            {
                spriterenderer.color = originalPlayerColor;
                for(int i = 0; i<healthsprite.Length; i++)
                    healthsprite[i].color = originalHealthbarColors[i];
                isFlash = false;
                flashTimer = flashTime;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        audioSource.PlayOneShot(hurt, 0.25f);
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        isFlash = true; // to make the enemy flash red (will be removed when sprites are added)

        if(currentHealth <= 0 && currentHealth > -100)
        {
            Die();
            currentHealth = -300;
        }
    }

    public void Heal()
    {
        currentHealth += 10;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("respawnpoint"))
        {
            fullHeal = true;
            hasreached = true;
        }
    }
}
