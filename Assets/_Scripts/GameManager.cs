using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Enemy Tracking")]
    public int enemiesAlive = 0;

    [Header("Soul Orbs")]
    public int soulOrbsRemaining = 0;

    [Header("Win / Lose Scenes")]
    public string winSceneName = "WinScene";
    public string loseSceneName = "DeathScene"; // or your LoseScene name

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Re-scan enemies (you already added something like this)
        EnemyHealth[] allEnemies = FindObjectsOfType<EnemyHealth>();
        enemiesAlive = 0;
        foreach (var e in allEnemies)
        {
            if (e != null && e.countsAsEnemy)
                enemiesAlive++;
        }

        Debug.Log($"[GameManager] Initial enemy count = {enemiesAlive}");
    }

    // ---------- Enemies ----------

    public void RegisterEnemy()
    {
        enemiesAlive++;
        Debug.Log($"[GameManager] Enemy registered. Enemies alive = {enemiesAlive}");
    }

    public void UnregisterEnemy()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
        Debug.Log($"[GameManager] Enemy unregistered. Enemies alive = {enemiesAlive}");

        if (enemiesAlive == 0)
        {
            OnAllEnemiesDefeated();
        }
    }

    void OnAllEnemiesDefeated()
    {
        Debug.Log("[GameManager] All enemies defeated! Triggering win.");
        Win("You cleansed Ghostpine of its horrors.");
    }

    public void Win(string message)
    {
        Debug.Log("[GameManager] WIN: " + message);
        SceneManager.LoadScene(winSceneName);
    }

    // ---------- Soul Orbs ----------

    public void RegisterSoulOrb()
    {
        soulOrbsRemaining++;
        Debug.Log($"[GameManager] Soul orb registered. Remaining = {soulOrbsRemaining}");
    }

    public void OnSoulOrbDestroyedByGhost()
    {
        soulOrbsRemaining = Mathf.Max(0, soulOrbsRemaining - 1);
        Debug.Log($"[GameManager] Soul orb destroyed by ghost. Remaining = {soulOrbsRemaining}");

        if (soulOrbsRemaining == 0)
        {
            Lose("The ghosts devoured all the trapped souls.");
            PlayerDeathHandler.Die("The ghosts devoured all the trapped souls.");
        }
    }

    public void Lose(string message)
    {
        Debug.Log("[GameManager] LOSE: " + message);
        // Your existing Death/Lose pipeline; if PlayerDeathHandler already loads this scene,
        // you could call PlayerDeathHandler.Die(message) here instead.
        SceneManager.LoadScene(loseSceneName);
    }
}