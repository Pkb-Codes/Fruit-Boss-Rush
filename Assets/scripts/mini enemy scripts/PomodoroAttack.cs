using UnityEngine;

public class PomodoroAttack : MonoBehaviour
{

    public GameObject label;
    private SpriteRenderer labelsprite;
    public AudioClip beep;
    public AudioClip boom;
    private AudioSource audioSource;

    public float proximity = 7f;

    public GameObject player;
    private Transform playertransform;


    [Header("Warning Settings")]
    public float totalDuration = 3f;      // Total time before explosion
    public float startDelay = 0.8f;       // Slowest wait time (Start)
    public float endDelay = 0.05f;        // Fastest wait time (End)
    public float flashDuration = 0.1f;    // How long the red stays on per flash
    public Color flashColor = Color.red;  
    
    [Header("Explosion Settings")]
    public GameObject projectilePrefab;
    public int numberOfProjectiles = 8;
    public float projectileSpeed = 5f;
    public float radius = 0.5f;

    private SpriteRenderer mySprite;
    private Color originalColor;

    private bool preventspam = false;

    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        originalColor = mySprite.color;
        audioSource = GetComponent<AudioSource>();
        
        labelsprite = label.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        playertransform = player.transform;

        float distance = Vector2.Distance(transform.position, playertransform.position);
        if (distance < proximity)
        {
            if(preventspam == false)
            {
                StartCoroutine(WarningRoutine());
                preventspam = true;
            }
        }
    }

    System.Collections.IEnumerator WarningRoutine()
    {
        float startTime = Time.time;
        float timePassed = 0f;

        while (timePassed < totalDuration)
        {
            // 1. Calculate Progress (0.0 to 1.0)
            float progress = timePassed / totalDuration;

            // 2. FLASH ON
            mySprite.color = flashColor;
            audioSource.PlayOneShot(beep);
            yield return new WaitForSeconds(flashDuration); 

            // 3. FLASH OFF
            mySprite.color = originalColor;

            // 4. VARIABLE WAIT
            // As progress gets closer to 1, this delay gets smaller (faster flashing)
            float currentWait = Mathf.Lerp(startDelay, endDelay, progress);
            yield return new WaitForSeconds(currentWait);

            // Update time
            timePassed = Time.time - startTime;
        }

        Explode();
    }

    void Explode()
    {
        float angleStep = 360f / numberOfProjectiles;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 direction = rotation * Vector2.up;
            Vector2 spawnPos = (Vector2)transform.position + (direction * radius);

            GameObject proj = Instantiate(projectilePrefab, spawnPos, rotation);

            // Visual Layering (Behind Enemy)
            SpriteRenderer projSprite = proj.GetComponent<SpriteRenderer>();
            if (projSprite != null) projSprite.sortingOrder = mySprite.sortingOrder - 1;

            // Physics Launch
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = direction * projectileSpeed;
        }

        // Disable enemy visuals to simulate "destroyed"
        mySprite.enabled = false;
        labelsprite.enabled = false;
        audioSource.PlayOneShot(boom);
        Destroy(gameObject, boom.length);
    }
}

