using UnityEngine;
public class GhoulTrapDropper : MonoBehaviour
{
    [Header("Trap Settings")]
    public GameObject trapPrefab;
    public Transform trapSpawnPoint; 
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