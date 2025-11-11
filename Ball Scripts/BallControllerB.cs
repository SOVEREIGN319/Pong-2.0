
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallControllerB : MonoBehaviour
{
    public float initialLaunchSpeed = 8f;
    public float speedIncreasePerHit = 0.5f;
    // The minimum absolute value the Y velocity must maintain
    public float minimumYSpeed = 4f;

    private Rigidbody2D rb;
    private float currentSpeed;

    // Public property to expose currentSpeed for the magnet script
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
        float cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float minX = -cameraWidth + 0.5f;
        float maxX = cameraWidth - 0.5f;

        float randomXDirection = Random.Range(-1f, 1f);
        Vector2 launchDirection = new Vector2(randomXDirection, 1f); // launches vertically instead of horizontally
        launchDirection.Normalize();

        rb.linearVelocity = launchDirection * initialLaunchSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Increase the current speed
        currentSpeed += speedIncreasePerHit;

        // Get the current velocity direction
        Vector2 currentDirection = rb.linearVelocity.normalized;

        // Apply the new, faster velocity
        rb.linearVelocity = currentDirection * currentSpeed;

        // Enforce minimum vertical speed
        EnsureMinimumYSpeed();
    }


    /// Ensures the ball always maintains a minimum vertical velocity.
    private void EnsureMinimumYSpeed()
    {
        Vector2 currentVelocity = rb.linearVelocity;

        // Check if the absolute Y speed is below our minimum threshold
        if (Mathf.Abs(currentVelocity.y) < minimumYSpeed)
        {
            // Determine which direction it was heading (-1 or 1)
            float signY = Mathf.Sign(currentVelocity.y);

            // Handle zero case (unlikely but possible)
            if (signY == 0)
                signY = (Random.value > 0.5f) ? 1f : -1f;

            // Calculate the new Y velocity magnitude
            float newY = minimumYSpeed * signY;

            // Apply the corrected velocity vector (keep X the same)
            rb.linearVelocity = new Vector2(currentVelocity.x, newY);

            Debug.Log("Corrected velocity to meet minimum Y speed requirement.");
        }
    }
}
