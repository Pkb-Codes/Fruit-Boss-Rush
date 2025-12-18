using UnityEngine;

public class BossAttackScript : MonoBehaviour
{

    public int touchDamage = 20; //amount of damage when player touched the boss

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealthScript from the object that actually collided
            playerHealthScript playerHealth = collision.gameObject.GetComponent<playerHealthScript>();
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamage); //infllict touchDamage to the player
            }
        }
    }
}
