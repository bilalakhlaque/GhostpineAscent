using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 3f;
    public bool destroyOnDeath = true;

    [Header("Is this an enemy for win tracking?")]
    public bool countsAsEnemy = true;

    private float currentHealth;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;

        if (countsAsEnemy && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemy();
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"[EnemyHealth] {gameObject.name} took {amount} damage. HP = {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[EnemyHealth] {gameObject.name} died.");

        if (countsAsEnemy && GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterEnemy();
        }

        // Later: drop loot, play effects etc.

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayOneShot(SFXManager.Instance.enemyDeathClip);

    }
}