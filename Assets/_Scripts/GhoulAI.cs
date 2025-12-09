using UnityEngine;
using System.Collections.Generic;

//[RequireComponent(typeof(CharacterController))]
public class GhoulAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;          // Player

    [Header("Movement")]
    public float moveSpeed = 3.0f;
    private AStarPathfinding pathfinder;
    private List<Vector3> path;
    private float pathTimer = 0f;
    public float stoppingDistance = 1.5f;

    [Header("Grounding")]
    public LayerMask groundMask;      // Set to Ground in Inspector
    public float groundRayHeight = 5f;
    public float groundOffset = 0.9f; // How high Ghoul sits above the ground

    private CharacterController controller;

    void Start()
    {
        pathfinder = gameObject.AddComponent<AStarPathfinding>();
        //controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (target == null) return;

        // Recalculate path every 0.5 seconds
        pathTimer += Time.deltaTime;
        if (pathTimer > 0.5f)
        {
            path = pathfinder.FindPath(transform.position, target.position);
            pathTimer = 0;
        }

        // Move along path
        if (path != null && path.Count > 0)
        {
            Vector3 nextStep = path[0];
            transform.position = Vector3.MoveTowards(transform.position, nextStep, moveSpeed * Time.deltaTime);
            
            // If we reached the node, remove it
            if (Vector3.Distance(transform.position, nextStep) < 0.1f)
            {
                path.RemoveAt(0);
            }
        }
        StickToGround();
    }

    void StickToGround()
    {
        // Raycast down from above Ghoul to find ground
        Vector3 rayOrigin = transform.position + Vector3.up * groundRayHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundRayHeight * 2f, groundMask))
        {
            // Snap Y to ground + offset, leave XZ as is
            Vector3 pos = transform.position;
            pos.y = hit.point.y + groundOffset;
            transform.position = pos;
        }
    }
}