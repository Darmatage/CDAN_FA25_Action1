using UnityEngine;

public class PlayerBottomRespawn : MonoBehaviour
{
    public GameHandler gameHandler;
    private Transform playerPos;

    private bool canKill = true; // Prevent multiple triggers

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerPos = player.transform;

        gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    void Update()
    {
        if (playerPos == null || !canKill) return;

        // Trigger if player falls below the floor
        if (playerPos.position.y <= transform.position.y)
        {
            Debug.Log("Kill floor triggered — setting player health to 0!");
            canKill = false;

            // Set health to 0 directly
            GameHandler.playerHealth = 0;

            // Update UI and trigger death
            gameHandler.updateStatsDisplay();
            gameHandler.playerDies();
        }
    }
}
