using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private GameObject player;
    private GameObject flashlight;
    public static int playerHealth = 100;
    public int StartPlayerHealth = 100;
    public TMP_Text healthText;
    public static int Lives;
    public int maxLives = 5;
    private PlayerRespawn playerRespawn;

    public GameObject textLives;

    public static int gotTokens = 100;
    public TMP_Text tokensText;

    public bool isDefending = false;

    public static bool stairCaseUnlocked = false;

    private string sceneName;
    public static string lastLevelDied;

    // Battery meter images (9 public slots)
    public Image battery0;
    public Image battery1;
    public Image battery2;
    public Image battery3;
    public Image battery4;
    public Image battery5;
    public Image battery6;
    public Image battery7;
    public Image battery8;

    // --- New: Reference to zero-energy UI effect
    public GameHandlerZeroEnergy zeroEnergyUIEffect;

    // --- New: Damage flash settings ---
    public Image damageFlashImage; // assign in inspector
    public float flashDuration = 0.2f; // total flash time (fade in + fade out)

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        player = GameObject.FindWithTag("Player");

        // If no player exists (Main Menu, Credits, etc), stop here
        if (player == null)
        {
            Debug.Log("No player found in this scene. GameHandler running in non-gameplay scene.");
            return;
        }

        // Gameplay-only setup
        playerHealth = StartPlayerHealth;

        playerRespawn = player.GetComponent<PlayerRespawn>();

        if (Lives <= 0)
            Lives = maxLives;

        updateStatsDisplay();
    }

    public void playerGetTokens(int newTokens)
    {
        gotTokens += newTokens;
        updateStatsDisplay();

        if (gotTokens > 0 && zeroEnergyUIEffect != null)
        {
            zeroEnergyUIEffect.StopZeroEnergySequence();
        }
    }

    public void playerGetHit(int damage)
    {
        if (!isDefending)
        {
            playerHealth -= damage;
            if (playerHealth >= 0)
            {
                updateStatsDisplay();
            }
            if (damage > 0)
            {
                // Play GetHit animation
                player.GetComponent<PlayerHurt>().playerHit();

                // Trigger damage flash
                if (damageFlashImage != null)
                    StartCoroutine(DamageFlashCoroutine());
            }
        }

        if (playerHealth > StartPlayerHealth)
        {
            playerHealth = StartPlayerHealth;
            updateStatsDisplay();
        }

        if (playerHealth <= 0)
        {
            playerHealth = 0;
            updateStatsDisplay();
            playerDies();
        }
    }

    public static void SpendTokens(int amount)
    {
        gotTokens = Mathf.Max(0, gotTokens - amount);
        FindObjectOfType<GameHandler>().updateStatsDisplay();
    }

    public static void GainTokens(int amount)
    {
        gotTokens = Mathf.Min(100, gotTokens + amount);
        FindObjectOfType<GameHandler>().updateStatsDisplay();
    }

    public void updateStatsDisplay()
    {
        healthText.text = "HEALTH: " + playerHealth;
        tokensText.text = "ENERGY: " + gotTokens;
        textLives.GetComponent<TMP_Text>().text = "LIVES: " + Lives;

        // Update battery meter images
        battery0.enabled = battery1.enabled = battery2.enabled = battery3.enabled = battery4.enabled =
        battery5.enabled = battery6.enabled = battery7.enabled = battery8.enabled = false;

        if (gotTokens <= 11)
            battery0.enabled = true;
        else if (gotTokens <= 23)
            battery1.enabled = true;
        else if (gotTokens <= 34)
            battery2.enabled = true;
        else if (gotTokens <= 45)
            battery3.enabled = true;
        else if (gotTokens <= 56)
            battery4.enabled = true;
        else if (gotTokens <= 67)
            battery5.enabled = true;
        else if (gotTokens <= 78)
            battery6.enabled = true;
        else if (gotTokens <= 89)
            battery7.enabled = true;
        else
            battery8.enabled = true;

        // Trigger zero energy sequence
        if (gotTokens <= 0 && zeroEnergyUIEffect != null)
        {
            zeroEnergyUIEffect.StartZeroEnergySequence();
        }
    }

    public void playerDies()
    {
        if (zeroEnergyUIEffect != null)
            zeroEnergyUIEffect.StopZeroEnergySequence();

        if (Lives <= 0)
        {
            Lives = 0;
            updateStatsDisplay();
            SceneManager.LoadScene("EndLose");
            return;
        }

        // still have lives, so respawn instead of reload
        StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        if (zeroEnergyUIEffect != null)
            zeroEnergyUIEffect.StopZeroEnergySequence();

        gotTokens = 100;

        yield return new WaitForSeconds(1f);

        updateStatsDisplay();
    }

    private IEnumerator DamageFlashCoroutine()
    {
        float halfDuration = flashDuration / 1.25f;

        // Fade in
        Color color = damageFlashImage.color;
        color.a = 0f;
        damageFlashImage.color = color;

        float timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / halfDuration);
            damageFlashImage.color = color;
            yield return null;
        }

        // Fade out
        timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / halfDuration);
            damageFlashImage.color = color;
            yield return null;
        }

        color.a = 0f;
        damageFlashImage.color = color;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        playerHealth = StartPlayerHealth;
    }

    public void ReplayLastLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(lastLevelDied);
        playerHealth = StartPlayerHealth;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
