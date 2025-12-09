/*
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Bounds & Falling")]
    public bool hasFallProtection = false;
    public Vector2 clampXRange = new Vector2(1f, 48f);
    public Vector2 clampZRange = new Vector2(1f, 48f);
    public float fallDeathY = -5f;  // if player falls below this and has no protection → death

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Simple WASD movement in world XZ
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0f, z);
        if (move.sqrMagnitude > 1f)
            move.Normalize();

        controller.SimpleMove(move * moveSpeed);

        // Face movement direction
        if (move.sqrMagnitude > 0.001f)
        {
            Vector3 lookDir = new Vector3(move.x, 0f, move.z);
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        ApplyBoundsOrFall();
    }

    void ApplyBoundsOrFall()
    {
        Vector3 pos = transform.position;

        if (!hasFallProtection)
        {
            // If allowed to fall, check death condition
            if (pos.y < fallDeathY)
            {
                PlayerDeathHandler.Die("You fell from Ghostpine.");
            }
        }
        else
        {
            // Fall protection ON → clamp X/Z so we stay on the island
            pos.x = Mathf.Clamp(pos.x, clampXRange.x, clampXRange.y);
            pos.z = Mathf.Clamp(pos.z, clampZRange.x, clampZRange.y);
            transform.position = pos;
        }
    }
}
*/
///--------------------------------------------------------------------- BELOW WORKS RN

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TopDownController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public Transform cameraTransform;
    [Header("Bounds & Falling")]
    public Vector2 clampXRange = new Vector2(1f, 48f);
    public Vector2 clampZRange = new Vector2(1f, 48f);
    public float fallDeathY = -5f;  // if player falls below this and has no protection → death
    [Header("Sprint / Potions")]
    public bool hasFallProtection = false;
    public bool hasHermesBlessing = false;
    public float sprintMultiplier = 2f;
    public KeyCode sprintKey = KeyCode.LeftShift;


    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //usually false but true for testing
    }

    void Update()
    {
        HandleMovement();
        ApplyBoundsOrFall();
    }
        
    void HandleMovement() {
        if (cameraTransform == null) return;

        // Camera-relative directions on XZ plane
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = camForward * v + camRight * h;
        if (moveDir.sqrMagnitude > 1f)
            moveDir.Normalize();

        // Apply sprint multiplier
        if (hasHermesBlessing && Input.GetKey(sprintKey))
        {
            moveDir *= sprintMultiplier;
        }

        // Move via CharacterController
        controller.SimpleMove(moveDir * moveSpeed);

        // Rotate player towards movement direction
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 10f
            );
        }
        /* BOUNDARY CHECK needs to be improved imo. 
        float mapLimit = 50f; // Assuming map size 50, keep them inside 0-48
        Vector3 clamedPos = transform.position;
        
        clamedPos.x = Mathf.Clamp(clamedPos.x, 1f, mapLimit);
        clamedPos.z = Mathf.Clamp(clamedPos.z, 1f, mapLimit);
        
        transform.position = clamedPos; */
    }
    void ApplyBoundsOrFall()
    {
        Vector3 pos = transform.position;

        if (!hasFallProtection)
        {
            // If allowed to fall, check death condition
            if (pos.y < fallDeathY)
            {
                PlayerDeathHandler.Die("You fell from Ghostpine.");
            }
        }
        else
        {
            // Fall protection ON → clamp X/Z so we stay on the island
            pos.x = Mathf.Clamp(pos.x, clampXRange.x, clampXRange.y);
            pos.z = Mathf.Clamp(pos.z, clampZRange.x, clampZRange.y);
            transform.position = pos;
        }
    }
} 