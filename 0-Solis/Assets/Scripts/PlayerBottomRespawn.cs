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

            gameHandler.updateStatsDisplay();
            gameHandler.playerDies();

            // Re-enable after 4 frames
            StartCoroutine(ReEnableKill());
        }
    }

    private System.Collections.IEnumerator ReEnableKill()
    {
        // Wait exactly 4 frames
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        canKill = true;
    }
}
