using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneSwapManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text playTimerText;
    public Text countdownText;

    [Header("Swap Settings")]
    [Tooltip("How long the countdown lasts before swapping scenes.")]
    public float countdownDuration = 3f;

    [Tooltip("Minimum time between swaps (in seconds).")]
    public float minSwapInterval = 10f;

    [Tooltip("Maximum time between swaps (in seconds).")]
    public float maxSwapInterval = 15f;

    [Tooltip("Name of the horizontal scene.")]
    public string horizontalScene = "HorizontalPong";

    [Tooltip("Name of the vertical scene.")]
    public string verticalScene = "VerticalPong";

    private float playTime;
    private float nextSwapTime;
    private bool countdownActive;

    private static SceneSwapManager instance;
    private string currentScene;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        ResetNextSwapTime();

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        playTime += Time.deltaTime;

        if (playTimerText != null)
            playTimerText.text = $"{FormatTime(playTime)}";

        if (!countdownActive && Time.timeSinceLevelLoad >= nextSwapTime)
        {
            StartCoroutine(StartCountdown());
        }
    }

    private IEnumerator StartCountdown()
    {
        countdownActive = true;

        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        float countdown = countdownDuration;

        while (countdown > 0f)
        {
            if (countdownText != null)
                countdownText.text = $"Swapping in: {Mathf.Ceil(countdown)}s";
            countdown -= Time.deltaTime;
            yield return null;
        }

        if (countdownText != null)
            countdownText.text = "Swapping...";

        yield return new WaitForSeconds(0.3f); // short delay to allow final frame update

        SwapScene();
    }

    private void SwapScene()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // Make sure scene names match exactly what’s in Build Settings
        string nextScene = (currentScene == horizontalScene) ? verticalScene : horizontalScene;

        Debug.Log($"[SceneSwapManager] Swapping from {currentScene} to {nextScene}");

        // If for any reason the next scene isn't valid, print a warning
        if (Application.CanStreamedLevelBeLoaded(nextScene))
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.LogError($"Scene '{nextScene}' not found in Build Settings! Check the name.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.name;
        ResetNextSwapTime();

        countdownActive = false;
        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        // Re-link UI elements in new scene (if they exist)
        if (playTimerText == null)
            playTimerText = GameObject.Find("PlayTimerText")?.GetComponent<Text>();
        if (countdownText == null)
            countdownText = GameObject.Find("CountdownText")?.GetComponent<Text>();
    }

    private void ResetNextSwapTime()
    {
        nextSwapTime = Time.timeSinceLevelLoad + Random.Range(minSwapInterval, maxSwapInterval);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}