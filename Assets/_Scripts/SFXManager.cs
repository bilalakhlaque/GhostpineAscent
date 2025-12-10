using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [Header("Source")]
    public AudioSource sfxSource;

    [Header("Pickup Clips")]
    public AudioClip pickupPotionClip;
    [Range(0f, 1f)] public float pickupPotionVolume = 0.8f;

    public AudioClip pickupResourceClip;
    [Range(0f, 1f)] public float pickupResourceVolume = 0.8f;

    [Header("World / Interaction Clips")]
    public AudioClip doorOpenClip;
    [Range(0f, 1f)] public float doorOpenVolume = 0.8f;

    public AudioClip enemyDeathClip;
    [Range(0f, 1f)] public float enemyDeathVolume = 0.9f;

    public AudioClip playerHurtClip;
    [Range(0f, 1f)] public float playerHurtVolume = 0.9f;

    public AudioClip playerDeathClip;
    [Range(0f, 1f)] public float playerDeathVolume = 1f;

    public AudioClip playerWinClip;
    [Range(0f, 1f)] public float playerWinVolume = 1f;

    public AudioClip footstepClip;
    [Range(0f, 1f)] public float footstepVolume = 0.4f;

    public AudioClip dropSaltClip;
    [Range(0f, 1f)] public float dropSaltVolume = 0.7f;

    public AudioClip enemyAttackClip;      // witch cast, ghoul swipe
    [Range(0f, 1f)] public float enemyAttackVolume = 0.8f;

    public AudioClip ghostEatOrbClip;
    [Range(0f, 1f)] public float ghostEatOrbVolume = 0.8f;
    
    [Header("Melee / Tools")]
    public AudioClip swordSwingClip;
    [Range(0f, 1f)] public float swordSwingVolume = 0.8f;

    public AudioClip pickaxeSwingClip;
    [Range(0f, 1f)] public float pickaxeSwingVolume = 0.8f;
    
    [Header("UI")]
    public AudioClip uiButtonClickClip;
    [Range(0f, 1f)] public float uiButtonClickVolume = 0.7f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Generic
    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }
    }

    // Convenience wrappers so your game code stays clean:

    public void PlayPickupPotion() =>
        PlayOneShot(pickupPotionClip, pickupPotionVolume);

    public void PlayPickupResource() =>
        PlayOneShot(pickupResourceClip, pickupResourceVolume);

    public void PlayDoorOpen() =>
        PlayOneShot(doorOpenClip, doorOpenVolume);

    public void PlayEnemyDeath() =>
        PlayOneShot(enemyDeathClip, enemyDeathVolume);

    public void PlayPlayerHurt() =>
        PlayOneShot(playerHurtClip, playerHurtVolume);

    public void PlayPlayerDeath() =>
        PlayOneShot(playerDeathClip, playerDeathVolume);

    public void PlayPlayerWin() =>
        PlayOneShot(playerWinClip, playerWinVolume);

    public void PlayFootstep() =>
        PlayOneShot(footstepClip, footstepVolume);

    public void PlayDropSalt() =>
        PlayOneShot(dropSaltClip, dropSaltVolume);

    public void PlayEnemyAttack() =>
        PlayOneShot(enemyAttackClip, enemyAttackVolume);

    public void PlayGhostEatOrb() =>
        PlayOneShot(ghostEatOrbClip, ghostEatOrbVolume);
    
    public void PlaySwordSwing() =>
        PlayOneShot(swordSwingClip, swordSwingVolume);

    public void PlayPickaxeSwing() =>
        PlayOneShot(pickaxeSwingClip, pickaxeSwingVolume);

    public void PlayUIButtonClick() =>
        PlayOneShot(uiButtonClickClip, uiButtonClickVolume);

    public void StopAllSFX()
    {
        if (sfxSource != null)
            sfxSource.Stop();
    }
}