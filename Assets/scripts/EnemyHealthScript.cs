using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyHealthScript : MonoBehaviour
{

    public int maxHealth = 300;
    public int currentHealth;
    public float flashTime = 1f; // to make the enemy flash red (will be removed when sprites are added)
    private float flashTimer; // to make the enemy flash red (will be removed when sprites are added)
    private bool isFlash = false; // to make the enemy flash red (will be removed when sprites are added)


    private SpriteRenderer spriterenderer;
    private Color originalColor; // to make the enemy flash red (will be removed when sprites are added)
    private Collider2D boxCollider;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        spriterenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = spriterenderer.color; // to make the enemy flash red (will be removed when sprites are added)
        flashTimer = flashTime; // to make the enemy flash red (will be removed when sprites are added)
        currentHealth = maxHealth;
    }

    void Update()
    {
        // to make the enemy flash red (will be removed when sprites are added)
        if(isFlash)
        {
            spriterenderer.color = Color.black;
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

    public void Knockback(int direction, int force)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(force * direction, 0f), ForceMode2D.Impulse);
    }

    void Die()
    {
        //make the enemy die
        spriterenderer.enabled = false;
        boxCollider.enabled = false;

        //disabling the hands hitboxes
        // transform.Find("lefthandHitbox").gameObject.SetActive(false);
        // transform.Find("righthandHitbox").gameObject.SetActive(false);

    }
}
