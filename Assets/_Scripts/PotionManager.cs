using UnityEngine;
using System.Collections;

public class PotionManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject hermesPotionPrefab;
    public GameObject healthPotionPrefab;

    [Header("Counts")]
    public int hermesPotionCount = 2;
    public int healthPotionCount = 3;

    [Header("Spawn ranges")]
    public Vector2 spawnXRange = new Vector2(2f, 48f);
    public Vector2 spawnZRange = new Vector2(2f, 48f);

    [Header("Layers")]
    public LayerMask groundMask;
    public LayerMask obstacleMask;

    public int maxAttemptsPerPotion = 50;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // Wait a frame so terrain exists
        yield return null;

        for (int i = 0; i < hermesPotionCount; i++)
        {
            TrySpawnPotion(hermesPotionPrefab);
        }

        for (int i = 0; i < healthPotionCount; i++)
        {
            TrySpawnPotion(healthPotionPrefab);
        }
    }

    void TrySpawnPotion(GameObject prefab)
    {
        if (prefab == null) return;

        for (int attempt = 0; attempt < maxAttemptsPerPotion; attempt++)
        {
            float x = Random.Range(spawnXRange.x, spawnXRange.y);
            float z = Random.Range(spawnZRange.x, spawnZRange.y);

            Vector3 start = new Vector3(x, 20f, z);

            if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, 50f, groundMask))
            {
                Vector3 pos = hit.point + Vector3.up * 0.3f;

                if (!Physics.CheckSphere(pos, 0.5f, obstacleMask))
                {
                    Instantiate(prefab, pos, Quaternion.identity);
                    return;
                }
            }
        }

        Debug.LogWarning("[PotionManager] Failed to find potion spawn spot.");
    }
}