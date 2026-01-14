using UnityEngine;
using System.Collections;
using System.Drawing;

public class CameraShakeScript : MonoBehaviour
{
    Vector3 originalPos;
    public float zoomSpeed = 5f;
    public float targetSize = 7f;
    public GameObject StartMenu;
    public GameObject health;
    // public GameObject ability;

    void Start()
    {
        originalPos = transform.localPosition;
        Time.timeScale = 0;
    }


    void Update()
    {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
    }


    public IEnumerator CameraShake(float magnitude, float duration)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            if (Time.timeScale == 0f) 
            {
                yield return null;
                continue;
            }

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void StartTime()
    {
        Time.timeScale = 1;
        StartMenu.SetActive(false);

        health.SetActive(true);
        // ability.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("camerazoom"))
        {
            targetSize = 13;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("camerazoom"))
        {
            targetSize = 7;
        }
    }
}
