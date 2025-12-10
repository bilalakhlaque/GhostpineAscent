using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerDeathHandler
{
    // This flag ensures death only happens once
    public static bool PlayerIsDead { get; private set; } = false;

    public static void Die(string message)
    {
        // Prevent multiple deaths or conflicts with win logic
        if (PlayerIsDead)
        {
            Debug.Log("[DeathHandler] Die() called but player is already dead.");
            return;
        }

        PlayerIsDead = true;

        Debug.Log("[Death] " + message);

        // Stop all audio, then play death sound once
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.StopAllSFX();
            SFXManager.Instance.PlayPlayerDeath();
        }

        // Load our Death Scene, '3'...
        SceneManager.LoadScene("DeathScene");
    }

    // Reset should be called when reloading the main game scene
    public static void ResetDeathState()
    {
        PlayerIsDead = false;
    }
}