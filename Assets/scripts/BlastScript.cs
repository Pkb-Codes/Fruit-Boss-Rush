using UnityEngine;

public class BlastScript : MonoBehaviour
{
    private EnemyHealthScript enemyhealth;
    private GameObject player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("enemy"))
        {
            enemyhealth = collision.GetComponentInParent<EnemyHealthScript>();
        }

        if(collision.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }


    void GiveDmg()
    {
        if(player != null && Vector3.Distance(transform.position, player.transform.position) < 3)
        {
            player.GetComponent<playerHealthScript>().TakeDamage(60);
        }
    }

    void SummomCircle()
    {
        enemyhealth.ActicateBlastCircle();
    }

    void BossEnd()
    {
        enemyhealth.Vanish();
    }

    void End()
    {
        Destroy(gameObject);
    }
}
