using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    [Header("Orb Settings")]
    public GameObject orbPrefab;
    public int orbCount = 5;

    [Header("Spawn Area (XZ)")]
    public Vector2 spawnXRange = new Vector2(-20f, 20f);
    public Vector2 spawnZRange = new Vector2(-20f, 20f);

    [Header("Ground Raycast")]
    public LayerMask groundMask;       // Set to Ground in Inspector
    public LayerMask obstacleMask;     // Set to Obstacles in Inspector
    public float raycastHeight = 50f;
    public float orbHeightOffset = 0.5f;
    public int maxAttemptsPerOrb = 30;

    // Static list for ghosts to find orbs
    public static List<Transform> activeOrbs = new List<Transform>();

    // IMPORTANT: use coroutine so we wait until after MountainGenerator.Start()
    IEnumerator Start()
    {
        // Wait one frame so the island has time to be generated
        yield return null;

        for (int i = 0; i < orbCount; i++)
        {
            SpawnOrb();
        }
    }

    void SpawnOrb()
    {
        for (int attempt = 0; attempt < maxAttemptsPerOrb; attempt++)
        {
            float x = Random.Range(spawnXRange.x, spawnXRange.y);
            float z = Random.Range(spawnZRange.x, spawnZRange.y);

            // Start ray high above and cast down
            Vector3 start = new Vector3(x, 20f, z);

            if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, 50f, groundMask))
            {
                Vector3 pos = hit.point + Vector3.up * 0.3f; // slight lift above floor

                // Avoid spawning too close to walls/trees
                if (!Physics.CheckSphere(pos, 0.6f, obstacleMask))
                {
                    GameObject orb = Instantiate(orbPrefab, pos, Quaternion.identity);
                    activeOrbs.Add(orb.transform);
                    return;
                }
            }
        }

        Debug.LogWarning("[OrbManager] Failed to find valid orb spawn after max attempts.");
    }

    public static Transform GetRandomOrb()
    {
        activeOrbs.RemoveAll(item => item == null); // Clean up destroyed orbs
        if (activeOrbs.Count == 0) return null;
        return activeOrbs[Random.Range(0, activeOrbs.Count)];
    }
}