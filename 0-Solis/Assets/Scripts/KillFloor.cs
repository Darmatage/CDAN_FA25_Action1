using UnityEngine;

public class KillFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Instantly kill player
            GameHandler.playerHealth = 0;

            // Force health UI update + death handling
            FindObjectOfType<GameHandler>().updateStatsDisplay();
            FindObjectOfType<GameHandler>().playerDies();
        }
    }
}