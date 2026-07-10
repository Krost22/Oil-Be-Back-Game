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
        Debug.Log("Player position: " + player.position.z);
        Debug.Log("Object position: " + transform.position.z);
        if (transform.position.z < player.position.z - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
