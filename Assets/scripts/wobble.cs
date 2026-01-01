using UnityEngine;

public class wobble : MonoBehaviour
{
    public float floatHeight = 0.25f;   // how high it moves
    public float floatSpeed = 2f;       // how fast it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + Vector3.up * yOffset;
    }
}
