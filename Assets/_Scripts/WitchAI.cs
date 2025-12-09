using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WitchAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float stopDistanceFromPlayer = 8f;   // she keeps this distance while shooting
    public float heightOffset = 0.5f;

    [Header("Grounding")]
    public LayerMask groundMask;
    public float groundRayHeight = 10f;
    public float groundRayDistance = 20f;

    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = false;
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        Vector3 toPlayer = player.position - transform.position;
        Vector3 flatDir = new Vector3(toPlayer.x, 0f, toPlayer.z);

        float distance = flatDir.magnitude;
        if (distance > 0.01f)
        {
            Vector3 moveDir = flatDir.normalized;

            // Rotate toward player on Y only
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 10f * Time.fixedDeltaTime));

            // Move only if farther than stop distance
            if (distance > stopDistanceFromPlayer)
            {
                Vector3 desiredPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;

                // Grounding: raycast down from above desiredPos
                Vector3 rayStart = new Vector3(desiredPos.x, groundRayHeight, desiredPos.z);
                if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, groundRayDistance, groundMask))
                {
                    desiredPos.y = hit.point.y + heightOffset;
                }

                rb.MovePosition(desiredPos);
            }
        }
    }
}