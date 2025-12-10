using System.Collections.Generic;
using UnityEngine;

public class MountainGenerator : MonoBehaviour
{
    [Header("Mountain Settings")]
    public int mapSize = 50; // Size of the area
    public float scale = 8f; // How "zoomed in" the noise is
    
    [Header("Height Settings")]
    public float minMountainHeight = 2f; // Minimum height of a wall
    public float heightMultiplier = 15f; // How much the noise affects height
    
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
                float xCoord = (float)x / mapSize * scale;
                float zCoord = (float)z / mapSize * scale;
                float noiseVal = Mathf.PerlinNoise(xCoord + 100, zCoord + 100);

                // 2. Logic: High values = Walls (Mountain), Low values = Floor (Valley)
                // Threshold is 0.6f
                if (noiseVal > 0.6f) 
                {
                    // --- VARIABLE HEIGHT LOGIC ---
                    
                    // Calculate dynamic height based on how "intense" the noise is (0.6 to 1.0)
                    float noiseDifference = noiseVal - 0.6f; 
                    float finalHeight = minMountainHeight + (noiseDifference * heightMultiplier);

                    // Create the wall
                    // Note: If our pivot is in the CENTER, we move Y up by half the height so it sits on 0.
                    Vector3 mountainPos = new Vector3(x, finalHeight / 2f, z);
                    
                    GameObject wall = Instantiate(wallPrefab, mountainPos, Quaternion.identity, transform);
                    
                    // Apply the new height to the Y scale
                    wall.transform.localScale = new Vector3(1, finalHeight, 1);
                }
                else 
                {
                    // Spawn Floor (unchanged)
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