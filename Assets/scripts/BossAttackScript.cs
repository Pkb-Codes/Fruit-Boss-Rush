using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossAttackScript : MonoBehaviour
{

    public int touchDamage = 20; //amount of damage when player touched the boss
    public float jumpForce = 1f;
    public float horizontalForce = 15f;
    public float bounceDuration = 5f;

    private float timer = 0f;
    private float temptimer = 0f;
    private float dir;
    private bool isBouncing = false;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        temptimer += Time.deltaTime;

        if(temptimer >= 10)
        {
            CannonballAttack();
            temptimer = 0;
        }

        if(isBouncing && timer > bounceDuration)
        {
            isBouncing = false;
            timer = 0f;
        }
    }

    void CannonballAttack()
    {
        float direction = Random.Range(-2f, 2f);
        dir = direction;
        rb.AddForce(new Vector2(direction * 100 * horizontalForce, 100 * jumpForce), ForceMode2D.Impulse); //multiplying by 100 because the mass of boss is too high

        isBouncing = true;
        timer = 0f;
    }

    //touch damage
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealthScript from the object that actually collided
            playerHealthScript playerHealth = collision.gameObject.GetComponent<playerHealthScript>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamage); //infllict touchDamage to the player
            }
        }

        if (isBouncing && collision.gameObject.CompareTag("ground"))
        {
            rb.AddForce(new Vector2(0, 100*jumpForce), ForceMode2D.Impulse);
        }
        
        if (isBouncing && collision.gameObject.CompareTag("wall"))
        {
            rb.AddForce(new Vector2(-1 * dir * 100 * horizontalForce, 0), ForceMode2D.Impulse);
            dir *= -1;
        }
    }
}
