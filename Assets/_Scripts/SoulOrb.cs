/*
using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    [Header("Counts toward lose condition")]
    public bool countsForLose = true;

    void Awake()
    {
        if (countsForLose && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterSoulOrb();
        }
    }

    // Call this when a ghost successfully eats/collects the orb
    public void ConsumeByGhost()
    {
        if (countsForLose && GameManager.Instance != null)
        {
            GameManager.Instance.OnSoulOrbDestroyedByGhost();
        }

        Destroy(gameObject);
    }
} 
*/

using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    [Header("Counts toward lose condition")]
    public bool countsForLose = true;

    void Awake()
    {
        if (countsForLose && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterSoulOrb();
        }
    }

    public void ConsumeByGhost()
    {
        if (countsForLose && GameManager.Instance != null)
        {
            GameManager.Instance.OnSoulOrbDestroyedByGhost();
        }

        Destroy(gameObject);
    }
}