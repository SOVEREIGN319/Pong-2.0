using UnityEngine;

public class EnemyControllerB : MonoBehaviour
{
    public float followSpeed = 5f;
    public float smoothness = 0.1f;

    private Transform ballTransform;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (ballTransform == null)
        {
            GameObject ballObject = GameObject.FindGameObjectWithTag("Ball");
            if (ballObject != null)
                ballTransform = ballObject.transform;

            if (ballTransform == null)
                return;
        }

        // --- Horizontal movement logic ---

        // 1. Get target X (follow the ball horizontally)
        float targetX = ballTransform.position.x;

        // 2. Smoothly interpolate toward target X
        float newX = Mathf.Lerp(rb.position.x, targetX, smoothness * followSpeed * Time.fixedDeltaTime);

        // 3. Keep Y fixed, move only along X
        Vector2 newPosition = new Vector2(newX, rb.position.y);

        // 4. Apply smooth movement
        rb.MovePosition(newPosition);
    }
}