using UnityEngine;

public class EnemyControllerA : MonoBehaviour
{
    // Speed at which the paddle moves towards the target Y position (higher = faster response)
    public float followSpeed = 5f;

    // How snappy the movement is (lower = smoother/slower to reach target)
    public float smoothness = 0.1f;

    // Reference to the ball's transform
    private Transform ballTransform;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Start Continuous Search Logic

        // In case there is currently no reference to the ball (If it was destroyed)
        if (ballTransform == null)
        {
            // Find a GameObject with the "Ball" tag
            GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");

            // Assign the Ball's transform to variable
            if (ballObject != null)
            {
                ballTransform = ballObject.transform;
            }

            // If there is still no reference to the ball (If it hasn't respawned yet), 
            if (ballTransform == null)
            {
                return;
            }
        }
        // End Continuous Search Logic


        // Movement Logic (runs only when ballTransform is not null)

        // Determine the target Y position
        float targetY = ballTransform.position.y;

        // Smoothly calculate the next Y position using Mathf.Lerp
        float newY = Mathf.Lerp(rb.position.y, targetY, smoothness);

        // Create the new position vector
        Vector2 newPosition = new Vector2(rb.position.x, newY);

        // Use Rigidbody.MovePosition for smooth physics movement and collision handling
        rb.MovePosition(newPosition);
    }

}
