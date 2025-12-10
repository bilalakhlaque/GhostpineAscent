using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerDeathHandler
{
    // Call this whenever the player should die for ANY reason.
    public static void Die(string reason)
    {
        GameLogger.Instance.Log("[Death] " + reason);

        // Prefer to go through GameManager so it sets gameEnded and handles SFX + scene.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Lose(reason);
        }
        else
        {
            // Fallback: if GameManager isn't in the scene for some reason
            //SceneManager.LoadScene("DeathScene");
        }
    }
}