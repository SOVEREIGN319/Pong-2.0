using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for CanvasGroup
using System.Collections; // Required for Coroutines

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name of the scene to load.")]
    [SerializeField] private string sceneName = "VerticalPong";

    [Header("Audio Settings")]
    [Tooltip("Audio Source component playing the continuous background menu music.")]
    public AudioSource MenuMusicSource;
    [Tooltip("Audio Source component to play the transition sound (e.g., a theme cue).")]
    public AudioSource TransitionMusicSource;
    [Tooltip("Rate at which audio fades in/out (e.g., 0.5 is a medium fade).")]
    public float AudioFadeRate = 0.5f;

    [Header("Visual Settings")]
    [Tooltip("Canvas Group on the black screen panel for visual fading.")]
    public CanvasGroup FadePanelCanvasGroup;
    [Tooltip("Rate at which the screen fades to black (higher is faster).")]
    public float VisualFadeRate = 1.0f;

    // Time to wait before loading the scene after max volume/alpha is reached
    private float loadDelay = 0.5f;

    void Start()
    {
        // Safety check to ensure the continuous music is playing at full volume at the start
        if (MenuMusicSource != null)
        {
            MenuMusicSource.volume = 0.02f;
            if (!MenuMusicSource.isPlaying)
            {
                // Assuming this scene handles background music, start it if it's not playing
                MenuMusicSource.Play();
            }
        }
    }

    // This method is called by the UI Button's OnClick() event
    public void StartTransition()
    {
        if (FadePanelCanvasGroup == null)
        {
            Debug.LogError("Fade Panel Canvas Group is not assigned! Cannot fade.");
            SceneManager.LoadScene(sceneName); // Fallback to immediate load
            return;
        }

        // 1. Start playing the transition music immediately (if assigned)
        if (TransitionMusicSource != null && TransitionMusicSource.clip != null)
        {
            TransitionMusicSource.Play();
        }

        // 2. Start the combined fade coroutine
        StartCoroutine(FadeOutAndLoad());
    }

    private IEnumerator FadeOutAndLoad()
    {
        float timer = 0f;

        // --- VISUAL AND AUDIO FADE OUT LOOP ---
        while (timer < 1f)
        {
            timer += Time.deltaTime * VisualFadeRate;

            // FADE SCREEN TO BLACK (Alpha from 0 to 1)
            FadePanelCanvasGroup.alpha = timer;

            // FADE MENU MUSIC OUT (Volume from 1 to 0)
            if (MenuMusicSource != null)
            {
                MenuMusicSource.volume = Mathf.Lerp(0.02f, 0f, timer * AudioFadeRate);
            }

            // FADE TRANSITION MUSIC IN (Volume from 0 to 1)
            if (TransitionMusicSource != null)
            {
                // Only start fading transition music in after a brief delay or based on a separate logic
                // For simplicity, we'll cross-fade aggressively here:
                TransitionMusicSource.volume = Mathf.Lerp(0f, 0.02f, timer * AudioFadeRate);
            }

            yield return null;
        }

        // Ensure everything is set to its final state
        FadePanelCanvasGroup.alpha = 1f;
        if (MenuMusicSource != null) MenuMusicSource.volume = 0f;

        // Wait a small moment on the black screen for effect
        yield return new WaitForSeconds(loadDelay);

        // --- LOAD NEW SCENE ---
        Debug.Log("Transition complete. Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}