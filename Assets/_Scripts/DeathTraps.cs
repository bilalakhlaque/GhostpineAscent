using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    [Header("Death message")]
    [TextArea]
    public string deathMessage = "You were shredded by a ghoul trap.";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            // Instantly kill by dealing full HP as damage
            stats.TakePhysicalDamage(stats.maxHealth, deathMessage);
        }
        else
        {
            // Fallback, if for some reason PlayerStats missing
            PlayerDeathHandler.Die(deathMessage);
        }
    }
}