using UnityEngine;

public class BossSpawner : MonoBehaviour
{

    public GameObject Boss;
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if(Vector3.Distance(player.transform.position, transform.position) < 25 && GameObject.FindGameObjectsWithTag("enemy").Length == 3)
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
