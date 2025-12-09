using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Per Hit")]
    public float physicalDamage = 1f;
    public float sanityDamage = 0f;

    [Header("Hit Settings")]
    public float hitCooldown = 1f;  // seconds between hits while touching

    [Header("Death Messages")]
    [TextArea]
    public string physicalDeathMessage = "Your body was torn apart.";
    [TextArea]
    public string sanityDeathMessage = "Your mind could not withstand the horrors.";

    private float _lastHitTime = -999f;

    public void TryApplyDamage(PlayerStats stats)
    {
        if (stats == null) return;

        if (Time.time - _lastHitTime < hitCooldown)
            return;

        bool didDamage = false;

        if (physicalDamage > 0f)
        {
            stats.TakePhysicalDamage(physicalDamage, physicalDeathMessage);
            didDamage = true;
        }

        if (sanityDamage > 0f)
        {
            stats.TakeSanityDamage(sanityDamage, sanityDeathMessage);
            didDamage = true;
        }

        if (didDamage)
        {
            _lastHitTime = Time.time;
        }
    }
}