using UnityEngine;
using System.Collections;

public class CameraShakeScript : MonoBehaviour
{
    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
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
}
