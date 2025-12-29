using System.Threading;
using UnityEngine;

public class RainInitiatorScript : MonoBehaviour
{
    public int count = 30;
    public GameObject RainSeed;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(0, 40);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y >= 10)
        {
            rb.linearVelocityY = 0;
            Destroy(gameObject);
            Explode();
        }
    }

    void Explode()
    {
        float spreadAngle = 180f;
        float angleStep = spreadAngle / (count - 1);
        float startAngle = 0;

        for(int i = 0;i < count;i++)
        {
            float angle = startAngle + angleStep * i;
            GameObject proj = Instantiate(RainSeed, transform.position, Quaternion.Euler(0, 0, angle));

            Vector2 direction = proj.transform.right;
            proj.GetComponent<RainSeedScript>().SetInitialDirection(direction, 8f);
        }
    }
}
