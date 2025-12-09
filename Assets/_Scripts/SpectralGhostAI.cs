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
    }
}
///---------------------------------------------------------------------
/*
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class SpectralGhostAI : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;
    public float chasePlayerDistance = 8f;

    [Header("Movement")]
    public float speed = 3f;

    [Header("Floating")]
    public LayerMask groundMask;      // set to Ground
    public float floatHeight = 3f;    // height above ground
    public float groundRayHeight = 10f;

    private Transform currentOrbTarget;
    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // We want kinematic rigidbody for trigger-based movement
        rb.isKinematic = true;
        rb.useGravity = false;

        // Make sure collider is NOT trigger (orbs will be trigger)
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

    Transform ChooseTarget()
    {
        if (player == null) return GetOrbTarget();

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= chasePlayerDistance)
        {
            // Close enough: chase player directly
            currentOrbTarget = null;
            return player;
        }

        // Otherwise: hunt an orb
        return GetOrbTarget();
    }

    Transform GetOrbTarget()
    {
        // Clean nulls from active orbs
        OrbManager.activeOrbs.RemoveAll(t => t == null);

        if (OrbManager.activeOrbs.Count == 0)
        {
            currentOrbTarget = null;
            return null;
        }

        // If we already had a target and it's still valid, keep it
        if (currentOrbTarget != null && OrbManager.activeOrbs.Contains(currentOrbTarget))
            return currentOrbTarget;

        // Choose nearest orb
        Transform nearest = null;
        float bestDist = Mathf.Infinity;

        foreach (Transform orb in OrbManager.activeOrbs)
        {
            float d = (orb.position - transform.position).sqrMagnitude;
            if (d < bestDist)
            {
                bestDist = d;
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

        Vector3 dir = targetPos - transform.position;
        if (dir.sqrMagnitude < 0.001f) return;

        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;

        // Face movement direction
        Vector3 lookTarget = transform.position + dir;
        lookTarget.y = transform.position.y;
        transform.LookAt(lookTarget);
    }

    void StickToFloatHeight()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * groundRayHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundRayHeight * 2f, groundMask))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + floatHeight;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Orbs should be triggers with tag "Orb"
        if (other.CompareTag("Orb"))
        {
            // Remove from list and destroy
            OrbManager.activeOrbs.Remove(other.transform);
            Destroy(other.gameObject);

            if (currentOrbTarget == other.transform)
            {
                currentOrbTarget = null;
            }
        }
    }
} */