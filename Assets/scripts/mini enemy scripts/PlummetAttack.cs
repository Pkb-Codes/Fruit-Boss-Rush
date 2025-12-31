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

    public AudioClip groundpound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private bool isGrounded;
    

    private enum State {Idle, Jumping, Slamming, Recovering}
    private State currentState = State.Idle;

    private Coroutine patrolCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolCoroutine = StartCoroutine(PatrolRoutine());
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        player = playerGO.transform;
        if(currentState == State.Idle)
        {
            CheckForPlayer();
        }

        if(currentState == State.Jumping)
            TrackPlayerX();


    }

    void CheckForPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if(distance < detectionRange && isGrounded)
            StartJump();
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
                num = 1;
            else
                num = -1;
            Vector2 randomDir = new Vector2(num, 0);
            
            // Pick a random speed
            float randomSpeed = Random.Range(minSpeed, maxSpeed);

            // Apply velocity
            rb.linearVelocity = new Vector2(randomDir.x * randomSpeed, rb.linearVelocity.y);

            // Wait for a random amount of time while moving
            float moveDuration = Random.Range(minMoveTime, maxMoveTime);
            yield return new WaitForSeconds(moveDuration);


            // --- STATE 2: STOP ---
            
            // Stop movement
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            // Wait for a random amount of time while stopped
            float waitDuration = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitDuration);
        }
    }

    void StartJump()
    {
        currentState = State.Jumping;
        rb.linearVelocity = Vector3.up * jumpForce;
        isGrounded = false;
        StopCoroutine(patrolCoroutine);
        patrolCoroutine = null;
    }

    void TrackPlayerX()
    {
        // 3. While in the air, smoothy move X towards player's X
        float newX = Mathf.MoveTowards(transform.position.x, player.position.x, trackingSpeed * Time.deltaTime);
        transform.position = new Vector2(newX, transform.position.y);

        // 4. Check if we reached the peak of the jump (vertical velocity is near 0)
        if (rb.linearVelocity.y <= 0.5f)
        {
            StartCoroutine(PrepareSlam());
        }
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

            if(patrolCoroutine == null)
                patrolCoroutine = StartCoroutine(PatrolRoutine());
            isGrounded = true;
            currentState = State.Idle;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
           if(currentState == State.Slamming)
                audioSource.PlayOneShot(groundpound);
            StartCoroutine(HurtPlayer());
        }

    }



}
