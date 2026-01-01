using UnityEngine;

public class BossSpawner : MonoBehaviour
{

    public GameObject Boss;
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if(player.transform.position.x > 55 && player.transform.position.x < 80 && GameObject.FindGameObjectsWithTag("enemy").Length == 3)
        {
            Boss.GetComponent<BossAttackScript>().timerStop = false;
            GetComponent<Collider2D>().enabled = false;
            Invoke("ColActive", 2f);
        }
    }

    void ColActive()
    {
        GetComponent<Collider2D>().enabled = true;
    }
}
