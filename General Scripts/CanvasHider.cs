using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHider : MonoBehaviour
{
    [Tooltip("List of scene names where this UI element should be HIDDEN.")]
    public string[] scenesToHideIn;

    private Canvas canvas;

    void Awake()
    {
        // Get the Canvas component or CanvasGroup, as this is typically attached to the Canvas root
        canvas = GetComponent<Canvas>();

        if (canvas == null)
        {
            Debug.LogWarning("SceneHider requires a Canvas component on this GameObject to function.");
            return;
        }

        // Immediately check the current scene when the object loads
        CheckCurrentScene(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += CheckCurrentScene;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= CheckCurrentScene;
    }

    /// Checks if the scene is in the 'hide' list and toggles the canvas.
    void CheckCurrentScene(Scene scene, LoadSceneMode mode)
    {
        bool shouldHide = false;
        string currentSceneName = scene.name;

        // Iterate through the list of scenes where the UI should be hidden
        foreach (string hideScene in scenesToHideIn)
        {
            if (currentSceneName.Equals(hideScene, System.StringComparison.OrdinalIgnoreCase))
            {
                shouldHide = true;
                break;
            }
        }

        // Toggle the Canvas component's enabled state
        canvas.enabled = !shouldHide;

        Debug.Log($"UI visibility for Canvas in scene '{currentSceneName}': {(shouldHide ? "HIDDEN" : "SHOWN")}");
    }
}