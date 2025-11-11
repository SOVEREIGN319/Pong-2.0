using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private string sceneName = "VerticalPong"; // Set the scene name here

    public void Play()
    {
        Debug.Log("Play button pressed. Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
