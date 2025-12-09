using UnityEngine;

public class SaltBlock : MonoBehaviour
{
    [Header("Salt Block Settings")]
    public int maxHits = 4;                    // how many pickaxe hits to break
    public GameObject saltBallPrefab;          // the pickup we drop
    public int saltDropped = 1;                // how much salt this block yields
    public Vector3 dropOffset = Vector3.up * 0.5f;

    private int currentHits = 0;

    // Called when hit by pickaxe
    public void TakePickaxeHit()
    {
        currentHits++;
        Debug.Log("Salt block hit " + currentHits + "/" + maxHits);

        if (currentHits >= maxHits)
        {
            BreakBlock();
        }
    }

    void BreakBlock()
    {
        if (saltBallPrefab != null)
        {
            Vector3 spawnPos = transform.position + dropOffset;
            GameObject pickup = Instantiate(saltBallPrefab, spawnPos, Quaternion.identity);
            
            // Optional: override amount on spawned pickup
            SaltPickup sp = pickup.GetComponent<SaltPickup>();
            if (sp != null)
            {
                sp.saltAmount = saltDropped;
            }
        }

        Destroy(gameObject);
    }
}