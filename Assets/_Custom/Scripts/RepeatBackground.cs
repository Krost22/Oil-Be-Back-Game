using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    public float repeatWidth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > startPos.z - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
