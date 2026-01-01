using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public GameObject animGO;
    private Animator animator;

    public int finalarenaEnemies = 4;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = animGO.GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectsWithTag("enemy").Length == 3+finalarenaEnemies)
        {
            animator.SetTrigger("OpenDoor");
            boxCollider.enabled = false;
        }
    }
}
