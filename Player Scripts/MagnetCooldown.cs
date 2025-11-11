using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MagnetCooldown : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The duration (in seconds) the magnet ability is unavailable after use.")]
    public float CooldownDuration = 3f;

    [Tooltip("The sound that plays when the magnet ability becomes ready to use.")]
    public AudioClip ReadySound;

    // Time when the cooldown will finish
    private float nextAvailableTime;
    private AudioSource audioSource;
    private bool isReady = true;

    /// Public property for other scripts (like BallMagnet) to check if the ability is ready.
    public bool CanUseMagnet => isReady;

    void Start()
    {
        // Get the AudioSource component, which is required by this script
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("MagnetCooldown requires an AudioSource component on the Player!");
        }

        // Initialize the ability as ready
        nextAvailableTime = 0f;
    }

    void Update()
    {
        // If the ability is currently NOT ready and the cooldown time has passed
        if (!isReady && Time.time >= nextAvailableTime)
        {
            isReady = true;
            Debug.Log("[MagnetCooldown] Magnet is READY!");

            // Play the ready sound effect
            if (audioSource != null && ReadySound != null)
            {
                audioSource.PlayOneShot(ReadySound);
            }
        }
    }

    /// Public method called by the BallMagnet script when the magnet is successfully used.
    public void StartCooldown()
    {
        if (isReady)
        {
            isReady = false;
            nextAvailableTime = Time.time + CooldownDuration;
            Debug.Log($"[MagnetCooldown] Magnet used! Cooldown started for {CooldownDuration} seconds.");
        }
    }
}