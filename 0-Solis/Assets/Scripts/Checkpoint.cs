using UnityEngine;
using TMPro;

public class Checkpoint : MonoBehaviour
{
    [Header("UI Prompt")]
    public GameObject pressEText; // "Press E" popup

    [Header("Lights")]
    public GameObject redLight1;
    public GameObject redLight2;
    public GameObject greenLight1;
    public GameObject greenLight2;

    [Header("Checkpoint Text States")]
    public GameObject text1; // inactive text
    public GameObject text2; // active text

    [Header("Checkpoint Images")]
    public GameObject image1; // off/inactive image
    public GameObject image2; // on/active image

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip checkpointSound;

    private bool playerInRange = false;
    private PlayerRespawn playerRespawn;

    // 🔥 NEW: prevents reactivation
    private bool isActivated = false;

    void Start()
    {
        pressEText.SetActive(false);

        // default inactive state
        greenLight1.SetActive(false);
        greenLight2.SetActive(false);

        text2.SetActive(false);
        text1.SetActive(true);

        image2.SetActive(false);
        image1.SetActive(true);
    }

    void Update()
    {
        // Only allow E if inside AND not activated already
        if (playerInRange && !isActivated && Input.GetKeyDown(KeyCode.E))
        {
            ActivateCheckpoint();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRespawn = other.GetComponent<PlayerRespawn>();
            playerInRange = true;

            // show prompt ONLY if not activated yet
            if (!isActivated)
            {
                pressEText.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // hide prompt
            pressEText.SetActive(false);
        }
    }

   void ActivateCheckpoint()
{
    if (playerRespawn == null) return;

    // Lock activation forever
    isActivated = true;

    // Set spawn
    playerRespawn.SetSpawnPoint(transform);

    // Lights
    redLight1.SetActive(false);
    redLight2.SetActive(false);
    greenLight1.SetActive(true);
    greenLight2.SetActive(true);

    // Texts swap
    text1.SetActive(false);
    text2.SetActive(true);

    // Images swap
    image1.SetActive(false);
    image2.SetActive(true);

    // Sound
    audioSource.PlayOneShot(checkpointSound);

    // Hide prompt permanently
    pressEText.SetActive(false);

    // Give the player +50 tokens
    GameHandler gameHandler = FindObjectOfType<GameHandler>();
    if (gameHandler != null)
    {
        GameHandler.gotTokens = 100;
        FindObjectOfType<GameHandler>().updateStatsDisplay();
    }

    Debug.Log("Checkpoint activated: " + name + " | Tokens awarded");
}
}