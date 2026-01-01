using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlummetAttack : MonoBehaviour
{

    [Header("Settings")]
    public float detectionRange = 5f;
    public float jumpForce = 12f;
    public float trackingSpeed = 5f; // How fast it aligns with player while in air
    public float slamSpeed = 20f;
    public float hangTime = 0.5f; // How long it hovers before slamming

    public int damage = 30;

    [Header("References")]
    public GameObject playerGO;
    private Transform player;

    [Header("Movement Settings")]
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    [Header("Timing Settings")]
    public float minMoveTime = 1f;
    public float maxMoveTime = 3f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 4f;
    public float RestTime = 2f;
    public float jumpTime = 1f;

    public AudioClip groundpound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private SpriteRenderer sr;
    

    private enum State {Idle, Jumping, Slamming, Recovering}
    private State currentState = State.Idle;
    private float timer = 0f;
    private float jumptimer = 0f;
    private bool slamStarted = false;


    private Coroutine patrolCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = RestTime;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        patrolCoroutine = StartCoroutine(PatrolRoutine());
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        player = playerGO.transform;
        if(currentState == State.Idle)
        {
            CheckForPlayer();
        }

        if(currentState == State.Jumping)
        {
            TrackPlayerX();
            jumptimer += Time.deltaTime;
            if(jumptimer >= jumpTime && !slamStarted)
            {
                slamStarted = true;
                jumptimer = 0f;
                StartCoroutine(PrepareSlam());
            }
        }
    }

    void CheckForPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if(distance < detectionRange && isGrounded && timer > RestTime)
            animator.SetTrigger("Smash");
    }

    System.Collections.IEnumerator PatrolRoutine()
    {
        // This loop runs forever as long as the object is active
        while(true)
        {
            // --- STATE 1: MOVE ---
            // Pick a random direction (Length of 1)
            int num;
            if(Random.Range(0, 2) == 1)
            {
                num = 1;
                sr.flipX = false;
            }
            else
            {
                num = -1;
                sr.flipX = true;
            }
            Vector2 randomDir = new Vector2(num, 0);
            
            // Pick a random speed
            float randomSpeed = Random.Range(minSpeed, maxSpeed);

            // Apply velocity
            rb.linearDamping = 0;
            animator.SetBool("IsWalking", true);
            rb.linearVelocity = new Vector2(randomDir.x * randomSpeed, rb.linearVelocity.y);

            // Wait for a random amount of time while moving
            float moveDuration = Random.Range(minMoveTime, maxMoveTime);
            yield return new WaitForSeconds(moveDuration);


            // --- STATE 2: STOP ---
            
            // Stop movement
            rb.linearDamping = 1;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("IsWalking", false);

            // Wait for a random amount of time while stopped
            float waitDuration = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitDuration);
        }
    }

    void StartJump()
    {
        currentState = State.Jumping;
        isGrounded = false;
        slamStarted = false;
        
        rb.linearVelocity = Vector3.up * jumpForce;
        if(patrolCoroutine != null) 
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    void TrackPlayerX()
    {
        // 3. While in the air, smoothy move X towards player's X
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * trackingSpeed, rb.linearVelocity.y);
    }

    System.Collections.IEnumerator PrepareSlam()
    {
        currentState = State.Slamming;

        // Stop movement briefly (The "Wile E. Coyote" hang time)
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0; // Turn off gravity so it doesn't float down
        
        yield return new WaitForSeconds(hangTime);

        // 5. THE SLAM
        rb.gravityScale = 1; // Restore gravity
        rb.linearVelocity = Vector2.down * slamSpeed; // Shoot down
    }

    System.Collections.IEnumerator HurtPlayer()
    {
        playerHealthScript playerHealth = playerGO.GetComponent<playerHealthScript>();
        playerHealth.TakeDamage(damage);
        yield return new WaitForSeconds(2);
    }

    


    void OnCollisionEnter2D(Collision2D collision)
    {
        //checking if player touches the ground
        if (collision.gameObject.CompareTag("ground"))
        {
            if(currentState == State.Slamming)
                audioSource.PlayOneShot(groundpound);
            slamStarted = false;
            isGrounded = true;
            currentState = State.Idle;
            timer = 0;

            if(patrolCoroutine == null)
                patrolCoroutine = StartCoroutine(PatrolRoutine());

        }

        if (collision.gameObject.CompareTag("Player"))
        {
           if(currentState == State.Slamming)
                audioSource.PlayOneShot(groundpound);
            StartCoroutine(HurtPlayer());
        }

    }
}
