using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class SpectralGhostAI : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;
    public float chasePlayerDistance = 8f;   // range to switch from orb → player

    [Header("Movement")]
    public float speed = 3f;
    public float stopDistanceFromPlayer = 2f;
    public float stopDistanceFromOrb = 1.5f;

    [Header("Floating")]
    public LayerMask groundMask;      // set to Ground layer
    public float floatHeight = 3f;
    public float groundRayHeight = 10f;

    private Transform currentOrbTarget;
    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Movement is manual (transform), physics just for triggers
        rb.isKinematic = true;
        rb.useGravity = false;

        // We want solid collider for hitbox, orbs will be trigger
        col.isTrigger = false;
    }

    void Update()
    {
        Transform targetToChase = ChooseTarget();

        if (targetToChase != null)
        {
            MoveTowardsTargetXZ(targetToChase);
        }

        StickToFloatHeight();
    }

    // Decide whether to chase player or an orb
    Transform ChooseTarget()
    {
        if (player == null)
            return GetOrbTarget();

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= chasePlayerDistance)
        {
            currentOrbTarget = null;
            return player;
        }

        return GetOrbTarget();
    }

    Transform GetOrbTarget()
    {
        OrbManager.activeOrbs.RemoveAll(t => t == null);  // clean up

        if (OrbManager.activeOrbs.Count == 0)
        {
            currentOrbTarget = null;
            return null;
        }

        if (currentOrbTarget != null && OrbManager.activeOrbs.Contains(currentOrbTarget))
            return currentOrbTarget;

        // Pick nearest orb
        Transform nearest = null;
        float bestDistSq = Mathf.Infinity;

        foreach (Transform orb in OrbManager.activeOrbs)
        {
            float d = (orb.position - transform.position).sqrMagnitude;
            if (d < bestDistSq)
            {
                bestDistSq = d;
                nearest = orb;
            }
        }

        currentOrbTarget = nearest;
        return currentOrbTarget;
    }

    void MoveTowardsTargetXZ(Transform target)
    {
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y; // move in XZ only

        Vector3 toTarget = targetPos - transform.position;
        float dist = toTarget.magnitude;
        if (dist < 0.001f) return;

        // Pick appropriate stop distance
        float stopDist =
            (target == player) ? stopDistanceFromPlayer : stopDistanceFromOrb;

        // If we are within stop distance, do not move closer (prevents pushing)
        if (dist <= stopDist)
            return;

        Vector3 dir = toTarget / dist;  // normalized

        // Move
        transform.position += dir * speed * Time.deltaTime;

        // Face direction of travel
        Vector3 lookTarget = transform.position + dir;
        lookTarget.y = transform.position.y;
        transform.LookAt(lookTarget);
    }

    void StickToFloatHeight()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * groundRayHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit,
                            groundRayHeight * 2f, groundMask))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + floatHeight;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            Debug.Log("Eating Orb!");

            // Keep OrbManager list clean
            OrbManager.activeOrbs.Remove(other.transform);

            // Use SoulOrb logic so GameManager is notified
            SoulOrb soul = other.GetComponent<SoulOrb>();
            if (soul != null)
            {
                soul.ConsumeByGhost();
            }
            else
            {
                Destroy(other.gameObject);
            }

            currentOrbTarget = null;
        }

        SFXManager.Instance?.PlayGhostEatOrb();
    }
}