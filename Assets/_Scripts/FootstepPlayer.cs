using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepPlayer : MonoBehaviour
{
    [Header("Step Settings")]
    public float stepInterval = 0.4f;   // time between steps while moving
    public float minSpeedForSteps = 0.1f; // threshold so tiny jitter doesn’t play sounds

    private CharacterController controller;
    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogWarning("[FootstepPlayer] No CharacterController found on this GameObject.");
        }
    }

    void Update()
    {
        if (controller == null) return;

        // Use horizontal velocity only (ignore Y so falling doesn’t spam footsteps)
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;
        bool isMoving = speed > minSpeedForSteps;

        if (isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                
                if (SFXManager.Instance != null)
                    SFXManager.Instance.PlayFootstep();

                // For now, just log to confirm timing:
                Debug.Log("[FootstepPlayer] Step");

                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset timer so we have an immediate step when starting to walk again
            stepTimer = 0f;
        }
    }
}