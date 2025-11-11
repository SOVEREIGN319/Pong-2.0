using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Game Settings (Win/Loss)")]
    public int maxScore = 7;
    [Tooltip("Time limit in minutes (5 = 5 minutes)")]
    public float maxTimeMinutes = 5f;
    [SerializeField] private string winSceneName = "WinScene";
    [SerializeField] private string lossSceneName = "LossScene";

    [Header("UI Elements")]
    public Text playerScoreText;
    public Text aiScoreText;

    [Header("Scores")]
    public int playerScore = 0;
    public int aiScore = 0;

    private float gameTimerSeconds = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        // CRUCIAL FIX: Ensure time is always running when this script starts or reloads a scene.
        Time.timeScale = 1f;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ⭐ NEW METHOD: Public function to reset all game state variables
    public void ResetScoresAndState()
    {
        playerScore = 0;
        aiScore = 0;
        gameTimerSeconds = 0f;
        isGameOver = false;
        UpdateUI();
        Debug.Log("Scores and game state reset successfully.");
    }

    void Update()
    {
        if (isGameOver)
            return;

        // 1. Update Timer
        gameTimerSeconds += Time.deltaTime;

        // 2. Check for time limit (5 minutes * 60 seconds)
        if (gameTimerSeconds >= maxTimeMinutes * 60f)
        {
            CheckGameEnd(true);
        }
    }

    public void AddPlayerScore(int amount = 1)
    {
        if (isGameOver) return;

        playerScore += amount;
        UpdateUI();
        CheckGameEnd(false);
    }

    public void AddAIScore(int amount = 1)
    {
        if (isGameOver) return;

        aiScore += amount;
        UpdateUI();
        CheckGameEnd(false);
    }

    private void UpdateUI()
    {
        if (playerScoreText != null)
            playerScoreText.text = $"{playerScore}";

        if (aiScoreText != null)
            aiScoreText.text = $"{aiScore}";
    }

    /// <summary>
    /// Checks if the game-ending conditions have been met and loads the next scene.
    /// </summary>
    private void CheckGameEnd(bool isTimeOut)
    {
        if (isGameOver)
            return;

        string sceneToLoad = "";

        // --- WIN/LOSS LOGIC --- (Omitted for brevity, but stays here)
        if (playerScore >= maxScore)
        {
            sceneToLoad = winSceneName;
        }
        else if (aiScore >= maxScore)
        {
            sceneToLoad = lossSceneName;
        }
        else if (isTimeOut)
        {
            sceneToLoad = lossSceneName;
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            isGameOver = true;
            Time.timeScale = 0f;
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }
}