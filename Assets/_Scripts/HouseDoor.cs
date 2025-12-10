using UnityEngine;
using System.Collections;

public class HouseDoor : MonoBehaviour
{
    [Header("Identification")]
    public int houseId = 0;               // keyId must match this to open

    [Header("Door Pieces")]
    public Transform doorHinge;           // the mesh/pivot we rotate

    [Header("Open Settings")]
    public float openAngle = 90f;
    public float openSpeed = 3f;
    public bool consumeKeyOnOpen = false;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Awake()
    {
        if (doorHinge == null)
        {
            Debug.LogWarning("[HouseDoor] No doorHinge assigned, using this transform.");
            doorHinge = transform;
        }

        closedRotation = doorHinge.localRotation;
        openRotation = Quaternion.Euler(0f, openAngle, 0f) * closedRotation;

        // Make sure THIS collider is a trigger
        Collider c = GetComponent<Collider>();
        if (c != null)
        {
            c.isTrigger = true;
        }
        else
        {
            Debug.LogWarning("[HouseDoor] No collider found on DoorTrigger object.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerKeys keys = other.GetComponent<PlayerKeys>();
        if (keys == null)
            return;

        if (keys.HasKey(houseId))
        {
            Debug.Log("[HouseDoor] Player has key " + houseId + ", opening door.");

            if (consumeKeyOnOpen)
            {
                keys.UseKey(houseId);
            }

            StartCoroutine(OpenDoorRoutine());
        }
        else
        {
            Debug.Log("[HouseDoor] Player does NOT have key " + houseId + ".");
        }
    }

    IEnumerator OpenDoorRoutine()
    {
        isOpen = true;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;
            doorHinge.localRotation = Quaternion.Slerp(closedRotation, openRotation, t);
            yield return null;
        }

        doorHinge.localRotation = openRotation;
        SFXManager.Instance?.PlayDoorOpen();
    }
}