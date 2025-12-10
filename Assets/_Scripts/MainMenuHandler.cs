using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    void Start()
    {
        // Unlock and show cursor when entering any Menu/Death scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        // Reset game state before starting a new run
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetStateForNewRun();
        }
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}