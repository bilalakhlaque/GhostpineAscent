using UnityEngine;

public class WoodBlock : MonoBehaviour
{
    [Header("Wood Block Settings")]
    public int maxHits = 4;                    
    public GameObject woodLogPrefab;          
    public int woodDropped = 1;                
    public Vector3 dropOffset = Vector3.up * 0.5f;

    private int currentHits = 0;


    // Called when hit by pickaxe
    public void TakePickaxeHit()
    {
        currentHits++;
        GameLogger.Instance.Log("Wood block hit " + currentHits + "/" + maxHits);

        if (currentHits >= maxHits)
        {
            BreakBlock();
        }
    }

    void BreakBlock()
    {
        if (woodLogPrefab != null)
        {
            Vector3 spawnPos = transform.position + dropOffset;
            GameObject pickup = Instantiate(woodLogPrefab, spawnPos, Quaternion.identity);
            
            // Optional: override amount on spawned pickup
            WoodPickup sp = pickup.GetComponent<WoodPickup>();
            if (sp != null)
            {
                sp.woodAmount = woodDropped;
            }
        }

        Destroy(gameObject);
    }
}