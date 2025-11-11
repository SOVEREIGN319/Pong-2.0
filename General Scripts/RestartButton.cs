using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    [Tooltip("The name of the scene to load.")]
    [SerializeField] private string sceneName = "VerticalPong";
    private AudioSource audioSource;
    public AudioClip soundToPlay;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("RestartButton requires an AudioSource component on the same GameObject!");
        }
    }

    // This method is called by the UI Button's OnClick() event
    public void StartTransition()
    {
        // Play Audio (if set up)
        if (soundToPlay != null && audioSource != null)
        {
            audioSource.PlayOneShot(soundToPlay);
        }

        //  Reset scores before loading scene
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScoresAndState();
        }
        else
        {
            // This usually means the ScoreManager was destroyed or isn't running
            Debug.LogError("ScoreManager.Instance not found! Scores may not reset correctly.");
        }

        // Load the Game Scene
        Debug.Log("Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

}

