using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;          

    [Header("Orbit")]
    public Vector3 offset = new Vector3(0f, 4f, -6f);
    public float followSmooth = 10f;
    public float yaw;                 // horizontal angle, we are rotating about the y-axis
    public float pitch = 20f;         // vertical angle, we are rotating about the x-axis
    public float minPitch = -10f;
    public float maxPitch = 60f;

    [Header("Look-at")]
    public float targetHeightOffset = 1.5f;

    void Start()
    {
        if (target != null)
        {
            // Initialize yaw from current camera direction
            Vector3 flatDir = transform.position - target.position;
            if (flatDir.sqrMagnitude > 0.001f)
            {
                Quaternion current = Quaternion.LookRotation(flatDir);
                Vector3 e = current.eulerAngles;
                yaw = e.y;
            }
        }
    }

    public void AdjustRotation(float deltaPitch, float deltaYaw)
    {
        yaw += deltaYaw;
        pitch = Mathf.Clamp(pitch - deltaPitch, minPitch, maxPitch);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = target.position + rot * offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSmooth * Time.deltaTime
        );

        Vector3 lookTarget = target.position + Vector3.up * targetHeightOffset;
        transform.LookAt(lookTarget);
    }
}