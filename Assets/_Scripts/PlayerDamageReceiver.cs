using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerDamageReceiver : MonoBehaviour
{
    private PlayerStats stats;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogWarning("[PlayerDamageReceiver] No PlayerStats found on player.");
        }
    }

    // Called by CharacterController when we bump into something
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (stats == null) return;

        // Look for EnemyContactDamage on the thing we hit (or its parent)
        EnemyContactDamage damage =
            hit.collider.GetComponent<EnemyContactDamage>() ??
            hit.collider.GetComponentInParent<EnemyContactDamage>();

        if (damage != null)
        {
            damage.TryApplyDamage(stats);
        }
    }
}