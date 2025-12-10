using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Key Settings")]
    public int keyId = 0;   // must match houseId of the house it opens

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerKeys keys = other.GetComponent<PlayerKeys>();
        if (keys == null)
            return;

        keys.AddKey(keyId);
        SFXManager.Instance?.PlayPickupResource();
        Destroy(gameObject);
    }
}