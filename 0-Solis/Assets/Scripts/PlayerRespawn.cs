using System.Collections;
using UnityEngine;
// Needed for Light2D
using UnityEngine.Rendering.Universal;


public class PlayerRespawn : MonoBehaviour
{
    [Header("References")]
    public Enemy_BossTrigger bossTrigger;
    public GameHandler gameHandler;
    public Transform pSpawn;
    //public bool BossExists = false;

    [Header("Death Settings")]
    public Animator playerAnimator;
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip tickSound;

    [Header("Sprites to Toggle on Death")]
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;

    [Header("Lights to Toggle on Death")]
    public Light2D light1;
    public Light2D light2;
    public Light2D light3; // NEW third light

    private bool isDead = false;

    void Start()
    {
        // Get GameHandler if not assigned
        if (gameHandler == null)
        {
            GameObject ghObj = GameObject.FindWithTag("GameHandler");
            if (ghObj != null)
                gameHandler = ghObj.GetComponent<GameHandler>();
        }
        if (light3 != null) light3.enabled = false;
    }

    void Update()
    {
        if (isDead) return;

        if (GameHandler.playerHealth <= 0)
        {
            isDead = true;
            StartCoroutine(DeathWithRespawn());
        }
    }

    public void SetSpawnPoint(Transform newSpawn)
    {
        pSpawn = newSpawn;
        Debug.Log("New spawn point set: " + pSpawn.name);
    }

    private IEnumerator DeathWithRespawn()
    {
        // Disable sprites
        if (sprite1 != null) sprite1.enabled = false;
        if (sprite2 != null) sprite2.enabled = false;

        // Disable lights
        if (light1 != null) light1.enabled = false;
        if (light2 != null) light2.enabled = false;
        if (light3 != null) light3.enabled = true; // NEW

        // Disable movement
        PlayerMove move = GetComponent<PlayerMove>();
        PlayerJump jump = GetComponent<PlayerJump>();
        if (move != null) move.isAlive = false;
        if (jump != null) jump.isAlive = false;

        // Play death animation and sound
        if (playerAnimator != null) playerAnimator.SetTrigger("Die");
        if (audioSource != null && deathSound != null) audioSource.PlayOneShot(deathSound);

        yield return new WaitForSeconds(1f);

        // Play tick sound 3 times
        for (int i = 0; i < 3; i++)
        {
            if (audioSource != null && tickSound != null)
                audioSource.PlayOneShot(tickSound);
            yield return new WaitForSeconds(0.5f);
        }

        // Decrease lives and handle zero lives
        GameHandler.Lives = Mathf.Max(GameHandler.Lives - 1, 0);
        if (GameHandler.Lives <= 0)
        {
            gameHandler.updateStatsDisplay();
            // Load game over scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndLose");
            yield break;
        }

        // Move player to spawn
        if (pSpawn != null)
            transform.position = new Vector3(pSpawn.position.x, pSpawn.position.y, transform.position.z);
        else
            Debug.LogWarning("No spawn point set for player!");

        // Re-enable sprites and lights
        if (sprite1 != null) sprite1.enabled = true;
        if (sprite2 != null) sprite2.enabled = true;
        if (light1 != null) light1.enabled = true;
        if (light2 != null) light2.enabled = true;
        if (light3 != null) light3.enabled = false; // NEW

        // Trigger respawn animation
        if (playerAnimator != null) playerAnimator.SetTrigger("Respawned");

        // Re-enable movement
        if (move != null) move.isAlive = true;
        if (jump != null) jump.isAlive = true;

        // Reset health and update UI AFTER respawn
        GameHandler.playerHealth = gameHandler.StartPlayerHealth;
        if (gameHandler != null)
            gameHandler.updateStatsDisplay();

        if (bossTrigger != null)
{
    bossTrigger.ResetBosses();
}

        isDead = false;
    }
}
