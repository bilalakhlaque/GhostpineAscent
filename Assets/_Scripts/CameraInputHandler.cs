using UnityEngine;

public class CameraInputHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public float sensitivity = 2f;
    public bool invertY = false;
    public bool invertX = false;

    void Update()
    {
        if (cameraFollow == null) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        if (invertY) mouseY = -mouseY;
        if (invertX) mouseX = -mouseX;

        cameraFollow.AdjustRotation(mouseY, mouseX);
    }
}