using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{

    public Transform attackPoint; //tracks location of attack point
    public float attackRange = 0.5f; //range of attack
    public int attackDamage = 20;
    public int knockbackForce = 2;
    public LayerMask enemyLayer;
    public float attackCooldown = 5f;


    private float timer = 0f;
    private int facingDirection = 1; 

    Vector3 attackPointOffset; //to make the attack point be in the direction the player is facing

    void Start()
    {
        attackPointOffset = attackPoint.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > attackCooldown && Input.GetKeyDown(KeyCode.I))
        {
            Attack();
            timer = 0f;
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

        //to move the attackpoint to the same offset in the other direction;
        attackPoint.localPosition = new Vector3(Mathf.Abs(attackPointOffset.x) * facingDirection, attackPointOffset.y, attackPointOffset.z);
    }


    void Attack ()
    {
        //keeps trcks of all enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            // makes each enemy take damage
            enemy.GetComponentInParent<EnemyHealthScript>().TakeDamage(attackDamage);
            enemy.GetComponentInParent<EnemyHealthScript>().Knockback(facingDirection, knockbackForce);
        }
    }

    void SpinSlash()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        //just to see the attack point and range lol
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
