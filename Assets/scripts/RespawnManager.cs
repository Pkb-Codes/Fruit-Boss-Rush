using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] GameObject deathMenuUI;
    Animator[] animators;
    public bool isDead;

    public void PlayerDied()
    {
        if (!isDead) return;

        isDead = true;
        deathMenuUI.SetActive(true);

        animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach (Animator anim in animators) {anim.speed = 0f;}
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<playerHealthScript>().Respawn();

        animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach (Animator anim in animators) {anim.speed = 1f;}

        isDead = false;
        deathMenuUI.SetActive(false);
    }
}
