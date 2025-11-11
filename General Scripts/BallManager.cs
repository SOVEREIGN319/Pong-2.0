using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BallManager : MonoBehaviour
{
    // Prefabs for different scenes
    [Tooltip("The Ball Prefab to use when the scene is 'HorizontalPong'.")]
    public GameObject horizontalBallPrefab;

    [Tooltip("The Ball Prefab to use when the scene is 'VerticalPong'.")]
    public GameObject verticalBallPrefab;

    // Scene Name Constants
    private const string HorizontalSceneName = "HorizontalPong";
    private const string VerticalSceneName = "VerticalPong";
    
    // -----------------------------------------------------------------------------------------

    [Tooltip("The point where the ball should spawn (set this in the Inspector).")]
    public Transform spawnPoint;

    [Tooltip("Time to wait before respawning after destruction.")]
    public float respawnDelay = 1f;

    void Start()
    {
        // Instantiate the first ball when the game begins
        SpawnNewBall();
    }


    // Make sure to call this public function from another script 
    public void HandleBallDestroyed()
    {
        // Start the coroutine to wait and respawn the ball
        StartCoroutine(RespawnBallRoutine());
    }


    /// Coroutine to handle the delay before respawning.
    IEnumerator RespawnBallRoutine()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(respawnDelay);

        // Once the delay is over, spawn a new ball
        SpawnNewBall();
    }


    /// Instantiates a new ball at the spawn point based on the active scene.
    void SpawnNewBall()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Missing Spawn Point reference in BallManager! Please assign one in the Inspector.");
            return;
        }

        // Get the name of the current active scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        GameObject ballToSpawn = null;

        // Check the current scene name to select the correct prefab
        if (currentSceneName == HorizontalSceneName)
        {
            ballToSpawn = horizontalBallPrefab;
        }
        else if (currentSceneName == VerticalSceneName)
        {
            ballToSpawn = verticalBallPrefab;
        }
        else
        {
            // In case the scene name doesn't match
            Debug.LogWarning($"Current scene '{currentSceneName}' does not match the defined scene names. Spawning horizontal ball as a default.");
            ballToSpawn = horizontalBallPrefab;
        }

        // Final check and instantiation
        if (ballToSpawn != null)
        {
            // Instantiate the prefab at the spawn point's position and rotation
            Instantiate(ballToSpawn, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"New ball spawned: {ballToSpawn.name} in scene: {currentSceneName}");
        }
        else
        {
            Debug.LogError("The required Ball Prefab is missing! Please assign both Horizontal and Vertical Ball Prefabs in the Inspector.");
        }
    }

}
