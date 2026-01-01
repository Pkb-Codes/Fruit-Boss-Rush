using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthScript : MonoBehaviour
{
    public float maxHealth = 300;
    public float flashTime = 1f; // to make the enemy flash red (will be removed when sprites are added)
    public float currentHealth;
    public bool invincible = true;
    public Image BossHealthFill;
    public GameObject BossHealthUI;
    public GameObject Blaster;
    public SpriteRenderer blastcircle;

    private Animator animator;
    public float flashTimer; // to make the enemy flash red (will be removed when sprites are added)
    private bool isFlash = false; // to make the enemy flash red (will be removed when sprites are added)
    public bool spawned = false;

    private int hitCount = 0;
    private SpriteRenderer spriterenderer;
    private Color originalColor; // to make the enemy flash red (will be removed when sprites are added)
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalColor = spriterenderer.color; // to make the enemy flash red (will be removed when sprites are added)
        flashTimer = flashTime; // to make the enemy flash red (will be removed when sprites are added)
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(isFlash)
        {
            spriterenderer.color = Color.black;
            flashTimer -= Time.deltaTime;
            if(flashTimer <= 0f)
            {
                spriterenderer.color = originalColor;
                isFlash = false;
                flashTimer = flashTime;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth <= 0) {return;}

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(!invincible)
        {
            //add damage sound here
            currentHealth -= damage;
            hitCount++;
            if(hitCount == 3)
            {
                player.GetComponent<playerHealthScript>().Heal();
            }
            isFlash = true; // to make the enemy flash red (will be removed when sprites are added)
            BossHealthFill.fillAmount = currentHealth / maxHealth;
        }

        PlayerMovement playerMove = player.GetComponent<PlayerMovement>();
        playerMove.TakeKnockback(0, 0);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Knockback(int direction, int force)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(force * direction, 0f), ForceMode2D.Impulse);
    }

    void Die()
    {
        //add death sound here
        BossAttackScript attack = GetComponent<BossAttackScript>();
        attack.isBouncing = false;
        attack.leftHand.enabled = true;
        attack.rightHand.enabled = true;

        animator.SetBool("IsBracing", false);

        animator.SetTrigger("Ded");
        CameraShakeScript shake = Camera.main.GetComponent<CameraShakeScript>();
        StartCoroutine(shake.CameraShake(0.5f, 2));

        Invoke("SummonBlast", 1f);
    }

    void SummonBlast()
    {
        Instantiate(Blaster, transform.position, transform.rotation);
    }

    public void ActicateBlastCircle()
    {
        blastcircle.enabled = true;
        StartCoroutine(FadeInBlast());
    }

    IEnumerator FadeInBlast()
    {
        BossHealthUI.SetActive(false);
        float alpha = 0f;

        while (alpha < 0.5)
        {
            alpha += Time.deltaTime / 0.5f;

            blastcircle.color = new Color(blastcircle.color.r, blastcircle.color.g, blastcircle.color.b, alpha);

            yield return null;
        }

        // clamp at exact value
        blastcircle.color = new Color(blastcircle.color.r, blastcircle.color.g, blastcircle.color.b, 0.5f);
        CameraShakeScript shake = Camera.main.GetComponent<CameraShakeScript>();
        shake.StartCoroutine(shake.CameraShake(3, 1.5f));
    }

    public void Vanish()
    {
        GameObject.FindGameObjectWithTag("gamemanager").GetComponent<GameRestarter>().BossDied();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(!spawned && collision.gameObject.CompareTag("ground"))
        {
            //add roar sound here
            animator.SetTrigger("Roar");
            BossHealthUI.SetActive(true);
            spawned = true;
        }
    }
}
