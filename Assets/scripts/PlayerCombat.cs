using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{

    public Transform attackPoint; //tracks location of attack point
    public float attackRange = 0.5f; //range of attack
    public int attackDamage = 20;
    public int knockbackForce = 2;

    public LayerMask enemyLayer;
    public float attackCooldown = 3f;


    private float timer = 0f;
    private int facingDirection = 1; 


    [Header("Ability Things")]
    public int AbilityTime = 7; //How long the ability lasts

    public int CooldownTime = 10; //How long it takes to cool down


    private float abilityTimer = 0f;
    private bool abilityValid = false;
    private bool abTrigger = false;

    private SpriteRenderer spriteRenderer;

    private Animator animator;
    public BoxCollider2D spinHitbox;
    private Color originalColor;


    Vector3 attackPointOffset; //to make the attack point be in the direction the player is facing

    void Start()
    {
        attackPointOffset = attackPoint.localPosition;
        abilityTimer = AbilityTime;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        spinHitbox.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("abilityTimer: " + abilityTimer);
        timer += Time.deltaTime;
        

        //Ability spaghetticode
        if(Input.GetKey(KeyCode.P) && abilityValid == false && abTrigger == false)
        {
            abTrigger = true; //triggers the ability period
            abilityValid = true; //player is able to use ability now
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
        }







        if(timer > attackCooldown && Input.GetKey(KeyCode.I))
        {
            timer = 0f;
            Attack();
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


    void Attack ()
    {
        //keeps trcks of all enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            // makes each enemy take damage
            enemy.GetComponent<EnemyHealthScript>().TakeDamage(attackDamage);
            enemy.GetComponent<EnemyHealthScript>().Knockback(facingDirection, knockbackForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        //just to see the attack point and range lol
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
