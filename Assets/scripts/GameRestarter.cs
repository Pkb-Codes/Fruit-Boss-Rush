using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    public GameObject endscreen;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BossDied()
    {
        Invoke("gameendscene", 5f);
    }

    void gameendscene()
    {
        endscreen.SetActive(true);
    }
}
