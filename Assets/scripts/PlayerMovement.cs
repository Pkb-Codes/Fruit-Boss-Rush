using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float jumpSpeed = 10f;
    private float move;
    private Animator animator;
    private Rigidbody2D body;

    public BoxCollider2D legCollider;

    bool isGrounded = true;

    bool isStanding = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        animator.SetBool("OnGround", isStanding);

        //input reading for horizontal movement
        if(Input.GetKey(KeyCode.A)) 
        {
            move = -1f;
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetTrigger("Move");
        }
        
        else if(Input.GetKey(KeyCode.D)) 
        {
            move = 1f;
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetTrigger("Move");
        }
        else 
        {
            move = 0f;
            animator.SetTrigger("Idle");
        }
        
        //horizontal movement
        body.linearVelocity = new Vector2(move * moveSpeed, body.linearVelocityY);

        //vertical movement
        if(Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.W))
        {
            if(isGrounded) {
                body.linearVelocity = new Vector2(body.linearVelocityX, jumpSpeed);
                isGrounded = false;
                isStanding = false;
                
            }
        }
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
