using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 5f;
    public float currentHealth;

    [Header("Sanity")]
    public float maxSanity = 5f;
    public float currentSanity;

    void Awake()
    {
        currentHealth = maxHealth;
        currentSanity = maxSanity;
    }

    public void TakePhysicalDamage(float amount, string deathMessageIfKilled)
    {
        if (amount <= 0f) return;

        currentHealth -= amount;
        Debug.Log($"[PlayerStats] Took {amount} physical damage. HP = {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            PlayerDeathHandler.Die(deathMessageIfKilled);
        }

        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayOneShot(SFXManager.Instance.playerHurtClip);
    }

    public void TakeSanityDamage(float amount, string deathMessageIfKilled)
    {
        if (amount <= 0f) return;

        currentSanity -= amount;
        Debug.Log($"[PlayerStats] Took {amount} sanity damage. Sanity = {currentSanity}/{maxSanity}");

        if (currentSanity <= 0f)
        {
            currentSanity = 0f;
            PlayerDeathHandler.Die(deathMessageIfKilled);
        }

        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayOneShot(SFXManager.Instance.playerHurtClip);
    }
}