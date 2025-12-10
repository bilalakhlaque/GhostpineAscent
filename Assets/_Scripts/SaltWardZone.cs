using UnityEngine;

public class SaltWardZone : MonoBehaviour
{
    [Header("Lifetime")]
    public float lifetime = 8f;  // how long the salt stays in the world

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If a ghost touches the salt, it is DISPELLED
        SpectralGhostAI ghost = other.GetComponent<SpectralGhostAI>();
        if (ghost != null)
        {
            GameLogger.Instance.Log("[SaltWardZone] Ghost touched salt and was dispelled.");
            Destroy(ghost.gameObject);
            Destroy(gameObject); // consume the salt ward
        }
    }
}