using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;

public class BossAttackScript : MonoBehaviour
{

    public int touchDamage = 20; //amount of damage when player touched the boss
    public float jumpForce = 1f;
    public float horizontalForce = 5f;
    public float bounceDuration = 5f;
    public float meleeDashSpeed = 20f;

    private float bounceTimer = 0f;
    private float attackTimer = 0f;
    private int attackCode;
    private int phase = 1;
    private float dir;
    private bool isBouncing = false;
    
    private bool isIdle = true;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private SpriteRenderer sr;
    private EnemyHealthScript health;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealthScript>();
    }

    void Update()
    {
        if(isIdle) {attackTimer += Time.deltaTime;}

        //phase 1
        if(health.currentHealth > 200)
        {
            
        }

        //phase 2
        if(health.currentHealth > 100 && health.currentHealth < 200)
        {
            phase = 2;
        }

        //phase 3
        if(health.currentHealth < 100)
        {
            phase = 3;
        }    



        if(attackTimer > 5)
        {
            switch(phase)
            {
                case 1: 
                    attackCode = Random.Range(1, 3); //gives a random number 1,2
                    break;
                case 2:
                    attackCode = Random.Range(1, 4); //gives a random number 1,2,3
                    break;
                case 3:
                    attackCode = Random.Range(1, 5); //gives a random number 1,2,3,4
                    break;
            }
            isIdle = false;
            attackTimer = 0;
        }

        switch(attackCode)
        {
            case 1:
                MeleeAttackState();
                break;
            case 2:
                RollAttack();
                break;
            case 3:
                CannonballAttackState();
                break;
            case 4:
                MissileAttack();
                break;
        }
        attackCode = 0;


        if(isBouncing)
        {
            bounceTimer += Time.deltaTime;
        }
        //cannonball attack end
        if(isBouncing && bounceTimer > bounceDuration)
        {
            isBouncing = false;
            animator.SetBool("IsBracing", false);
            bounceTimer = 0f;
            isIdle = true;
        }
        
    }

    void CannonballAttack()
    {
        float direction = Random.Range(-2f, 2f);
        dir = direction;

        rb.AddForce(new Vector2(direction * 100 * horizontalForce, 100 * jumpForce), ForceMode2D.Impulse); //multiplying by 100 because the mass of boss is too high
        isBouncing = true;
        bounceTimer = 0f;

    }
    void CannonballAttackState()
    {
        animator.SetBool("IsBracing", true);
        Invoke(nameof(CannonballAttack), 0.75f);
    }

    void MeleeAttack()
    {
        rb.linearVelocityX = dir * meleeDashSpeed;
    }

    void MeleeAttackState()
    {
        if(player.transform.position.x - transform.position.x > 0) {dir = 1;}
        else {dir = -1;}

        if(dir == 1) {sr.flipX = true;}
        else {sr.flipX = false;}

        animator.SetTrigger("Melee");
    }

    void RollAttack()
    {
        isIdle = true;
    }

    void MissileAttack()
    {
        isIdle = true;
    }

    void AttackEnd()
    {
        isIdle = true;
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
