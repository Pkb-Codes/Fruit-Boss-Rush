using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MiniEnemyHealth : MonoBehaviour
{
    public int maxHealth = 40;
    public float flashTime = 0.15f;

    private float flashTimer = 0f;
    private bool isFlashing = false;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Color originalColor;
    private int currentHealth;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        originalColor = sr.color;
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
    }

    public void TakeDamage(int damage)
    {
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

        rb.AddForce(new Vector2(dir * force, 0), ForceMode2D.Impulse);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
