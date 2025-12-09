using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Player References")]
    public PlayerStats playerStats;
    public PlayerInventory playerInventory;
    public PlayerEquipment playerEquipment;

    [Header("UI Text")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI sanityText;
    public TextMeshProUGUI saltText;
    public TextMeshProUGUI slotText;

    void Awake()
    {
        // If not assigned manually, try to find player by tag
        if (playerStats == null || playerInventory == null || playerEquipment == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (playerStats == null)
                    playerStats = player.GetComponent<PlayerStats>();
                if (playerInventory == null)
                    playerInventory = player.GetComponent<PlayerInventory>();
                if (playerEquipment == null)
                    playerEquipment = player.GetComponent<PlayerEquipment>();
            }
        }
    }

    void Update()
    {
        UpdateHealthUI();
        UpdateSanityUI();
        UpdateSaltUI();
        UpdateSlotUI();
    }

    void UpdateHealthUI()
    {
        if (healthText == null || playerStats == null) return;

        healthText.text = $"HP: {playerStats.currentHealth:0}/{playerStats.maxHealth:0}";
    }

    void UpdateSanityUI()
    {
        if (sanityText == null || playerStats == null) return;

        sanityText.text = $"Sanity: {playerStats.currentSanity:0}/{playerStats.maxSanity:0}";
    }

    void UpdateSaltUI()
    {
        if (saltText == null || playerInventory == null) return;

        saltText.text = $"Salt: {playerInventory.saltCount}";
    }

    void UpdateSlotUI()
    {
        if (slotText == null || playerEquipment == null) return;

        string slotName = playerEquipment.currentSlot switch
        {
            PlayerEquipment.EquipSlot.Tool   => "Tool",
            PlayerEquipment.EquipSlot.Weapon => "Weapon",
            PlayerEquipment.EquipSlot.Potion => "Potion",
            _                                => playerEquipment.currentSlot.ToString()
        };

        slotText.text = $"Slot: {slotName}";
    }
}