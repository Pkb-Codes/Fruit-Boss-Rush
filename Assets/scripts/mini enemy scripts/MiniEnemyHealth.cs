using UnityEngine;

public class MiniEnemyHealth : MonoBehaviour
{
    public AudioClip hurt;
    public AudioClip die;
    private AudioSource audioSource;
    public int maxHealth = 40;
    public float flashTime = 0.15f;

    private float flashTimer = 0f;
    private bool isFlashing = false;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Color originalColor;
    private int currentHealth;
    private bool isalive = true;
    public bool isKnocked = false;

    private SpriteRenderer mySprite;
    private BoxCollider2D boxCollider;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        originalColor = sr.color;
        mySprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(isFlashing)
        {
            flashTimer += Time.deltaTime;
            sr.color = Color.black;

            if(flashTimer >= flashTime)
            {
                isFlashing = false;
                flashTimer = 0f;
                sr.color = originalColor;
            }
        }

        if(isKnocked && rb.linearVelocityX < 0.1 && rb.linearVelocityX > -0.1)
        {
            isKnocked = false;
        }
    }

    public void TakeDamage(int damage)
    {
        audioSource.PlayOneShot(hurt, 1f);
        currentHealth -= damage;
        isFlashing = true;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeKnockback(float force, float x)
    {
        int dir = 0;
        if(transform.position.x - x > 0) {dir = 1;}
        else {dir = -1;}

        isKnocked = true;

        rb.AddForce(new Vector2(dir * force, 0), ForceMode2D.Impulse);
    }

    void Die()
    {
        if(isalive == true)
        {
            //these two lines can be changed to have a death anim play if any
            mySprite.enabled = false;
            boxCollider.enabled = false;
            isalive = false;
            audioSource.PlayOneShot(die);
            Destroy(gameObject, die.length);
        }
    }
}
