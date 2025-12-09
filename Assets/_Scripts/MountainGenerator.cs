using System.Collections.Generic;
using UnityEngine;

public class MountainGenerator : MonoBehaviour
{
    [Header("Mountain Settings")]
    public int mapSize = 50; // Size of the area
    public float scale = 8f; // How "zoomed in" the noise is
    
    [Header("Prefabs")]
    public GameObject floorPrefab; // The ground tile
    public GameObject wallPrefab; // The unclimbable mountains
    public GameObject obstaclePrefab; // Trees or rocks in the valley

    void Start()
    {
        GenerateValley();
    }

    void GenerateValley()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Vector3 pos = new Vector3(x, 0, z);
                
                // 1. Calculate height using Perlin Noise
                // We offset by 100 to get a different part of the noise texture
                float xCoord = (float)x / mapSize * scale;
                float zCoord = (float)z / mapSize * scale;
                float height = Mathf.PerlinNoise(xCoord + 100, zCoord + 100);

                // 2. Logic: High values = Walls (Mountain), Low values = Floor (Valley)
                if (height > 0.6f) 
                {
                    // Spawn Mountain Wall (Blocker)
                    Instantiate(wallPrefab, pos + Vector3.up, Quaternion.identity, transform);
                }
                else 
                {
                    // Spawn Floor
                    GameObject floor = Instantiate(floorPrefab, pos, Quaternion.identity, transform);
                    
                    // Optional: Randomly spawn an obstacle in the valley
                    if (Random.value > 0.95f)
                    {
                        Instantiate(obstaclePrefab, pos + Vector3.up * 0.5f, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}