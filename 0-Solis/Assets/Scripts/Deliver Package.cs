using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class DeliverPackage : MonoBehaviour
{
    private KeyCode interactKey = KeyCode.E;

    [Header("References")]
    public AudioSource audioSource;
    public AudioClip activateSound;
    public GameObject tooltipUI;
    public Image fadeImage;

    public Light2D activatedLight;
    public Light2D deactivatedLight;
    public string nextLevelName;

    [Header("Timings")]
    public float blackScreenDelay = 1.0f;
    public float fadeDuration = 2f;
    public float blackScreenHoldTime = 1.5f;

    private bool playerNearby = false;
    private bool activated = false;

    private Animator[] childAnimators;

    void Start()
    {
        if (tooltipUI)
            tooltipUI.SetActive(false);

        if (fadeImage)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        childAnimators = GetComponentsInChildren<Animator>();

        if (activatedLight) activatedLight.enabled = false;
        if (deactivatedLight) deactivatedLight.enabled = true;
    }

    void Update()
    {
        if (playerNearby && !activated && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(ActivateSequence());
        }
    }

    IEnumerator ActivateSequence()
    {
        activated = true;

        if (tooltipUI)
            tooltipUI.SetActive(false);

        if (activateSound && audioSource)
            audioSource.PlayOneShot(activateSound);

        foreach (Animator anim in childAnimators)
        {
            if (anim != null)
                anim.SetTrigger("activated");
        }

        if (activatedLight) activatedLight.enabled = true;
        if (deactivatedLight) deactivatedLight.enabled = false;

        yield return new WaitForSeconds(blackScreenDelay);

        yield return FadeToBlack();

        yield return new WaitForSeconds(blackScreenHoldTime);

        if (!string.IsNullOrEmpty(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (tooltipUI)
                tooltipUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (tooltipUI)
                tooltipUI.SetActive(false);
        }
    }
}
