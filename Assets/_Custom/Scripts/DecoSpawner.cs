using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecoSpawner : MonoBehaviour
{
    private Transform player;
    public GameObject[] spawnableDeco;
    public float spawnInterval = 2f;
    [SerializeField] private float destroyDistance;
    public float leftLimit;
    public float rightLimit;
    public float spawnPosFrontLeft;
    public float spawnPosFrontRight;
    void Start()
    {

        InvokeRepeating(nameof(SpawnLeftObject), spawnInterval, spawnInterval);
        InvokeRepeating(nameof(SpawnRightObject), spawnInterval, spawnInterval);
    }

    void SpawnLeftObject()
    {;
        Vector3 spawnPosition = new Vector3(leftLimit, -9, spawnPosFrontLeft);
        int obstacleIndex = Random.Range(0, spawnableDeco.Length);
        Instantiate(spawnableDeco[obstacleIndex], spawnPosition, spawnableDeco[obstacleIndex].transform.rotation);
    }

    void SpawnRightObject()
    {
        Vector3 spawnPosition = new Vector3(rightLimit, -9, spawnPosFrontRight);
        int obstacleIndex = Random.Range(0, spawnableDeco.Length);
        Instantiate(spawnableDeco[obstacleIndex], spawnPosition, spawnableDeco[obstacleIndex].transform.rotation);
    }

}
