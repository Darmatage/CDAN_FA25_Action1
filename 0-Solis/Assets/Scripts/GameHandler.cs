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

    public AudioClip zeroEnergyClip;
    public AudioClip breathingClip;
    public float breathingClipMinPitch = 0.8f;
    public float breathingClipPitch = 1.2f;
    public float zeroEnergyMinPitch = 0.8f;
    public float zeroEnergyMaxPitch = 1.2f;

    public AudioSource levelAudioSource;
    //public AudioClip levelStartClip;      
    //public float minPitch = 0.8f;         
    //public float maxPitch = 1.2f;

    public bool[] playZeroEnergyAudioPerPhase = new bool[10];
    public bool[] playbreathingAudioPerPhase = new bool[10];  
    // assign in Inspector: true/false for each zero-energy phase

    public static int gotTokens = 100;
    public TMP_Text tokensText;

    public bool isDefending = false;

    public static bool stairCaseUnlocked = false;
    //this is a flag check. Add to other scripts: GameHandler.stairCaseUnlocked = true;

    private string sceneName;
    public static string lastLevelDied; //allows replaying the Level where you died

    // Battery meter images (9 public slots)
    public Image battery0; // 0-11%
    public Image battery1; // 12-23%
    public Image battery2; // 24-34%
    public Image battery3; // 35-45%
    public Image battery4; // 46-56%
    public Image battery5; // 57-67%
    public Image battery6; // 68-78%
    public Image battery7; // 79-89%
    public Image battery8; // 90-100%

    // --- New: Zero-energy phase images (10 phases)
    public Image[] zeroEnergyPhases = new Image[10];  // assign 10 images in inspector
    private bool zeroEnergySequenceRunning = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sceneName = SceneManager.GetActiveScene().name;
        //if (sceneName=="MainMenu"){ //uncomment these two lines when the MainMenu exists
        playerHealth = StartPlayerHealth;

        foreach (Image img in zeroEnergyPhases)
        {
            if (img != null)
                img.enabled = false;

            //if (levelAudioSource != null && levelStartClip != null){
            //levelAudioSource.pitch = Random.Range(minPitch, maxPitch);
            //levelAudioSource.PlayOneShot(levelStartClip);
            //}
        }

        updateStatsDisplay();
        //}
        updateStatsDisplay();
    }

    public void playerGetTokens(int newTokens)
    {
        gotTokens += newTokens;
        updateStatsDisplay();

        // Stop zero energy sequence if energy is regained
        if (gotTokens > 0 && zeroEnergySequenceRunning)
        {
            StopCoroutine("ZeroEnergySequence");
            zeroEnergySequenceRunning = false;

            // Hide all phase images
            foreach (Image img in zeroEnergyPhases)
            {
                if (img != null)
                    img.enabled = false;
            }
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
                //play GetHit animation:
                player.GetComponent<PlayerHurt>().playerHit();
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
        gotTokens = Mathf.Max(0, gotTokens - amount);    // prevent negatives
        FindObjectOfType<GameHandler>().updateStatsDisplay();
    }

    public static void GainTokens(int amount)
    {
        gotTokens = Mathf.Min(100, gotTokens + amount);    // prevent over 100
        FindObjectOfType<GameHandler>().updateStatsDisplay();
    }

    public void updateStatsDisplay()
    {
        healthText.text = "HEALTH: " + playerHealth;
        tokensText.text = "ENERGY: " + gotTokens;

        // Update battery meter images based on gotTokens
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

        // --- New: Trigger zero energy sequence ---
        if (gotTokens <= 0 && !zeroEnergySequenceRunning)
        {
            StartCoroutine("ZeroEnergySequence");
        }
    }

    // --- New: Coroutine for 10-phase zero energy sequence ---
    IEnumerator ZeroEnergySequence()
    {
        zeroEnergySequenceRunning = true;

        for (int i = 0; i < zeroEnergyPhases.Length; i++)
        {
            // Hide all images first
            foreach (Image img in zeroEnergyPhases)
            {
                if (img != null)
                    img.enabled = false;
            }

            // Show the current phase image
            if (zeroEnergyPhases[i] != null)
                zeroEnergyPhases[i].enabled = true;

            // Play audio for this phase using the existing levelAudioSource if the toggle for this phase is on
            if (levelAudioSource != null && zeroEnergyClip != null)
            {
                if (playZeroEnergyAudioPerPhase.Length > i && playZeroEnergyAudioPerPhase[i])
                {
                    levelAudioSource.pitch = Random.Range(zeroEnergyMinPitch, zeroEnergyMaxPitch);
                    levelAudioSource.PlayOneShot(zeroEnergyClip);
                }
            }
            if (levelAudioSource != null && breathingClip != null)
            {
                if (playbreathingAudioPerPhase.Length > i && playbreathingAudioPerPhase[i])
                {
                    levelAudioSource.pitch = Random.Range(breathingClipMinPitch, breathingClipPitch);
                    levelAudioSource.PlayOneShot(breathingClip);
                }
            }

            yield return new WaitForSeconds(2f);

            // Stop if energy regained
            if (gotTokens > 0)
            {
                zeroEnergySequenceRunning = false;
                foreach (Image img in zeroEnergyPhases)
                {
                    if (img != null)
                        img.enabled = false;
                }
                yield break;
            }
        }

        // After last phase, player dies
        zeroEnergySequenceRunning = false;
        playerDies();
    }

    public void playerDies()
    {
        player.GetComponent<PlayerHurt>().playerDead();       //play Death animation
        lastLevelDied = sceneName;       //allows replaying the Level where you died
        StartCoroutine(DeathPause());
    }

    IEnumerator DeathPause()
    {
        player.GetComponent<PlayerMove>().isAlive = false;
        player.GetComponent<PlayerJump>().isAlive = false;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("EndLose");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Return to MainMenu
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        // Reset all static variables here, for new games:
        playerHealth = StartPlayerHealth;
    }

    // Replay the Level where you died
    public void ReplayLastLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(lastLevelDied);
        // Reset all static variables here, for new games:
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
}
