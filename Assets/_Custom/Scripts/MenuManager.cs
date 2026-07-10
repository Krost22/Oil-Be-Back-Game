using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [Header("Configuración de Escenas")]
    [SerializeField] private string gameSceneName = "1_Game";

    [Header("Panel Primario")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Paneles Secundarios")]
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("botones de Menú")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject howToPlayButton;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject quitButton;

    public void PlayGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        quitButton.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        quitButton.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        quitButton.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        quitButton.SetActive(false);
    }

}
