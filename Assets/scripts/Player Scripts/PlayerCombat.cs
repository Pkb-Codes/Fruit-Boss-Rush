using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework;

public class PlayerCombat : MonoBehaviour
{
    public AudioClip swordslash;

    private AudioSource audioSource;

    public Transform attackPoint; //tracks location of attack point
    public float attackRange = 0.5f; //range of attack
    public int attackDamage = 20;
    public int knockbackForce = 2;
    public Animator SlashAnimator;
    public Image abilityBarFill;

    public LayerMask enemyLayer;
    public float attackCooldown = 3f;


    private float timer = 0f;
    private int facingDirection = 1; 


    [Header("Ability Things")]
    public int spinSlashDamage = 10;
    public float AbilityTime = 7f; //How long the ability lasts
    public int CooldownTime = 10; //How long it takes to cool down
    public BoxCollider2D spinHitbox;

    float spinTickTimer = 0f;  //timer to make spinslash deal damage
    int SpinHitCount = 6;  //number of times spinslash damages on full interval
    float spinInterval;  //timer interval for each hit

    private float abilityTimer = 0f;
    private bool abilityValid = false;
    private bool abTrigger = false;
    private Animator animator;

    HashSet<EnemyHealthScript> enemies = new HashSet<EnemyHealthScript>();
    HashSet<MiniEnemyHealth> miniEnemies = new HashSet<MiniEnemyHealth>();

    // private EnemyHealthScript enemy;
    // private MiniEnemyHealth minienemy;
    private HashSet<EnemyHealthScript> enemiesHit = new HashSet<EnemyHealthScript>();


    void Start()
    {
        abilityTimer = AbilityTime;
        animator = GetComponent<Animator>();

        spinHitbox.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        abilityBarFill.fillAmount = abilityTimer/AbilityTime;
        //SpinSlash code
        {

            if(Input.GetKey(KeyCode.P) && abilityValid == false && abTrigger == false)
            {
                abTrigger = true; //triggers the ability period
                abilityValid = true; //player is able to use ability now

                spinTickTimer = 0f;
                spinInterval = AbilityTime / SpinHitCount;
            }

            if(abTrigger == true && abilityValid == true)
            {
                abilityTimer -= Time.deltaTime; //ability period is decreasing after being activated
            }

            if(abilityTimer < 0)
            {
                abilityTimer = 0;
                abilityValid = false; //switches to cooldown period after using ability, now ability is invalid
                animator.SetBool("IsSpinslash", false);
                enemies.Clear();
                miniEnemies.Clear();
                spinHitbox.enabled = false;
            }

            if(abilityValid == false && abTrigger == true) //cooldown period maths
            {
                float refillerConstant = CooldownTime/AbilityTime;

                abilityTimer += (Time.deltaTime)/refillerConstant;
                
                if(abilityTimer > AbilityTime)
                {
                    abTrigger = false;
                    abilityTimer = AbilityTime;
                }
            }

            if(Input.GetKey(KeyCode.P) && abilityValid)
            {
                spinHitbox.enabled = true;
                animator.SetBool("IsSpinslash", true);
            }

            if(Input.GetKeyUp(KeyCode.P) && abilityValid)
            {
                spinHitbox.enabled = false;
                animator.SetBool("IsSpinslash", false);
                enemies.Clear();
                miniEnemies.Clear();
            }

            //makes spinslash deal damage spinHitCount times in total
            if(spinHitbox.enabled == true)
            {
                spinTickTimer += Time.deltaTime;

                if(spinTickTimer >= spinInterval)
                {
                    SpinSlash();
                    spinTickTimer = 0f;
                }
            }
        }
        //spinslash code ends



        if(timer > attackCooldown && Input.GetKey(KeyCode.I))
        {
            timer = 0f;
            SlashAttackState();
        }

        //to see which direction the player is looking at
        if(Input.GetKey(KeyCode.A))
        {
            facingDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            facingDirection = 1;
        }
    }
   
    void SlashAttackState()
    {
        audioSource.PlayOneShot(swordslash, 0.45f);
        SlashAnimator.SetTrigger("Slash");
        Invoke("SlashAttack", 0.04f);
    }
    void SlashAttack ()
    {
        //keeps trcks of all enemies hit
        enemiesHit.Clear();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            MiniEnemyHealth minienemyHealth = enemy.GetComponent<MiniEnemyHealth>();
            if(minienemyHealth != null) 
            {
                minienemyHealth.TakeDamage(attackDamage);
                minienemyHealth.TakeKnockback(2 * knockbackForce, transform.position.x);
            }

            EnemyHealthScript enemyHealth = enemy.GetComponentInParent<EnemyHealthScript>();
            if(enemyHealth != null)
            {
                if(enemiesHit.Contains(enemyHealth) == true) {return;}
                // makes each enemy take damage
                enemyHealth.TakeDamage(attackDamage);
                enemyHealth.Knockback(facingDirection, knockbackForce);
                enemiesHit.Add(enemyHealth);
            }
        }
    }
    void SpinSlash()
    {
        foreach(EnemyHealthScript e in enemies)
        {
            if(e != null)
            {
                e.TakeDamage(spinSlashDamage);
                e.Knockback(facingDirection, knockbackForce);
            }
        }
        
        foreach(MiniEnemyHealth m in miniEnemies)
        {
            if(m != null)
            {
                m.TakeDamage(spinSlashDamage);
                m.TakeKnockback(knockbackForce/2, transform.position.x);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(spinHitbox.enabled && collision.CompareTag("enemy"))
        {
            EnemyHealthScript e = collision.GetComponentInParent<EnemyHealthScript>();
            if(e != null) {enemies.Add(e);}
            MiniEnemyHealth m = collision.GetComponent<MiniEnemyHealth>();
            if(m != null) {miniEnemies.Add(m);}
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (spinHitbox.enabled && collision.CompareTag("enemy"))
        {
            EnemyHealthScript e = collision.GetComponentInParent<EnemyHealthScript>();
            if (e != null) enemies.Remove(e);

            MiniEnemyHealth m = collision.GetComponent<MiniEnemyHealth>();
            if (m != null) miniEnemies.Remove(m);
        }
    }


    private void OnDrawGizmosSelected()
    {
        //just to see the attack point and range lol
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
