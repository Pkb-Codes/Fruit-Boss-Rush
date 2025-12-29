using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    Animator[] animators;
    bool isPaused;

    void Awake()
    {
        animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    void Pause()
    {
        animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        foreach (Animator anim in animators) {anim.speed = 0f;}
        isPaused = true;
    }

    public void Resume()
    {
        animators = FindObjectsByType<Animator>(FindObjectsSortMode.None);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        foreach (Animator anim in animators) {anim.speed = 1f;}
        isPaused = false;
    }
}
