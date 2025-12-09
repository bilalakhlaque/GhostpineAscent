using UnityEngine;
public class GhoulTrapDropper : MonoBehaviour
{
    [Header("Trap Settings")]
    public GameObject trapPrefab;
    public Transform trapSpawnPoint;  // assign in Inspector
    public float dropInterval = 3f;
    public float minDistanceFromPlayerToDrop = 2f;

    private Transform player;
    private float timeSinceLastDrop;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (trapPrefab == null || player == null || trapSpawnPoint == null)
            return;

        timeSinceLastDrop += Time.deltaTime;
        if (timeSinceLastDrop < dropInterval) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < minDistanceFromPlayerToDrop)
        {
            timeSinceLastDrop = 0f;
            return;
        }

        Instantiate(trapPrefab, trapSpawnPoint.position, trapSpawnPoint.rotation);
        timeSinceLastDrop = 0f;
    }
}


/*
using UnityEngine;

public class GhoulTrapDropper : MonoBehaviour
{
    [Header("Trap Settings")]
    public GameObject trapPrefab;
    public float dropInterval = 3f;
    public float trapOffsetBack = 1f;
    public float trapOffsetUp = 0.1f;

    [Header("Conditions")]
    public float minDistanceFromPlayerToDrop = 2f;

    private Transform player;
    private float timeSinceLastDrop = 0f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (trapPrefab == null || player == null)
            return;

        timeSinceLastDrop += Time.deltaTime;
        if (timeSinceLastDrop < dropInterval)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Optional: only drop when not right on top of the player
        if (distance < minDistanceFromPlayerToDrop)
        {
            timeSinceLastDrop = 0f;
            return;
        }

        // Spawn trap slightly behind the ghoul along its forward
        Vector3 spawnPos = transform.position - transform.forward * trapOffsetBack;
        spawnPos.y += trapOffsetUp;

        Instantiate(trapPrefab, spawnPos, Quaternion.identity);

        timeSinceLastDrop = 0f;
    }
} */