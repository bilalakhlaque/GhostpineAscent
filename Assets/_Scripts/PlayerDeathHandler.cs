using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerDeathHandler
{
    public static void Die(string reason)
    {
        Debug.Log("[Death] " + reason);

        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.StopAllSFX();
            SFXManager.Instance.PlayPlayerDeath(); // jingle right before swap (or move this to death scene)
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScene");
    }
}