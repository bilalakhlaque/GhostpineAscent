using UnityEngine;

public class PlayerToolUser : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Transform toolOrigin;        // where the ray should start (child at chest/hand)
    public float toolRange = 3.5f;
    public float weaponRange = 3.5f;

    [Header("Layer Masks")]
    // Resource layers (salt blocks, later trees/metal)
    public LayerMask resourceMask;
    public LayerMask obstacleMask;
    // Enemy layers (ghoul, ghost)
    public LayerMask enemyMask;

    [Header("Weapon Settings")]
    public float weaponDamage = 1f;

    [Header("Debug")]
    public bool drawDebugRay = true;

    public enum HotbarSlot
    {
        Tool,
        Weapon,
        Potion
    }
    
    [Header("Hotbar State")]
    public HotbarSlot activeSlot = HotbarSlot.Tool;

    [Tooltip("If false, potion slot will never do anything.")]
    public bool hasPotionUse = false;

    private PlayerEquipment equipment;

    void Awake()
    {
        equipment = GetComponent<PlayerEquipment>();
        if (equipment == null)
        {
            Debug.LogWarning("[PlayerToolUser] No PlayerEquipment found on this GameObject. Equip logic will be ignored.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (equipment == null)
            {
                // Fallback: default to tool behavior
                Debug.Log("[PlayerToolUser] No equipment, using tool hit by default.");
                TryToolHit();
                return;
            }

            if (equipment.IsToolEquipped)
            {
                GameLogger.Instance.Log("[PlayerToolUser] Left click → Tool hit.");
                if (SFXManager.Instance != null)
                {
                    SFXManager.Instance.PlayPickaxeSwing();
                }
                TryToolHit();
            }
            else if (equipment.IsWeaponEquipped)
            {
                GameLogger.Instance.Log("[PlayerToolUser] Left click → Weapon hit.");
                if (SFXManager.Instance != null)
                {
                    SFXManager.Instance.PlaySwordSwing();
                }
                TryWeaponHit();
            }
            else
            {
                Debug.Log("[PlayerToolUser] Left click ignored: Potion slot or no valid item.");
            }
        }
    }

    // ---------- Shared ray helper ----------

    Ray BuildRay()
    {
        if (toolOrigin == null)
        {
            Debug.LogWarning("[PlayerToolUser] No toolOrigin assigned! Using player position.");
            return new Ray(transform.position + Vector3.up * 1.0f, transform.forward);
        }

        return new Ray(toolOrigin.position, transform.forward);
    }

    // ---------- Tool (resource) hit ----------

    void TryToolHit()
    {
        Ray ray = BuildRay();

        if (drawDebugRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * toolRange, Color.cyan, 0.5f);
        }

        RaycastHit hit;
        bool hitSomething;

        // If no mask set, hit everything
        if (resourceMask.value == 0)
        {
            hitSomething = Physics.Raycast(ray, out hit, toolRange);
        }
        else
        {
            hitSomething = Physics.Raycast(ray, out hit, toolRange, resourceMask);
        }

        if (!hitSomething)
        {
            Debug.Log("[PlayerToolUser] Tool ray hit NOTHING.");
            return;
        }

        Debug.Log("[PlayerToolUser] Tool ray hit: " + hit.collider.name + " on layer " + LayerMask.LayerToName(hit.collider.gameObject.layer));

        // Salt block
        SaltBlock saltBlock = hit.collider.GetComponent<SaltBlock>();
        if (saltBlock != null)
        {
            GameLogger.Instance.Log("[PlayerToolUser] SaltBlock found, applying hit.");
            saltBlock.TakePickaxeHit();
            return;
        }

        Debug.Log("[PlayerToolUser] Tool hit something, but it was not a SaltBlock.");

        // Wood block
        WoodBlock woodBlock = hit.collider.GetComponent<WoodBlock>();
        if (woodBlock != null)
        {
            GameLogger.Instance.Log("[PlayerToolUser] WoodBlock found, applying hit.");
            woodBlock.TakePickaxeHit();
            return;
        }

        Debug.Log("[PlayerToolUser] Tool hit something, but it was not a WoodBlock.");
        
    }

    // ---------- Weapon (enemy) hit ----------

    void TryWeaponHit()
    {
        Ray ray = BuildRay();

        if (drawDebugRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * weaponRange, Color.red, 0.5f);
        }

        RaycastHit hit;
        bool hitSomething;

        if (enemyMask.value == 0)
        {
            hitSomething = Physics.Raycast(ray, out hit, weaponRange);
        }
        else
        {
            hitSomething = Physics.Raycast(ray, out hit, weaponRange, enemyMask);
        }

        if (!hitSomething)
        {
            Debug.Log("[PlayerToolUser] Weapon ray hit NOTHING.");
            return;
        }

        GameLogger.Instance.Log("[PlayerToolUser] Weapon ray hit: " + hit.collider.name + " on layer " + LayerMask.LayerToName(hit.collider.gameObject.layer));

        // Is it an enemy?
        EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            GameLogger.Instance.Log("[PlayerToolUser] EnemyHealth found, dealing damage.");
            enemy.TakeDamage(weaponDamage);
            return;
        }

        Debug.Log("[PlayerToolUser] Weapon hit something, but it was not an EnemyHealth.");
    }
}