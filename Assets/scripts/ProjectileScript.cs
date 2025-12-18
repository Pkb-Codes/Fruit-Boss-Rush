using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 6f;
    public float turnSpeed = 3f;
    public int damage = 20;
    public float aliveTime = 20f;

    private float timer = 0f;
    private Transform player;
    private Vector2 moveDirection;
    private GameObject p;
    private playerHealthScript healthscript;

    void Start()
    {
        p = GameObject.FindWithTag("Player");
        player = p.transform;
        healthscript = p.GetComponent<playerHealthScript>();

        // Initial direction (forward)
        moveDirection = Vector2.right;
    }

    void Update()
    {
        timer += Time.deltaTime;

        //destroy after certain time
        if(timer > aliveTime)
        {
            Destroy(gameObject);
        }

        // Direction of player
        Vector2 targetDir = ((Vector2)player.position - (Vector2)transform.position).normalized;

        // Smoothly bend toward player
        moveDirection = Vector2.Lerp(moveDirection, targetDir, turnSpeed * Time.deltaTime);

        // Move forward
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    //get destroyed when touched ground or player
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ground"))
        {
            Destroy(gameObject);
        }

        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            //damage player
            healthscript.TakeDamage(damage);
        }
    }
}
