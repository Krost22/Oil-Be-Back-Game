using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject gameOverUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Muestra la pantalla de Game Over.
    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
    }
}
