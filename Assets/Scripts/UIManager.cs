using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    // Pause UI Elements

    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject scoreButton;

    // Game Over UI Elements

    public GameObject gameoverPanel;
    public GameObject gameoverText;
    public GameObject gameoverImage;
    public GameObject gameoverRestartButton;
    public GameObject gameoverScoreText;

    private void Start()
    {
        // 1. Nos aseguramos de que el tiempo corra normalmente de entrada
        Time.timeScale = 1f;

        // 2. Encendemos la interfaz del juego normal (iconos de pausa, estrellas/score)
        if (pauseButton != null) pauseButton.SetActive(true);
        if (scoreButton != null) scoreButton.SetActive(true);

        // 3. Apagamos los paneles que no se deben ver al empezar
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameoverPanel != null) gameoverPanel.SetActive(false);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        scoreButton.SetActive(false);
        pausePanel.SetActive(true);
        resumeButton.SetActive(true);
        restartButton.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        scoreButton.SetActive(true);
        pausePanel.SetActive(false);
        resumeButton.SetActive(false);
        restartButton.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        pauseButton.SetActive(false);
        scoreButton.SetActive(false);
        gameoverPanel.SetActive(true);
        gameoverText.SetActive(true);
        gameoverImage.SetActive(true);
        gameoverRestartButton.SetActive(true);
        gameoverScoreText.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
