using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerA: MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found. Please add one to the GameObject.");
        }
    }

    void Update()
    {
        // Get mouse position in world coordinates (adjust Z for your camera setup if needed)
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.transform.position.z * -1; // General Z adjustment for 2D

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Define the target position, only changing the Y axis
        targetPosition = new Vector3(transform.position.x, mouseWorldPosition.y, transform.position.z);
    }

    void FixedUpdate()
    {
        // Calculate the next position using MoveTowards
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Use Rigidbody.MovePosition to leverage the physics system for collision resolution
        rb.MovePosition(newPosition);
    }
}

