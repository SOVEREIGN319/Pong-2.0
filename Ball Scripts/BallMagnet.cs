using UnityEngine;
using System; // Needed for Func delegate

public class BallMagnet : MonoBehaviour
{
    [Tooltip("The player character's Transform (the target the ball is launched towards).")]
    public Transform playerTarget;

    [Tooltip("The maximum distance from the player the ball can be launched from.")]
    public float pullRange = 20f;

    // Use Rigidbody2D for 2D physics
    private Rigidbody2D rb;
    private const string PlayerTag = "Player";

    // Reference to the Player's cooldown script
    private MagnetCooldown magnetCooldownManager;

    // A delegate to hold the function that retrieves the correct currentSpeed 
    private Func<float> GetCurrentBallSpeed;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("BallMagnet requires a Rigidbody2D component on the same GameObject for 2D physics!");
            return;
        }

        // --- PLAYER FINDING LOGIC & COOLDOWN SETUP ---
        // We find the player to set the target and get the Cooldown script
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindWithTag(PlayerTag);

            if (playerObject != null)
            {
                playerTarget = playerObject.transform;

                // Get the Cooldown script from the player object
                magnetCooldownManager = playerObject.GetComponent<MagnetCooldown>();
                if (magnetCooldownManager == null)
                {
                    Debug.LogError("[BallMagnet] Player object is missing the MagnetCooldown script! Cooldown will not function.");
                }
            }
            else
            {
                Debug.LogError($"[BallMagnet] Player Target not assigned and no GameObject with tag '{PlayerTag}' found in the scene! Magnet will not work.");
            }
        }

        // --- FIND CONTROLLER AND SETUP SPEED DELEGATE ---
        // This links the script to the speed variable in either controller A or B.
        BallControllerA controllerA = GetComponent<BallControllerA>();
        BallControllerB controllerB = GetComponent<BallControllerB>();

        if (controllerA != null)
        {
            GetCurrentBallSpeed = () => controllerA.CurrentSpeed;
        }
        else if (controllerB != null)
        {
            GetCurrentBallSpeed = () => controllerB.CurrentSpeed;
        }
        else
        {
            // Fallback: uses the ball's current linear velocity magnitude if no controller is found
            GetCurrentBallSpeed = () => rb.linearVelocity.magnitude;
        }
    }

    void Update()
    {
        // Check for a single left mouse button press
        if (Input.GetMouseButtonDown(0) && playerTarget != null && rb != null && GetCurrentBallSpeed != null)
        {
            // 1. COOLDOWN CHECK: Only proceed if the cooldown is NOT active
            if (magnetCooldownManager != null && !magnetCooldownManager.CanUseMagnet)
            {
                Debug.Log("[BallMagnet] Magnet is on cooldown! Cannot use yet.");
                return;
            }

            Vector2 ballPosition = transform.position;
            Vector2 targetPosition = playerTarget.position;

            // 2. Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(ballPosition, targetPosition);

            // 3. Only activate if the ball is within the designated pull range
            if (distanceToPlayer <= pullRange)
            {
                // 4. Clear angular velocity (stop spin)
                rb.angularVelocity = 0f;

                // 5. Calculate the direction from the ball to the player
                Vector2 launchDirection = (targetPosition - ballPosition).normalized;

                // 6. Get the required speed from the linked controller script
                float speedToMaintain = GetCurrentBallSpeed.Invoke();

                // 7. SET the velocity using linearVelocity (as requested)
                rb.linearVelocity = launchDirection * speedToMaintain;

                // 8. Start the cooldown on the player's script immediately after use
                if (magnetCooldownManager != null)
                {
                    magnetCooldownManager.StartCooldown();
                }

                Debug.Log($"Magnet activated! Redirecting ball towards player at speed: {speedToMaintain:F2}");
            }
        }
    }
}

/* using UnityEngine;
using System; // Needed for Func delegate

public class BallMagnet : MonoBehaviour
{
    [Tooltip("The player character's Transform (the target the ball is launched towards).")]
    public Transform playerTarget;

    [Tooltip("The maximum distance from the player the ball can be launched from.")]
    public float pullRange = 20f;

    // Use Rigidbody2D for 2D physics
    private Rigidbody2D rb;
    private const string PlayerTag = "Player";

    // A delegate to hold the function that retrieves the correct currentSpeed 
    private Func<float> GetCurrentBallSpeed;

    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("BallMagnet requires a Rigidbody2D component on the same GameObject for 2D physics!");
            return;
        }

        // --- FIND CONTROLLER AND SETUP SPEED DELEGATE ---
        // Check which controller script is attached to the ball (only one should be present)
        BallControllerA controllerA = GetComponent<BallControllerA>();
        BallControllerB controllerB = GetComponent<BallControllerB>();

        if (controllerA != null)
        {
            // Link to the CurrentSpeed property of Controller A
            GetCurrentBallSpeed = () => controllerA.CurrentSpeed;
            Debug.Log("[BallMagnet] Linked to BallControllerA for speed.");
        }
        else if (controllerB != null)
        {
            // Link to the CurrentSpeed property of Controller B
            GetCurrentBallSpeed = () => controllerB.CurrentSpeed;
            Debug.Log("[BallMagnet] Linked to BallControllerB for speed.");
        }
        else
        {
            // Fallback: If no specific controller is found, use the ball's existing velocity magnitude
            Debug.LogError("[BallMagnet] No BallControllerA or B found. Magnet will use the ball's current velocity magnitude as its speed.");
            GetCurrentBallSpeed = () => rb.linearVelocity.magnitude;
        }

        // --- PLAYER FINDING LOGIC ---
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindWithTag(PlayerTag);

            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
                Debug.Log($"[BallMagnet] Automatically found player target with tag '{PlayerTag}'.");
            }
            else
            {
                Debug.LogError($"[BallMagnet] Player Target not assigned and no GameObject with tag '{PlayerTag}' found in the scene! Magnet will not work.");
            }
        }
    }

    void Update()
    {
        // Check for a single left mouse button press
        if (Input.GetMouseButtonDown(0) && playerTarget != null && rb != null && GetCurrentBallSpeed != null)
        {
            Vector2 ballPosition = transform.position;
            Vector2 targetPosition = playerTarget.position;

            // 1. Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(ballPosition, targetPosition);

            // 2. Only activate if the ball is within the designated range
            if (distanceToPlayer <= pullRange)
            {
                // 3. Clear angular velocity (stop spin)
                rb.angularVelocity = 0f;

                // 4. Calculate the direction from the ball to the player (X and Y coordinates)
                Vector2 launchDirection = (targetPosition - ballPosition).normalized;

                // 5. Get the required speed from the linked controller script
                float speedToMaintain = GetCurrentBallSpeed.Invoke();

                // --- VISUAL DEBUGGING ---
                // Draw a line in the Scene view showing the direction of the launch for 1 second
                Debug.DrawRay(ballPosition, launchDirection * 5, Color.red, 1f);
                Debug.Log($"Magnet activated! Redirecting ball towards player at speed: {speedToMaintain:F2}");
                // ------------------------

                // 6. SET the velocity directly. This instantly changes the ball's direction 
                // but keeps its speed (magnitude) consistent with the CurrentSpeed variable.
                rb.linearVelocity = launchDirection * speedToMaintain;
            }
        }
    }
} */