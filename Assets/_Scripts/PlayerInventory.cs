using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Salt")]
    public int saltCount = 0;

    public void AddSalt(int amount)
    {
        saltCount += amount;
        GameLogger.Instance.Log("Salt collected. Total salt = " + saltCount);
        // Later: update UI here.
    }

    [Header("Wood")]
    public int woodCount = 0;

    public void AddWood(int amount)
    {
        woodCount += amount;
        GameLogger.Instance.Log("Wood collected. Total wood = " + woodCount);
        // Later: update UI here.
    }
}