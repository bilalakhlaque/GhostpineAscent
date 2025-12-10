using UnityEngine;
using System.Collections;

public class SaltManager : MonoBehaviour
{
    [Header("Salt Block Prefab")]
    public GameObject saltBlockPrefab;

    [Header("Counts")]
    public int saltBlockCount = 6;   // how many salt deposits to spawn

    [Header("Spawn ranges (world)")]
    public Vector2 spawnXRange = new Vector2(2f, 48f);
    public Vector2 spawnZRange = new Vector2(2f, 48f);

    [Header("Layers")]
    public LayerMask groundMask;     // Ground tiles
    public LayerMask obstacleMask;   // Mountains, trees, houses, etc.

    [Header("Placement Settings")]
    public float saltRadius = 0.6f;       // keep away from obstacles/other objects
    public int maxAttemptsPerSalt = 50;

    void Start()
    {
        // Wait a frame just in case terrain is spawned in Start() elsewhere
        StartCoroutine(SpawnSaltDelayed());
    }

    IEnumerator SpawnSaltDelayed()
    {
        yield return null; // one frame

        for (int i = 0; i < saltBlockCount; i++)
        {
            TrySpawnSaltBlock();
        }
    }

    void TrySpawnSaltBlock()
    {
        if (saltBlockPrefab == null)
        {
            Debug.LogWarning("[SaltManager] No saltBlockPrefab assigned.");
            return;
        }

        for (int attempt = 0; attempt < maxAttemptsPerSalt; attempt++)
        {
            float x = Random.Range(spawnXRange.x, spawnXRange.y);
            float z = Random.Range(spawnZRange.x, spawnZRange.y);

            Vector3 rayStart = new Vector3(x, 20f, z);

            // 1) Raycast down to find the ground
            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 50f, groundMask))
                continue;

            Vector3 pos = hit.point + Vector3.up * 0.3f; // slightly above ground

            // 2) Avoid obstacles (mountains, trees, houses) and too-tight spots
            if (Physics.CheckSphere(pos, saltRadius, obstacleMask))
                continue;

            // Success!!!!!!
            Instantiate(saltBlockPrefab, pos, Quaternion.identity);
            return;
        }

        Debug.LogWarning("[SaltManager] Failed to find spot for a salt block.");
    }
}