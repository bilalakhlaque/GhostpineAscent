using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    void PlayClick()
    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayUIButtonClick();
        }
    }

    public void PlayGame()
    {
        PlayClick();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetStateForNewRun();
        }

        SceneManager.LoadScene("DemoScene"); 
    }

    public void ReturnToMenu()
    {
        PlayClick();
        SceneManager.LoadScene("MainMenu"); 
    }

    public void QuitGame()
    {
        PlayClick();
        Debug.Log("Quitting Game..");
        Application.Quit();
    }
}