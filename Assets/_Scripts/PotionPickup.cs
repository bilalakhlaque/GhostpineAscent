using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    public enum PotionType
    {
        FallProtection,
        Hermes,
        Health
    }

    public PotionType potionType = PotionType.FallProtection;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        TopDownController controller = other.GetComponent<TopDownController>();
        PlayerInventory inventory = other.GetComponent<PlayerInventory>(); // if you want to track potions

        switch (potionType)
        {
            case PotionType.FallProtection:
                if (controller != null)
                {
                    controller.hasFallProtection = true;
                    Debug.Log("[PotionPickup] Fall protection acquired.");
                }
                break;

            case PotionType.Hermes:
                if (controller != null)
                {
                    controller.hasHermesBlessing = true;
                    Debug.Log("[PotionPickup] Hermes blessing acquired.");
                }
                break;

            case PotionType.Health:
            {
                if (stats != null)
                {
                    float healAmount = 5f;

                    // Only heal if missing HP
                    if (stats.currentHealth < stats.maxHealth)
                    {
                        float oldHP = stats.currentHealth;

                        stats.currentHealth = Mathf.Min(stats.currentHealth + healAmount, stats.maxHealth);

                        float healed = stats.currentHealth - oldHP;

                        Debug.Log($"[PotionPickup] Health potion restored {healed} HP. Now {stats.currentHealth}/{stats.maxHealth}");
                    }
                    else
                    {
                        Debug.Log("[PotionPickup] Health potion wasted: already at full HP.");
                    }
                }
                break;
            }
        }

        // Later: play sound/VFX here
        if (SFXManager.Instance != null)
    SFXManager.Instance.PlayPickupPotion();
        
        Destroy(gameObject);
    }
}