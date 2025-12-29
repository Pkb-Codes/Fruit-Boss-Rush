using System.Runtime.InteropServices;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 6f;
    public float turnSpeed = 3f;
    public int damage = 20;
    public float aliveTime = 10f;

    private float timer = 0f;
    private float moveSpeed;
    private float homingDelay = 0.25f;
    private Transform player;
    private GameObject p;
    private Animator animator;
    private Collider2D col;
    private Vector2 moveDirection;
    private playerHealthScript healthscript;

    void Start()
    {
        p = GameObject.FindWithTag("Player");
        player = p.transform;
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        healthscript = p.GetComponent<playerHealthScript>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        //destroy after certain time
        if(timer > aliveTime)
        {
            Explode();
        }

        if(timer >= homingDelay)
        {
            moveSpeed = speed;

            // Direction of player
            Vector2 targetDir = ((Vector2)player.position - (Vector2)transform.position).normalized;

            // Smoothly bend toward player
            moveDirection = Vector2.Lerp(moveDirection, targetDir, turnSpeed * Time.deltaTime);

            // Rotate missile to face movement direction
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle+135);
        }

        // Move forward
        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);    
    }

    public void SetInitialDirection(Vector2 dir, float speed)
    {
        moveDirection = dir.normalized;
        moveSpeed = speed;
    }

    //get destroyed when touched ground or player
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ground") || collision.CompareTag("wall"))
        {
            Explode();
        }
        if(collision.CompareTag("Player"))
        {
            healthscript.TakeDamage(damage);
            Explode();
        }
    }

    void Explode()
    {
        animator.SetTrigger("Explode");
        col.enabled = false;
        speed = 0;
        turnSpeed = 0;
    }

    void End()
    {
        Destroy(gameObject);
    }
}
