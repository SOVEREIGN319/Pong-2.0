using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallSoundFX : MonoBehaviour
{
    [Tooltip("A list of sound clips to choose from when the ball collides.")]
    public AudioClip[] HitSoundClips;

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to the ball
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("BallSoundFX requires an AudioSource component on the same GameObject!");
        }
    }

    /// Called by Unity every time the ball's 2D collider touches another 2D collider.

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Safety check: Ensure the AudioSource is valid and the array has clips
        if (audioSource != null && HitSoundClips.Length > 0)
        {
            // Select a random index from the array
            int randomIndex = Random.Range(0, HitSoundClips.Length);

            // Get the random clip
            AudioClip clipToPlay = HitSoundClips[randomIndex];

            // Play the selected sound clip once
            audioSource.PlayOneShot(clipToPlay);
        }
        else if (HitSoundClips.Length == 0)
        {
            Debug.LogWarning("HitSoundClips array is empty! Please assign audio clips in the Inspector.");
        }
    }

}
