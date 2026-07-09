using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool isPaused = false;
    public bool isGameOver = false;
    private bool isSplashScreenActive = false;
    public int score = 0;
    
    [Header("Escenas")]
    public string mainMenuSceneName = "MainMenuScene";

    [Header("Elementos de Pausa")]
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject scoreButton;
    public GameObject exitButton;

    [Header("Elementos de Game Over")]
    public GameObject gameoverPanel;
    public GameObject gameoverText;
    public GameObject gameoverImage;
    public GameObject gameoverRestartButton;

    [Header("Elementos de Puntaje (Score)")]
    public TextMeshProUGUI hudScoreText;
    public TextMeshProUGUI gameoverScoreTextComponent;

    [Header("Elementos de Splash Screen")]
    public GameObject splashScreenPanel;
    public CanvasGroup splashScreenCanvasGroup;

    private void Start()
    {
        Time.timeScale = 1f;
        isSplashScreenActive = false;

        if (pauseButton != null) pauseButton.SetActive(true);
        if (scoreButton != null) scoreButton.SetActive(true);

        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameoverPanel != null) gameoverPanel.SetActive(false);

        UpdateScoreUI();

        if (splashScreenPanel != null && splashScreenCanvasGroup != null)
        {
            TriggerSplashScreen();
            StartCoroutine(FadeOutSplashScreen());
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && !isGameOver)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                TogglePause();
            }
        }

        if (isGameOver && !gameoverPanel.activeSelf)
        {
            ShowGameOver();
        }
    }

    public void TogglePause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        scoreButton.SetActive(false);
        pausePanel.SetActive(true);
        resumeButton.SetActive(true);
        restartButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        scoreButton.SetActive(true);
        pausePanel.SetActive(false);
        resumeButton.SetActive(false);
        restartButton.SetActive(false);
    }

    public void TriggerSplashScreen()
    {
        isSplashScreenActive = true;
        splashScreenPanel.SetActive(true);
        if (splashScreenCanvasGroup != null) splashScreenCanvasGroup.alpha = 1f;
    }

    public void ShowGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        scoreButton.SetActive(false);
        gameoverPanel.SetActive(true);
        gameoverText.SetActive(true);
        gameoverImage.SetActive(true);
        gameoverRestartButton.SetActive(true);

        if (gameoverScoreTextComponent != null)
        {
            gameoverScoreTextComponent.gameObject.SetActive(true);
            gameoverScoreTextComponent.text = "Score: " + score.ToString();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }
    
    private void UpdateScoreUI()
    {
        if (hudScoreText != null)
        {
            hudScoreText.text = score.ToString();
        }
    }

    private IEnumerator FadeOutSplashScreen()
    {
        yield return new WaitForSeconds(3f); 

        float duration = 2f;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            if (splashScreenCanvasGroup != null)
            {
                splashScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            }
            yield return null;
        }

        splashScreenPanel.SetActive(false);
        isSplashScreenActive = false;
    }
}