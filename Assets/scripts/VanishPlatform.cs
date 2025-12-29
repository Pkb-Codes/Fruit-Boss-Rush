using System.Threading;
using UnityEngine;

public class VanishPlatform : MonoBehaviour
{

    public float ontime = 5f;
    public float downtime = 2f;
    private float shaketime = 0.5f;
    private float timer = 0f;
    private bool isthere = true;

    private float shakeintensity = 0.12f;


    private SpriteRenderer spriteRenderer;
    private Collider2D boxCollider;
    private Color ogColor;
    private Vector3 pvposition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pvposition = transform.position;
        timer = 0f;

        ogColor = spriteRenderer.color;

        boxCollider.enabled = true;
        spriteRenderer.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(isthere == true && timer < ontime)
        {
            spriteRenderer.color = ogColor;
            timer += Time.deltaTime;
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;

        }

        else if(isthere == true && timer > ontime)
        {
            isthere = false;
            timer = 0f;
        }

        else if(isthere == false)
        {
            if(timer < shaketime)
            {
                spriteRenderer.color = Color.red;
                float x = Random.Range(-1f, 1f) * shakeintensity;
                float y = Random.Range(-1f, 1f) * shakeintensity;
                transform.position = pvposition + new Vector3(x, y, 0);
                timer += Time.deltaTime;
            }

            else
            {

                if(timer < downtime+shaketime)
                {
                    spriteRenderer.enabled = false;
                    boxCollider.enabled = false;
                    timer += Time.deltaTime;
                }
                else
                {
                    isthere = true;
                    timer = 0;
                }
            }
        }


    }
}
