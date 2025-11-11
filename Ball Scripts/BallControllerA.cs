using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallControllerA : MonoBehaviour
{
    public float initialLaunchSpeed = 8f;
    public float speedIncreasePerHit = 0.5f;
    // The minimum absolute value the X velocity must maintain
    public float minimumXSpeed = 4f;

    private Rigidbody2D rb;
    private float currentSpeed;

    // NEW: Public property to expose currentSpeed for the magnet script
    public float CurrentSpeed => currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            currentSpeed = initialLaunchSpeed;
            Launch();
        }
    }

    void Launch()
    {
        // Launches the ball towards the enemyAI at a random height
        float cameraHeight = Camera.main.orthographicSize;
        float minY = -cameraHeight + 0.5f;
        float maxY = cameraHeight - 0.5f;

        float randomYDirection = Random.Range(-0.7f, 0.7f);
        Vector2 launchDirection = new Vector2(-1f, randomYDirection);
        launchDirection.Normalize();

        rb.linearVelocity = launchDirection * initialLaunchSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Increase the current speed
        currentSpeed += speedIncreasePerHit;

        // Get the current velocity direction
        Vector2 currentDirection = rb.linearVelocity.normalized;

        // Apply the new, faster velocity initially
        rb.linearVelocity = currentDirection * currentSpeed;

        // --- NEW LOGIC TO ENFORCE MINIMUM X SPEED ---
        EnsureMinimumXSpeed();
    }


    /// Ensures the ball always maintains a minimum horizontal velocity.
    private void EnsureMinimumXSpeed()
    {
        Vector2 currentVelocity = rb.linearVelocity;

        // Check if the absolute X speed is below our minimum threshold
        if (Mathf.Abs(currentVelocity.x) < minimumXSpeed)
        {
            // Determine which direction it was heading (-1 or 1)
            float signX = Mathf.Sign(currentVelocity.x);

            // If it was already moving slightly right/left, keep that direction.
            // If the sign was 0 (perfectly vertical collision, unlikely in pong), assume previous direction 
            if (signX == 0)
            {
                // If the sign somehow ends up 0, force it one way or the other
                signX = (Random.value > 0.5f) ? 1f : -1f;
            }

            // Calculate the new X velocity magnitude
            float newX = minimumXSpeed * signX;

            // Apply the corrected velocity vector. The current Y velocity is maintained.
            rb.linearVelocity = new Vector2(newX, currentVelocity.y);

            Debug.Log("Corrected velocity to meet minimum X speed requirement.");
        }
    }
}

/* using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallControllerA : MonoBehaviour
{
    public float initialLaunchSpeed = 8f;
    public float speedIncreasePerHit = 0.5f;
    // The minimum absolute value the X velocity must maintain
    public float minimumXSpeed = 4f;

    private Rigidbody2D rb;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            currentSpeed = initialLaunchSpeed;
            Launch();
        }
    }

    void Launch()
    {
        // Launches the ball towards the enemyAI at a random height
        float cameraHeight = Camera.main.orthographicSize;
        float minY = -cameraHeight + 0.5f;
        float maxY = cameraHeight - 0.5f;

        float randomYDirection = Random.Range(-0.7f, 0.7f);
        Vector2 launchDirection = new Vector2(-1f, randomYDirection);
        launchDirection.Normalize();

        rb.linearVelocity = launchDirection * initialLaunchSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Increase the current speed
        currentSpeed += speedIncreasePerHit;

        // Get the current velocity direction
        Vector2 currentDirection = rb.linearVelocity.normalized;

        // Apply the new, faster velocity initially
        rb.linearVelocity = currentDirection * currentSpeed;

        // --- NEW LOGIC TO ENFORCE MINIMUM X SPEED ---
        EnsureMinimumXSpeed();
    }


    /// Ensures the ball always maintains a minimum horizontal velocity.
    private void EnsureMinimumXSpeed()
    {
        Vector2 currentVelocity = rb.linearVelocity;

        // Check if the absolute X speed is below our minimum threshold
        if (Mathf.Abs(currentVelocity.x) < minimumXSpeed)
        {
            // Determine which direction it was heading (-1 or 1)
            float signX = Mathf.Sign(currentVelocity.x);

            // If it was already moving slightly right/left, keep that direction.
            // If the sign was 0 (perfectly vertical collision, unlikely in pong), assume previous direction 
            if (signX == 0)
            {
                // If the sign somehow ends up 0, force it one way or the other
                signX = (Random.value > 0.5f) ? 1f : -1f;
            }

            // Calculate the new X velocity magnitude
            float newX = minimumXSpeed * signX;

            // Apply the corrected velocity vector. The current Y velocity is maintained.
            rb.linearVelocity = new Vector2(newX, currentVelocity.y);

            Debug.Log("Corrected velocity to meet minimum X speed requirement.");
        }
    }
} */