using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] GameObject deathMenuUI;
    public Transform respawnpoint;
    public Transform bossSpawnPoint;
    public GameObject Boss;
    public GameObject BossUi;
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
        player.transform.position = respawnpoint.position;
        player.GetComponent<playerHealthScript>().Respawn();

        Boss.GetComponent<BossAttackScript>().Restart();
        Boss.transform.position = bossSpawnPoint.position;
        BossUi.SetActive(false);

        animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        foreach (Animator anim in animators) {anim.speed = 1f;}

        isDead = false;
        deathMenuUI.SetActive(false);
    }
}
