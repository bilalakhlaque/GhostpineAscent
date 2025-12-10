using UnityEngine;

public class WoodPickup : MonoBehaviour
{
    public int woodAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.AddWood(woodAmount);
        }

        SFXManager.Instance?.PlayPickupResource();
        Destroy(gameObject);
    }
}