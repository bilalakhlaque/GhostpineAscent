using UnityEngine;
using System.Collections;

public class GrowingHomingBomb : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotateSpeed = 2f;

    [Header("Growth")]
    public float growTime = 2f;   // time to reach max size
    public float maxSize = 2f;    // final scale

    public float maxLifetime = 3f; // time before auto-destroy

    [Header("Damage")]
    public float physicalDamage = 2f;
    [TextArea] public string deathMessage = "You were obliterated by a witch's curse.";

    private Transform target;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;

        // Start tiny
        transform.localScale = Vector3.zero;
        StartCoroutine(GrowRoutine());

        // Auto-destroy if it never hits anything
        Destroy(gameObject, maxLifetime);   // 3–4 seconds
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 toPlayer = (target.position - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(toPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    IEnumerator GrowRoutine()
    {
        float t = 0f;
        while (t < growTime)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / growTime);
            float size = Mathf.Lerp(0f, maxSize, p);
            transform.localScale = Vector3.one * size;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.TakePhysicalDamage(physicalDamage, deathMessage);
        }
        else
        {
            PlayerDeathHandler.Die(deathMessage);
        }

        Destroy(gameObject);
        return;
    }

    // Destroy on hitting ground/obstacles by layer, NOT tag
    int obstacleLayer = LayerMask.NameToLayer("Obstacle");
    int groundLayer   = LayerMask.NameToLayer("Ground");

    if (other.gameObject.layer == obstacleLayer ||
        other.gameObject.layer == groundLayer)
    {
        Destroy(gameObject);
    }
}
}