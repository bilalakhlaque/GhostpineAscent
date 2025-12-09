using UnityEngine;

public class HotbarSpawner : MonoBehaviour
{
    [Header("Where to Spawn")]
    public Transform handHolder; // Drag your "Hand" or "WeaponHolder" object here

    [Header("Item Prefabs")]
    public GameObject pickaxePrefab; // Press 1
    public GameObject swordPrefab;   // Press 2
    public GameObject potionPrefab;  // Press 3

    private GameObject currentItem; // Track what we are holding

    void Update()
    {
        // 1. Pickaxe
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem(pickaxePrefab);
        }

        // 2. Sword
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipItem(swordPrefab);
        }

        // 3. Potion
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipItem(potionPrefab);
        }
    }

    void EquipItem(GameObject itemPrefab)
    {
        // If we are already holding something, destroy it first
        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        // If we have a valid prefab to spawn
        if (itemPrefab != null)
        {
            // Spawn the new item at the hand's position and rotation
            currentItem = Instantiate(itemPrefab, handHolder.position, handHolder.rotation);
            
            // Make it a child of the hand so it moves with the player
            currentItem.transform.SetParent(handHolder);
            
            // Optional: Reset local scale/position just in case the prefab is weird
            currentItem.transform.localPosition = Vector3.zero;
            currentItem.transform.localRotation = Quaternion.identity;
        }
    }
}