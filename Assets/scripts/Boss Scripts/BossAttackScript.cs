using System.Collections;
using UnityEngine;

public class BossAttackScript : MonoBehaviour
{

    public int touchDamage = 10;
    public float jumpForce = 1f;
    public float horizontalForce = 5f;
    public float attackTime = 3f;
    public float bounceDuration = 5f;
    public float meleeDashSpeed = 30f;
    public float meleeKnockbackForce = 100f;

    public GameObject Seed;
    public GameObject RainInitiator;
    public Transform initiatorSpawnPoint;
    public Transform SeedSpawnPoint;
    public Collider2D leftHand;
    public Collider2D rightHand;

    private float bounceTimer = 0f;
    private float attackTimer = 0f;
    private float roarDuration = 1.5f;
    private int attackCode;
    private int phase = 1;
    private float dir;
    private float slamShakeMagnitude = 0.1f;
    private float roarShakeMagnitude = 0.5f;

    private bool phase2triggered = false;
    private bool phase3triggered = false;
    public bool isBouncing = false;
    private bool dashing = false;
    private bool isIdle = true;
    public bool timerStop = false;

    [Header("Sfx")]
    public AudioClip scream;
    public AudioClip swipe;
    public AudioClip cannonballImpact;
    public AudioClip dash;
    public AudioClip seed;




    private Rigidbody2D rb;
    private Animator animator;
    private GameObject player;
    private EnemyHealthScript health;
    private AudioSource audioSource;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealthScript>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //to make a roar
        if(!phase2triggered && phase == 2)
        {
            health.invincible = true;
            health.flashTimer = -1;
            phase2triggered = true;
            StartCoroutine(RoarFreeze());
        }
        if(!phase3triggered && phase == 3)
        {
            health.invincible = true;
            health.flashTimer = -1;
            phase3triggered = true;
            StartCoroutine(RoarFreeze());
        }

        if(isIdle && !timerStop) {attackTimer += Time.deltaTime;}

        //manual controls to showoff the attacks
        // if(Input.GetKeyDown(KeyCode.H)) {MeleeAttackState();}
        // if(Input.GetKeyDown(KeyCode.J)) {SeedAttackState();}
        // if(Input.GetKeyDown(KeyCode.K)) {CannonballAttackState();}
        // if(Input.GetKeyDown(KeyCode.L)) {SeedRainState();}

        //phase 2
        if(health.currentHealth > 100 && health.currentHealth <= 200)
        {
            phase = 2;
        }
        //phase 3
        if(health.currentHealth <= 100)
        {
            phase = 3;
        }    


        //choosing random attacks based on phase
        if(attackTimer > attackTime)
        {
            switch(phase)
            {
                case 1: 
                    attackCode = Random.Range(1, 3); //gives a random number 1,2
                    break;
                case 2:
                    attackCode = Random.Range(1, 4); //gives a random number 1,2,3
                    break;
                case 3:
                    attackCode = Random.Range(1, 5); //gives a random number 1,2,3,4
                    break;
            }
            isIdle = false;
            attackTimer = 0;
        }

        switch(attackCode)
        {
            case 1:
                MeleeAttackState();
                break;
            case 2:
                SeedAttackState();
                break;
            case 3:
                CannonballAttackState();
                break;
            case 4:
                SeedRainState();
                break;
        }
        attackCode = 0;


        if(isBouncing)
        {
            bounceTimer += Time.deltaTime;
            leftHand.enabled = false;
            rightHand.enabled = false;

            if(rb.linearVelocityY <= 0) {rb.gravityScale = 20;}
            else {rb.gravityScale = 6;}
        }
        //cannonball attack end
        if(isBouncing && bounceTimer > bounceDuration)
        {
            isBouncing = false;
            animator.SetBool("IsBracing", false);
            bounceTimer = 0f;
            isIdle = true;
            leftHand.enabled = true;
            rightHand.enabled = true;
        }
        
    }

    void CannonballAttackState()
    {
        animator.SetBool("IsBracing", true);
        Invoke(nameof(CannonballAttack), 0.75f);
    }
    void MeleeAttackState()
    {
        if(player.transform.position.x - transform.position.x > 0) {dir = 1;}
        else {dir = -1;}

        if(dir == 1) {transform.localScale = new Vector3(-1, 1, 1);}
        else {transform.localScale = new Vector3(1, 1, 1);}

        if(Vector2.Distance(player.transform.position, transform.position) < 5) {animator.SetTrigger("Melee");}
        else {animator.SetTrigger("Dash");}
    }
    void SeedAttackState()
    {
        if(player.transform.position.x - transform.position.x > 0) {dir = 1;}
        else {dir = -1;}

        if(dir == 1) {transform.localScale = new Vector3(-1, 1, 1);}
        else {transform.localScale = new Vector3(1, 1, 1);}

        animator.SetTrigger("Missile");
        audioSource.clip = seed;
        audioSource.time = 0.1f;
        audioSource.Play();
    }
    void SeedRainState()
    {
        animator.SetTrigger("SeedRain");
        audioSource.clip = seed;
        audioSource.time = 0.1f;
        audioSource.Play();
    }

    void CannonballAttack()
    {
        float direction = Random.Range(-2f, 2f);
        if(Mathf.Abs(direction) < 0.5) {direction = dir * 1;}
        dir = direction;

        rb.linearVelocity = new Vector2(direction * horizontalForce, jumpForce);
        isBouncing = true;
    }
    void DashAttack()
    {
        audioSource.volume = 1.5f;
        audioSource.clip = dash;
        audioSource.time = 0.35f;
        audioSource.Play();

        rb.linearVelocityX = dir * meleeDashSpeed;
        leftHand.enabled = false;
        rightHand.enabled = false;

        dashing = true;
    }
    void Stop()
    {
        rb.linearVelocityX = 0;
    }
    void Melee()
    {
        audioSource.PlayOneShot(swipe, 1.5f);
        rb.linearVelocityX = dir * 10;
        dashing = true;
    }

    void SeedAttack()
    {
        int projectileCount = phase*2 - 1;

        float spreadAngle = 80f * dir;
        float angleStep;
        if(phase == 1) {angleStep = spreadAngle;}
        else {angleStep = spreadAngle / (projectileCount - 1);}
        float startAngle = -10f * dir;

        for(int i = 0;i < projectileCount;i++)
        {
            float angle = startAngle + angleStep * i;
            GameObject proj = Instantiate(Seed, SeedSpawnPoint.position, Quaternion.Euler(0, 0, angle));

            Vector2 direction = proj.transform.right * dir;
            proj.GetComponent<ProjectileScript>().SetInitialDirection(direction, 20f);
        }
    }
    void SeedRain()
    {
        Instantiate(RainInitiator, initiatorSpawnPoint.position, initiatorSpawnPoint.rotation);
    }
    void AttackEnd()
    {
        isIdle = true;
        dashing = false;
        leftHand.enabled = true;
        rightHand.enabled = true;
        audioSource.volume = 1f;
    }

    private IEnumerator RoarFreeze()
    {
        animator.SetTrigger("Roar");

        audioSource.clip = scream;
        audioSource.time = 0.4f;
        audioSource.Play();

        animator.SetBool("IsBracing", false);
        isBouncing = false;
        leftHand.enabled = true;
        rightHand.enabled = true;

        yield return new WaitForSecondsRealtime(roarDuration);

        audioSource.Stop();
        health.invincible = false;
    }

    void roarShake()
    {
        PlayerMovement playerMove = player.GetComponent<PlayerMovement>();
        playerMove.AddVel(10, transform);

        CameraShakeScript shake = Camera.main.GetComponent<CameraShakeScript>();
        StartCoroutine(shake.CameraShake(roarShakeMagnitude, roarDuration));
        health.invincible = false;
    }

    public void Restart()
    {
        health.currentHealth = health.maxHealth;
        phase = 1;
        timerStop = true;
        isBouncing = false;
        animator.SetBool("IsBracing", false);
        bounceTimer = 0f;
        phase2triggered = false;
        phase3triggered = false;
        health.spawned = false;
        health.TakeDamage(0);
        AttackEnd();
    }
    //touch damage
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealthScript from the object that actually collided
            playerHealthScript playerHealth = collision.gameObject.GetComponent<playerHealthScript>();
            PlayerMovement playermove = collision.gameObject.GetComponent<PlayerMovement>();

            if(playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamage); //infllict touchDamage to the player
                if(dashing)
                {
                    playermove.AddVel(10, transform);
                }
            }
        }

        if (isBouncing && collision.gameObject.CompareTag("ground"))
        {
            audioSource.clip = cannonballImpact;
            audioSource.time = 0.1f;
            audioSource.Play();


            float Hforce = dir * Random.Range(3, 7) *horizontalForce;
            float Vforce = jumpForce + Random.Range(-5, 15);

            rb.linearVelocity = new Vector2(Hforce, Vforce);

            CameraShakeScript shake = Camera.main.GetComponent<CameraShakeScript>();
            StartCoroutine(shake.CameraShake(slamShakeMagnitude, 0.5f));
        }
        
        if (isBouncing && collision.gameObject.CompareTag("wall"))
        {
            audioSource.clip = cannonballImpact;
            audioSource.time = 0.1f;
            audioSource.Play();

            float Hforce = dir * Random.Range(3, 7) *horizontalForce;

            rb.linearVelocityX = dir * Hforce;
            dir *= -1;

            CameraShakeScript shake = Camera.main.GetComponent<CameraShakeScript>();
            StartCoroutine(shake.CameraShake(slamShakeMagnitude, 0.5f));
        }
    }
}
