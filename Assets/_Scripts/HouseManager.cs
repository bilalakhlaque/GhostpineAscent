using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HouseManager : MonoBehaviour
{
    [Header("House Prefab")]
    public GameObject housePrefab;

    [Header("Counts")]
    public int numberOfHouses = 2;

    [Header("Spawn ranges (world)")]
    public Vector2 spawnXRange = new Vector2(2f, 48f);
    public Vector2 spawnZRange = new Vector2(2f, 48f);

    [Header("Layers")]
    public LayerMask groundMask;    // Ground tiles
    public LayerMask obstacleMask;  // Mountains, trees, other houses

    [Header("Spacing")]
    public float houseRadius = 2f;     // radius around center
    public float minDistanceBetweenHouses = 8f;

    [Header("Vertical")]
    public float houseHeightOffset = 0.5f;  // tweak in Inspector

    public int maxAttemptsPerHouse = 100;

    void Awake()
    {
        // If masks are not set in Inspector, try to auto-configure.
        if (groundMask.value == 0)
        {
            groundMask = LayerMask.GetMask("Ground");
            Debug.Log("[HouseManager] groundMask not set; defaulting to Layer \"Ground\".");
        }

        if (obstacleMask.value == 0)
        {
            obstacleMask = LayerMask.GetMask("Obstacle");
            Debug.Log("[HouseManager] obstacleMask not set; defaulting to Layer \"Obstacle\".");
        }
    }

    void Start()
    {
        // Wait a frame so terrain / obstacles have time to spawn.
        StartCoroutine(SpawnHousesDelayed());
    }

    IEnumerator SpawnHousesDelayed()
    {
        // One frame delay; you can extend if needed.
        yield return null;
        SpawnHouses();
    }

    void SpawnHouses()
    {
        if (housePrefab == null)
        {
            Debug.LogWarning("[HouseManager] No housePrefab assigned.");
            return;
        }

        List<Vector3> occupiedPositions = new List<Vector3>();

        for (int i = 0; i < numberOfHouses; i++)
        {
            bool placed = false;

            for (int attempt = 0; attempt < maxAttemptsPerHouse; attempt++)
            {
                float x = Random.Range(spawnXRange.x, spawnXRange.y);
                float z = Random.Range(spawnZRange.x, spawnZRange.y);

                Vector3 rayStart = new Vector3(x, 20f, z);

                // 1) Hit ground?
                if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 50f, groundMask))
                {
                    continue; // no ground here
                }

                Vector3 pos = hit.point + Vector3.up * houseHeightOffset;

                /*2) Check obstacles (mountains, trees, other houses)
                if (Physics.CheckSphere(pos, houseRadius, obstacleMask))
                {
                    // too close to an obstacle; try again
                    continue;
                }
                /*
                // 3) Check spacing vs other houses we already placed
                bool tooClose = false;
                foreach (var p in occupiedPositions)
                {
                    if (Vector3.Distance(p, pos) < minDistanceBetweenHouses)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose)
                    continue; */

                // 4) Place house here
                Instantiate(housePrefab, pos, Quaternion.identity);
                occupiedPositions.Add(pos);
                placed = true;
                break;
            }

            if (!placed)
            {
                Debug.LogWarning($"[HouseManager] Failed to place house #{i}");
            }
        }
    }
}