using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObstacleSpawner : MonoBehaviour
{
    private Transform player;
    public GameObject[] spawnablePrefabs;
    public float spawnInterval = 2f;
    [SerializeField] private float spawnRange;
    [SerializeField] private float destroyDistance;
    private float difficultyTimer;
    void Start()
    {
        Debug.Log("Starting ObstacleSpawner");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(SpawnObject), 2f, spawnInterval);
    }


    void Update()
    {
        Debug.Log("Player position: " + player.position);
        // Aumenta la dificultad disminuyendo el intervalo entre cada spawn
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= 15f)
        {
            difficultyTimer = 0;
            if (spawnInterval > 0.5f)
            {
                spawnInterval -= 0.2f;
                CancelInvoke(nameof(SpawnObject));
                InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval);
            }
        }
    }

    void SpawnObject()
    {
        float spawnPosFront = player.position.x + 5f;
        Vector3 spawnPosition = new Vector3(spawnPosFront, 0, Random.Range(-spawnRange, spawnRange));
        int obstacleIndex = Random.Range(0, spawnablePrefabs.Length);
        Debug.Log("Spawning obstacle: " + spawnablePrefabs[obstacleIndex].name + " at position: " + spawnPosition);
        Instantiate(spawnablePrefabs[obstacleIndex], spawnPosition, spawnablePrefabs[obstacleIndex].transform.rotation);
    }


}
