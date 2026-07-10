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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(SpawnObject), 2f, spawnInterval);
    }


    void Update()
    {
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
        float spawnPosFront = player.position.x + 0f;
        Vector3 spawnPosition = new Vector3(Random.Range(-20, -12), -9, -46);
        int obstacleIndex = Random.Range(0, spawnablePrefabs.Length);
        Instantiate(spawnablePrefabs[obstacleIndex], spawnPosition, spawnablePrefabs[obstacleIndex].transform.rotation);
    }


}
