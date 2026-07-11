using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Speed")]
    public float baseGameSpeed = 5f;        // Velocidad normal del mundo.
    public float currentGameSpeed = 0f;     // Velocidad en tiempo real usada por escenarios y obstaculos.

    [Header("State")]
    public bool isGameOver = false;         // Indica si la partida termino.

    [Header("Score")]
    public int score = 0;                           // Puntuacion actual del jugador.

    [Header("Timer")]
    public float maxTime = 60f;                     // Tiempo inicial de la partida.
    public float remainingTime = 0f;                // Tiempo restante en segundos.

    [Header("Game Start")]
    public float gameStartTime;                     // Momento en el que inicio la partida.

    [Header("Oil VFX")]
    public ParticleSystem oilSlowdownVfx;           // VFX hijo del jugador. Se activa/desactiva segun el debuff.

    [Header("UI")]
    public TextMeshProUGUI timeText;                // Texto de la interfaz que muestra el tiempo.

    private Coroutine timerCoroutine;               // Referencia al contador.
    private Coroutine oilSlowdownCoroutine;         // Referencia a la corrutina de ralentizacion activa.
    private PlayerStatus playerStatus;              // Referencia al estado del jugador.

    public GameObject gameOverMenu;
    public TextMeshProUGUI gameOverScoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Inicializa la partida y establece la velocidad actual a la base.
    public void StartGame()
    {
        isGameOver = false;
        score = 0;
        remainingTime = maxTime;
        currentGameSpeed = baseGameSpeed;
        gameStartTime = Time.time;
        UpdateTimeText();
        SetOilVfxActive(false);

        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerStatus?.ResetStatus();

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    // Detiene el juego, la velocidad pasa a 0 y muestra la pantalla de derrota.
    public void EndGame()
    {
        if (isGameOver) return; 


        isGameOver = true;
        currentGameSpeed = 0f;
        gameOverMenu.SetActive(true);

        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Score: " + score;
        }
        if (oilSlowdownCoroutine != null)
        {
            StopCoroutine(oilSlowdownCoroutine);
            oilSlowdownCoroutine = null;
        }

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        SetOilVfxActive(false);
        if (playerStatus != null) playerStatus.isSlowed = false;

    }

    // Suma puntos a la puntuacion actual.
    public void AddScore(int points)
    {
        if (isGameOver) return;
        score += points;
    }

    // Suma segundos al temporizador.
    public void AddTime(float seconds)
    {
        if (isGameOver) return;
        remainingTime += seconds;
        UpdateTimeText();
    }

    // Actualiza el texto del tiempo en la interfaz.
    private void UpdateTimeText()
    {
        if (timeText == null) return;
        timeText.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    // Activa o desactiva el VFX de ralentizacion del jugador.
    private void SetOilVfxActive(bool active)
    {
        if (oilSlowdownVfx == null) return;
        oilSlowdownVfx.gameObject.SetActive(active);
    }

    // Reduce currentGameSpeed temporalmente al pisar aceite.
    public void ApplyOilSlowdown(float slowMultiplier, float duration)
    {
        if (isGameOver) return;

        if (oilSlowdownCoroutine != null)
        {
            StopCoroutine(oilSlowdownCoroutine);
        }

        SetOilVfxActive(true);
        if (playerStatus != null) playerStatus.isSlowed = true;
        oilSlowdownCoroutine = StartCoroutine(OilSlowdownRoutine(slowMultiplier, duration));
    }

    // Restaura currentGameSpeed a baseGameSpeed.
    public void RestoreGameSpeed()
    {
        if (isGameOver)
        {
            
            return;
        }

        currentGameSpeed = baseGameSpeed;
        if (playerStatus != null) playerStatus.isSlowed = false;

    }

    // Corrutina que aplica la ralentizacion y luego restaura la velocidad.
    private IEnumerator OilSlowdownRoutine(float slowMultiplier, float duration)
    {
        currentGameSpeed = baseGameSpeed * slowMultiplier;
        yield return new WaitForSeconds(duration);
        RestoreGameSpeed();
        SetOilVfxActive(false);
        oilSlowdownCoroutine = null;
    }

    // Cuenta regresiva del tiempo de partida.
    private IEnumerator TimerRoutine()
    {
        while (remainingTime > 0f)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
            UpdateTimeText();

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                EndGame();
            }
        }
    }
}
