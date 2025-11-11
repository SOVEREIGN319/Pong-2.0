using UnityEngine;

public class QuitButton: MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit button pressed. Exiting game...");
        Application.Quit();
    }
}
