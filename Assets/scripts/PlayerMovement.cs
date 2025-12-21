using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float jumpSpeed = 10f;
    private float move;
    public Animator animator;
    private Rigidbody2D body;

    bool isGrounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded == false)
            animator.SetTrigger("InTheAir");
        else
            animator.SetTrigger("OnGround");

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
        if(Input.GetKeyDown(KeyCode.W) && isGrounded) {
            body.linearVelocity = new Vector2(body.linearVelocityX, jumpSpeed);
            isGrounded = false;
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //checking if player touches the ground
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            animator.SetTrigger("Idle");
        }
    }
}
