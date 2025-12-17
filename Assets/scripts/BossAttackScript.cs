using UnityEngine;

public class BossAttackScript : MonoBehaviour
{

    public int touchDamage = 20; //amount of damage when player touched the boss

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the PlayerHealthScript from the object that actually collided
            playerHealthScript playerHealth = collision.GetComponent<playerHealthScript>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamage); //infllick touchDamage to the player
            }
        }
    }
}
