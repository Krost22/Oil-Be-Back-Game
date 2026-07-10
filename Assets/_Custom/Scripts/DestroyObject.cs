using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float destroyDistance = 90f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (transform.position.z > destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
