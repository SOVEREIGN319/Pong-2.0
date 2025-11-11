using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public enum GoalOwner { Player, AI }
    public GoalOwner owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            if (ScoreManager.Instance != null)
            {
                if (owner == GoalOwner.Player)
                    ScoreManager.Instance.AddAIScore();  // AI scores when player misses
                else
                    ScoreManager.Instance.AddPlayerScore(); // Player scores when AI misses
            }

            // Reset or destroy ball (optional)
            Destroy(collision.gameObject);
        }
    }
}
