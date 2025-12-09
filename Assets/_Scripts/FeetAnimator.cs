using UnityEngine;

public class FeetAnimator : MonoBehaviour
{
    [Header("Walking Animation")]
    public float stepSpeed = 15f;   // How fast they swing
    public float stepAngle = 20f;   // How far they swing (degrees)
    
    private Quaternion originalRotation;

    void Start()
    {
        // Remember the starting rotation so we can reset to it
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // 1. Check if we are pressing WASD
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // If moving (input is not zero)
        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            // Calculate a swinging angle using Sine wave based on time
            float swing = Mathf.Sin(Time.time * stepSpeed) * stepAngle;

            // Apply rotation to the X axis only
            transform.localRotation = Quaternion.Euler(swing, 0, 0);
        }
        else
        {
            // 2. Not moving? Return smoothly to original rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, Time.deltaTime * 10f);
        }
    }
}