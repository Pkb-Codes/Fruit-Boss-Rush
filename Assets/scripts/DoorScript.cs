using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public GameObject animGO;
    private Animator animator;
    private GameObject player;

    public int finalarenaEnemies = 4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = animGO.GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("enemy").Length <= 3+finalarenaEnemies)
        {
            if(Vector2.Distance(player.transform.position, transform.position) < 3)
            {
                animator.SetBool("Open",true);
                boxCollider.enabled = false;
            }

            if(Vector2.Distance(player.transform.position, transform.position) > 5)
            {
                animator.SetBool("Open",false);
                boxCollider.enabled = true;
            }
        }
    }
}
