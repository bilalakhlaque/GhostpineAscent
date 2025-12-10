using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class WitchShooter : MonoBehaviour
{
    [Header("Stats")]
    public float fireInterval = 2f;
    public float fireRange = 20f;

    [Header("Prefabs")]
    public GameObject projectilePrefab;      // GrowingHomingBomb prefab
    public Transform projectileSpawnPoint;   // tip of wand / hand

    [Header("Rotation")]
    public float rotateSpeed = 5f;

    private Transform player;
    private float timeSinceLastShot = 0f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
{
    if (player == null || projectilePrefab == null)
        return;

    Vector3 toPlayer = player.position - transform.position;
    float distance = toPlayer.magnitude;

    timeSinceLastShot += Time.deltaTime;
    if (distance <= fireRange && timeSinceLastShot >= fireInterval)
    {
        Shoot();
        timeSinceLastShot = 0f;
    }
}

    void Shoot()
    {
        Transform spawn = projectileSpawnPoint != null ? projectileSpawnPoint : transform;
        Instantiate(projectilePrefab, spawn.position, spawn.rotation);

        // GrowingHomingBomb handles movement and damage itself.
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayOneShot(SFXManager.Instance.enemyAttackClip);
    }
}