using UnityEngine;

public class SaltDropper : MonoBehaviour
{
    [Header("Salt Ward Settings")]
    public GameObject saltWardPrefab;     // prefab to drop
    public Transform dropOrigin;          
    public float dropHeightOffset = 0.1f; 
    public int saltPerDrop = 1;           // how much salt this costs
    public KeyCode dropKey = KeyCode.Space;

    [Header("References")]
    public PlayerInventory inventory;     // where saltCount is stored

    void Update()
    {
        if (Input.GetKeyDown(dropKey))
        {
            TryDropSalt();
        }
    }

    void TryDropSalt()
    {
        if (inventory == null)
        {
            Debug.LogWarning("[SaltDropper] No PlayerInventory assigned.");
            return;
        }

        if (saltWardPrefab == null)
        {
            Debug.LogWarning("[SaltDropper] No saltWardPrefab assigned.");
            return;
        }

        if (inventory.saltCount < saltPerDrop)
        {
            GameLogger.Instance.Log("[SaltDropper] Not enough salt to drop.");
            return;
        }

        // Where to drop
        Vector3 pos = dropOrigin != null ? dropOrigin.position : transform.position;
        pos.y += dropHeightOffset;

        Instantiate(saltWardPrefab, pos, Quaternion.identity);

        inventory.saltCount -= saltPerDrop;
        GameLogger.Instance.Log($"[SaltDropper] Dropped salt. Remaining salt = {inventory.saltCount}");

        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayOneShot(SFXManager.Instance.dropSaltClip);
    }
}