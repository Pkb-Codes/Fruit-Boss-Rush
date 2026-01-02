using System.Reflection.Emit;
using System.Threading;
using UnityEngine;

public class RainSeedScript : MonoBehaviour
{
    private float delayTime = 0.2f;
    private float timer = 0f;
    private float movespeed;
    private Vector2 dir;
    private Animator animator;
    private Collider2D col;
    private Rigidbody2D rb;
    private bool isExploding = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if(timer < delayTime)
        {
            rb.linearVelocity = dir * movespeed;
        }

        if(isExploding)
        {
            rb.linearVelocity = new Vector2(0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerHealthScript playerHealth = collision.gameObject.GetComponent<playerHealthScript>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(20);
            }
            Explode();
        }

        if(timer >= delayTime)
        {
            isExploding = true;
            Explode();
        }
    }

    public void SetInitialDirection(Vector2 direction, float speed)
    {
        movespeed = speed;
        dir = direction;
    }

    void Explode()
    {
        animator.SetTrigger("Explode");
        col.enabled = false;
    }

    void End()
    {
        Destroy(gameObject);
    }
}
