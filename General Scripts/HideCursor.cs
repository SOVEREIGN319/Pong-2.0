using UnityEngine;

public class CursorHider : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        // Toggle cursor visibility with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock cursor
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock cursor
                Cursor.visible = false;
            }
        }
    }
}