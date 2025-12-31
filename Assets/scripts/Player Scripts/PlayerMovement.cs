using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float jumpSpeed = 10f;
    public float knockbackTime = 0.5f;
    public float hitKnockForce = 5f;
    public BoxCollider2D legCollider;

    public AudioClip walk;
    public AudioClip jump;

    private AudioSource audioSource;

    private float knockbackTimer = 0f;
    private float move;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    bool isKnocked = false;
    bool isGrounded = true;
    bool isStanding = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetBool("OnGround", isStanding);

        //cannon move if getting knocked
        if(!isKnocked)
        {
            //input reading for horizontal movement
            if(Input.GetKey(KeyCode.A)) 
            {
                move = -1f;
                if (!audioSource.isPlaying && isGrounded)
                    audioSource.PlayOneShot(walk);
                transform.localScale = new Vector3(-1, 1, 1);
                animator.SetTrigger("Move");
            }
            
            else if(Input.GetKey(KeyCode.D)) 
            {
                move = 1f;
                if (!audioSource.isPlaying && isGrounded)
                    audioSource.PlayOneShot(walk);
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetTrigger("Move");
            }
            else 
            {
                move = 0f;
                animator.SetTrigger("Idle");
            }

            rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocityY);
        }
        
        if(isKnocked)
        {
            knockbackTimer += Time.deltaTime;

            if(knockbackTimer >= knockbackTime)
            {
                isKnocked = false;
                knockbackTimer = 0f;
                knockbackTime = 0.1f;
            }
        }
        //horizontal movement

        //vertical movement
        if(Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.W))
        {
            if(isGrounded) {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpSpeed);
                isGrounded = false;
                isStanding = false;
                audioSource.PlayOneShot(jump);

                
            }
        }
    }

    public void TakeKnockback(float force, float dir)
    {
        if(force == 0) {force = hitKnockForce;}
        if(dir == 0) 
        {
            if(transform.localScale == new Vector3(1,1,1)) {dir = -1;}
            else{dir = 1;}
        }

        PlayerCombat playerAttack = GetComponent<PlayerCombat>();
        if(playerAttack.spinHitbox.enabled == true) {return;}

        knockbackTimer = 0f;
        isKnocked = true;
        rb.linearVelocity = new Vector2(0, 0);
        rb.AddForce(new Vector2(force * dir, 0), ForceMode2D.Impulse);
    }

    public void AddVel(float speed, Transform trans)
    {
        isKnocked = true;
        knockbackTime = 0.25f;
        rb.linearVelocity = speed * new Vector3(transform.position.x - trans.position.x, transform.position.y - trans.position.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //checking if player touches the ground
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            isStanding = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the specific collider that touched 'other' is our legCollider
        if (legCollider.IsTouching(other))
        {
            isStanding = true;
        }
    }
}
