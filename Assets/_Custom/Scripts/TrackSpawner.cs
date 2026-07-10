using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Lista de prefabs de secciones del tobogan. Cada uno puede incluir obstaculos, powerups y monedas.")]
    public List<GameObject> trackPrefabs;

    [Header("Spawn")]
    [Tooltip("Longitud de cada seccion en el eje X.")]
    private float segmentLength;

    [Tooltip("Numero de secciones que se mantienen activas a la vez.")]
    public int maxActiveSegments = 5;

    [Tooltip("Numero de secciones generadas al inicio.")]
    public int initialSegments = 3;

    [Header("Player")]
    [Tooltip("Transform del jugador. Si esta vacio, se busca automaticamente por tag Player.")]
    public Transform playerTransform;

    private Queue<GameObject> activeSegments = new Queue<GameObject>();
    private Vector3 nextSpawnPosition;
    private int lastSpawnedIndex = -1;

    // Busca al jugador si no fue asignado y genera las secciones iniciales.
    private void Start()
    {
        FindPlayer();
        nextSpawnPosition = transform.position;

        for (int i = 0; i < initialSegments; i++)
        {
            SpawnSegment();
        }
    }

    // Genera nuevas secciones por delante del jugador y destruye las mas antiguas.
    private void Update()
    {
        if (playerTransform == null || trackPrefabs.Count == 0) return;
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        float playerX = playerTransform.position.x;
        float spawnFrontier = playerX + (maxActiveSegments * segmentLength);

        while (nextSpawnPosition.x < spawnFrontier)
        {
            SpawnSegment();
        }

        while (activeSegments.Count > maxActiveSegments)
        {
            Destroy(activeSegments.Dequeue());
        }
    }

    // Encuentra al jugador por tag si no fue asignado en el inspector.
    private void FindPlayer()
    {
        if (playerTransform != null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // Instancia una nueva seccion aleatoria y la encola.
    private void SpawnSegment()
    {
        int randomIndex = GetRandomPrefabIndex();
        GameObject prefab = trackPrefabs[randomIndex];
        GameObject segment = Instantiate(prefab, nextSpawnPosition, Quaternion.identity);
        activeSegments.Enqueue(segment);
        // Se mide el tobogán con el mesh collider
        Renderer meshRenderer = segment.GetComponentInChildren<Renderer>();
        if (meshRenderer != null)
        {
            segmentLength = meshRenderer.bounds.size.x;
        }
        else
        {
            segmentLength = 10f;
        }
        nextSpawnPosition += new Vector3(segmentLength, 0f, 0f);
    }

    // Devuelve un indice aleatorio evitando repetir el mismo prefab dos veces seguidas.
    private int GetRandomPrefabIndex()
    {
        if (trackPrefabs.Count == 1) return 0;

        int index;
        do
        {
            index = Random.Range(0, trackPrefabs.Count);
        } while (index == lastSpawnedIndex);

        lastSpawnedIndex = index;
        return index;
    }
}
