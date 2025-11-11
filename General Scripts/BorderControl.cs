using UnityEngine;
using UnityEngine.Audio;

public class BorderControl : MonoBehaviour
{
    private AudioSource audioSource;
    private BallManager ballManager;

    // Filter what gets destroyed by Tag
    public string tagToDestroy = "Ball";
    public AudioClip soundToPlay;

    void Start()
    {
        // Find the single instance of the BallManager script in the scene
        ballManager = Object.FindFirstObjectByType<BallManager>();

        if (ballManager == null)
        {
            Debug.LogError("BallManager not found in the scene! Please add one.");
        }

        // Get the AudioSource component once when the object starts
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("The AudioSource component is missing from this GameObject!");
        }

    }


    // This function is called when Is Trigger is checked
    void OnTriggerEnter2D(Collider2D other)
    {
        HandleDestruction(other.gameObject);
    }

    // This function is called when Is Trigger is unchecked (physical collision)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Access the GameObject from the collision information
        HandleDestruction(collision.gameObject);
    }

    /// Checks the tag and destroys the incoming object.
    /// <param name="objectToDestroy">The GameObject that made contact.</param>
    private void HandleDestruction(GameObject objectToDestroy)
    {
        // Check if a specific tag is required AND the tags don't match
        if (!string.IsNullOrEmpty(tagToDestroy) && !objectToDestroy.CompareTag(tagToDestroy))
        {
            // If the tags don't match, exit the function without destroying anything
            return;
        }

        // If the object matches the criteria, destroy it
        Debug.Log("Wall destroyed object: " + objectToDestroy.name);
        Destroy(objectToDestroy);

        if (soundToPlay != null)
        {
            // This plays the sound clip once and doesn't interrupt other sounds
            audioSource.PlayOneShot(soundToPlay);
        }


        if (ballManager != null)
        {
            ballManager.HandleBallDestroyed();
        }

    }
}

