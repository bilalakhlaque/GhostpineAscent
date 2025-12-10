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
    public string loseSceneName = "DeathScene";

    [Header("Enemy Sanity Check")]
    public float enemyCheckInterval = 1f; // how often to verify enemies in the scene

    private float enemyCheckTimer = 0f;
    private bool gameEnded = false;       // prevents double win/lose

    private const string GameplaySceneName = "DemoScene";

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
        // Initial scan of enemies in whatever scene we first appear in.
        RescanEnemiesInScene();

        GameLogger.Instance.Log($"[GameManager] Initial enemy count = {enemiesAlive}");

        enemyCheckTimer = enemyCheckInterval;

        // Also listen for scene changes so we can reset things if needed
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Whenever we enter the gameplay scene, rescan enemies
        if (scene.name == GameplaySceneName)
        {
            RescanEnemiesInScene();
            enemyCheckTimer = enemyCheckInterval;
        }
    }

    void Update()
    {
        if (gameEnded) return;

        // We only run enemy sanity checks in the gameplay scene
        Scene current = SceneManager.GetActiveScene();
        if (current.name != GameplaySceneName) return;

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
        enemyCheckTimer = enemyCheckInterval;

        PlayerDeathHandler.ResetDeathState();

        // When the gameplay scene loads, RescanEnemiesInScene will populate enemiesAlive again.
        GameLogger.Instance.Log("[GameManager] State reset for new run.");
    }

    // ---------- Helper: rescan enemies in current scene ----------

    void RescanEnemiesInScene()
    {
        // Use EnemyHealth to find count
        EnemyHealth[] allEnemies = FindObjectsOfType<EnemyHealth>();
        int count = 0;
        foreach (var e in allEnemies)
        {
            if (e != null && e.countsAsEnemy)
                count++;
        }

        enemiesAlive = count;
        GameLogger.Instance.Log($"[GameManager] Rescan found {enemiesAlive} enemies in scene {SceneManager.GetActiveScene().name}.");
    }

    // ---------- Enemy Sanity Check ----------

    void SanityCheckEnemies()
    {
        // Make sure only in gameplay scene
        if (SceneManager.GetActiveScene().name != GameplaySceneName) return;

        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        int actualCount = enemiesInScene.Length;

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
            if (enemiesAlive != actualCount)
            {
                GameLogger.Instance.Log($"[GameManager] Syncing enemiesAlive from {enemiesAlive} -> {actualCount} based on scene.");
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
        // Prevent double-win or win triggering after death.
        if (gameEnded)
        {
            Debug.Log("[GameManager] Win() ignored because gameEnded is already true.");
            return;
        }

        if (PlayerDeathHandler.PlayerIsDead)
        {
            Debug.Log("[GameManager] Win() ignored because player is dead.");
            return;
        }

        gameEnded = true;

        Debug.Log("[GameManager] WIN: " + message);

        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.StopAllSFX();
            SFXManager.Instance.PlayPlayerWin();
        }

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