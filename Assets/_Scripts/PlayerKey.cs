using UnityEngine;
using System.Collections.Generic;

public class PlayerKeys : MonoBehaviour
{
    [Header("Keys Owned (for debug)")]
    public List<int> ownedKeyIds = new List<int>();

    private HashSet<int> keySet = new HashSet<int>();

    public void AddKey(int keyId)
    {
        if (!keySet.Contains(keyId))
        {
            keySet.Add(keyId);
            ownedKeyIds.Add(keyId);
            GameLogger.Instance.Log("[PlayerKeys] Picked up key " + keyId);
        }
    }

    public bool HasKey(int keyId)
    {
        return keySet.Contains(keyId);
    }

    // Optional: if you want keys to be consumed when used
    public void UseKey(int keyId)
    {
        if (keySet.Remove(keyId))
        {
            ownedKeyIds.Remove(keyId);
            GameLogger.Instance.Log("[PlayerKeys] Used key " + keyId);
        }
    }
}