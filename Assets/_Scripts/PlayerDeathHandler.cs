using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayerDeathHandler
{
    public static string LastCauseOfDeath = "";

    public static void Die(string cause)
    {
        LastCauseOfDeath = cause;
        SceneManager.LoadScene("DeathScene");
    }
}