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

    [Header("Enemy Sanity Check")]
    public float enemyCheckInterval = 1f; // how often to verify enemies in the scene

    private float enemyCheckTimer = 0f;
    private bool gameEnded = false;       // prevents double win/lose

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
        // Initial scan of enemies using EnemyHealth (your original logic)
        EnemyHealth[] allEnemies = FindObjectsOfType<EnemyHealth>();
        enemiesAlive = 0;
        foreach (var e in allEnemies)
        {
            if (e != null && e.countsAsEnemy)
                enemiesAlive++;
        }

        GameLogger.Instance.Log($"[GameManager] Initial enemy count = {enemiesAlive}");

        // Start sanity check at the first interval
        enemyCheckTimer = enemyCheckInterval;
    }

    void Update()
    {
        if (gameEnded) return;

        // Periodically sanity-check enemies in case something died
        // without calling UnregisterEnemy (e.g. salt ward Destroy).
        enemyCheckTimer -= Time.deltaTime;
        if (enemyCheckTimer <= 0f)
        {
            enemyCheckTimer = enemyCheckInterval;
            SanityCheckEnemies();
        }
    }

    public void ResetStateForNewRun()
    {
    gameEnded = false;
    enemiesAlive = 0;
    soulOrbsRemaining = 0;
    // anything else you want to reset between runs
    }

    // ---------- Enemy Sanity Check ----------

    void SanityCheckEnemies()
    {
        // We’ll use the "Enemy" tag to cross-check what actually exists in the scene.
        // Make sure ALL enemy roots (ghosts, ghouls, witches) are tagged "Enemy".
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        int actualCount = enemiesInScene.Length;

        // If our counter says 0 OR the actual scene has 0, we treat that as victory.
        if (actualCount == 0)
        {
            if (!gameEnded)
            {
                GameLogger.Instance.Log("[GameManager] Sanity check: no enemies found in scene. Triggering win.");
                OnAllEnemiesDefeated();
            }
        }
        else
        {
            // Keep our counter in sync so the Debug logs stay meaningful
            if (enemiesAlive != actualCount)
            {
                GameLogger.Instance.Log($"[GameManager] Syncing enemiesAlive from {enemiesAlive} → {actualCount} based on scene.");
                enemiesAlive = actualCount;
            }
        }
    }

    // ---------- Enemies ----------

    public void RegisterEnemy()
    {
        enemiesAlive++;
        GameLogger.Instance.Log($"[GameManager] Enemy registered. Enemies alive = {enemiesAlive}");
    }

    public void UnregisterEnemy()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
        GameLogger.Instance.Log($"[GameManager] Enemy unregistered. Enemies alive = {enemiesAlive}");

        if (enemiesAlive == 0)
        {
            OnAllEnemiesDefeated();
        }
    }

    void OnAllEnemiesDefeated()
    {
        if (gameEnded) return;

        GameLogger.Instance.Log("[GameManager] All enemies defeated! Triggering win.");
        Win("You cleansed Ghostpine of its horrors.");
    }

    public void Win(string message)
    {
        if (gameEnded) return;
        gameEnded = true;

        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.StopAllSFX();
            SFXManager.Instance.PlayPlayerWin();
        }

        GameLogger.Instance.Log("[GameManager] WIN: " + message);
        SceneManager.LoadScene(winSceneName);
    }

    // ---------- Soul Orbs ----------

    public void RegisterSoulOrb()
    {
        soulOrbsRemaining++;
        GameLogger.Instance.Log($"[GameManager] Soul orb registered. Remaining = {soulOrbsRemaining}");
    }

    public void OnSoulOrbDestroyedByGhost()
    {
        soulOrbsRemaining = Mathf.Max(0, soulOrbsRemaining - 1);
        GameLogger.Instance.Log($"[GameManager] Soul orb destroyed by ghost. Remaining = {soulOrbsRemaining}");

        if (soulOrbsRemaining == 0)
        {
            // Let GameManager own the lose logic once.
            Lose("The ghosts devoured all the trapped souls.");
        }
    }

    public void Lose(string message)
{
    if (gameEnded) return;
    gameEnded = true;

    if (SFXManager.Instance != null)
    {
        SFXManager.Instance.StopAllSFX();
        SFXManager.Instance.PlayPlayerDeath();
    }

    GameLogger.Instance.Log("[GameManager] LOSE: " + message);

    SceneManager.LoadScene(loseSceneName);
}
}