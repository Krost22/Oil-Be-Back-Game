using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float destroyDistance = 20f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (transform.position.x < player.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
