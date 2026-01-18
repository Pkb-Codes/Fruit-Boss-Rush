using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int upperlimit = 0;
    private int middlelimit = -10;

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
    bool ceilingKnock = false;


    public GameObject label;
    private SpriteRenderer labelsprite;
    private float alpha = 1f;
    public float staytime = 2f;
    private float timeelapsed = 0f;
    public float fadetime = 1f;
    private Color currentColor;
    public bool is67 = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        labelsprite = label.GetComponent<SpriteRenderer>();
        currentColor = labelsprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(is67 == true)
        {
        if(timeelapsed > staytime)
        {
            if((timeelapsed - staytime) < fadetime)
            {
                alpha = Mathf.Lerp(1f, 0f, (timeelapsed-staytime)/fadetime);
                labelsprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                timeelapsed += Time.deltaTime;

            }
            else
                labelsprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        }
        else
            timeelapsed += Time.deltaTime;
        }

        //ceilingknock is to prevent player from flying off during the shakecamera scenes
        if(ceilingKnock == false)
        {
            if(transform.position.y < upperlimit && transform.position.y >= middlelimit)
                ceilingKnock = true;
        }

        if(ceilingKnock == true)
        {
            if(transform.position.y >= upperlimit)
            {
                ceilingKnock = false;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f);
            }

            if(transform.position.y < middlelimit)
                ceilingKnock = false;
        }

        animator.SetBool("OnGround", isStanding);

        if(transform.position.y < -22)
        {
            isGrounded = true;
            isStanding = true;
        }

        //cannon move if getting knocked
        if(!isKnocked && GetComponent<PlayerCombat>().isDashing == false)
        {
            //input reading for horizontal movement
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
            {
                move = -1f;
                if (!audioSource.isPlaying && isGrounded)
                    audioSource.PlayOneShot(walk);
                transform.localScale = new Vector3(-1, 1, 1);
                animator.SetTrigger("Move");
            }
            
            else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
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
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
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
        // if(playerAttack.spinHitbox.enabled == true) {return;}

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
            isGrounded = true;
        }
    }

    
}

