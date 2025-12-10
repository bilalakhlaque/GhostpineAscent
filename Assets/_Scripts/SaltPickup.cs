using UnityEngine;

public class SaltPickup : MonoBehaviour
{
    public int saltAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerInventory inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            inv.AddSalt(saltAmount);
        }
        
        SFXManager.Instance?.PlayPickupResource();
        
        Destroy(gameObject);
    }
}