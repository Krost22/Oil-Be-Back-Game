using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [Header("Chase")]
    [Tooltip("Velocidad normal de persecucion.")]
    public float baseChaseSpeed = 5f;

    [Tooltip("Velocidad de persecucion cuando el jugador esta stuneado/ralentizado.")]
    public float stunnedChaseSpeed = 8f;

    [Tooltip("Velocidad maxima cuando el jugador se aleja mucho.")]
    public float maxChaseSpeed = 12f;

    [Tooltip("Distancia a partir de la cual la gota acelera para alcanzar al jugador.")]
    public float catchUpDistance = 15f;

    [Header("Grace Period")]
    [Tooltip("Segundos iniciales donde la gota empuja al jugador en lugar de causar Game Over.")]
    public float gracePeriod = 3f;

    [Tooltip("Fuerza del empujon hacia el eje X positivo durante el periodo de gracia.")]
    public float pushForce = 10f;

    [Tooltip("Tiempo que se resta al jugador al empujarlo durante el periodo de gracia.")]
    public float pushTimePenalty = 5f;

    [Tooltip("Cooldown entre empujones en segundos.")]
    public float pushCooldown = 1f;

    [Header("Player")]
    [Tooltip("Referencia al PlayerStatus del jugador.")]
    public PlayerStatus playerStatus;

    [Tooltip("Tag del jugador.")]
    public string playerTag = "Player";

    private float lastPushTime;

    // Busca PlayerStatus si no fue asignado en el inspector.
    private void Start()
    {
        if (playerStatus == null)
        {
            playerStatus = FindAnyObjectByType<PlayerStatus>();
        }
    }

    // Persigue al jugador continuamente intentando tocarlo.
    private void Update()
    {
        if (playerStatus == null) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.isGameOver) return;
        if (GameManager.Instance.currentGameSpeed <= 0f) return;

        Transform playerTransform = playerStatus.transform;
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y = transform.position.y;

        float distance = Vector3.Distance(transform.position, targetPosition);
        float currentSpeed = CalculateChaseSpeed(distance);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }

    // Calcula la velocidad de persecucion segun el estado del jugador y la distancia.
    private float CalculateChaseSpeed(float distanceToPlayer)
    {
        float speed = baseChaseSpeed;

        if (playerStatus.isSlowed)
        {
            speed = stunnedChaseSpeed;
        }

        if (distanceToPlayer > catchUpDistance)
        {
            speed = maxChaseSpeed;
        }

        return speed;
    }

    // Detecta contacto con el jugador.
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.currentGameSpeed <= 0f) return;

        // Durante el periodo de gracia la gota empuja al jugador para darle chance de escapar.
        if (IsInGracePeriod())
        {
            PushPlayer(other);
            return;
        }

        GameManager.Instance.EndGame();
    }

    // Comprueba si aun estamos dentro del periodo de gracia desde el inicio de la partida.
    private bool IsInGracePeriod()
    {
        return Time.time - GameManager.Instance.gameStartTime < gracePeriod;
    }

    // Empuja al jugador hacia adelante y le resta tiempo.
    private void PushPlayer(Collider other)
    {
        if (Time.time - lastPushTime < pushCooldown) return;

        Rigidbody playerRb = other.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.AddForce(Vector3.right * pushForce, ForceMode.Impulse);
        }

        GameManager.Instance?.AddTime(-pushTimePenalty);
        lastPushTime = Time.time;
    }
}
