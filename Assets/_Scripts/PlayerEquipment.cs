using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public enum EquipSlot
    {
        Tool = 0,
        Weapon = 1,
        Potion = 2
    }

    [Header("Current Equipment")]
    public EquipSlot currentSlot = EquipSlot.Tool;

    // For now, assume we always have a pickaxe.
    // Later we can make these toggled by pickups.
    [Header("Owned Items")]
    public bool hasPickaxe = true;
    public bool hasSword = false;
    public bool hasPotionUse = false;

    void Update()
    {
        // Number keys to switch slots
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSlot(EquipSlot.Tool);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSlot(EquipSlot.Weapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSlot(EquipSlot.Potion);
        }
    }

    void SetSlot(EquipSlot slot)
    {
        currentSlot = slot;
        GameLogger.Instance.Log($"[PlayerEquipment] Selected slot: {currentSlot}");
        // Later: update UI highlight here.
    }

    // Helper properties the rest of the code can ask
    public bool IsToolEquipped => currentSlot == EquipSlot.Tool && hasPickaxe;
    public bool IsWeaponEquipped => currentSlot == EquipSlot.Weapon && hasSword;
    public bool IsPotionEquipped => currentSlot == EquipSlot.Potion && hasPotionUse;
}