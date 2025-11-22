using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class OverlayTrio
{
    public Image[] images;          // 3 overlay images
    public TMP_Text[] texts;        // Corresponding child texts
    public int temperatureIndex;    // Index of the text that should drop
}

[System.Serializable]
public class Phase
{
    [Header("Visuals")]
    public Image phaseImage;        // Main phase image
    public bool showOverlayTrio;    // Show the shared overlay trio for this phase

    [Header("Audio")]
    public bool playZeroEnergyAudio;
    public bool playBreathingAudio;
    public bool playAlarmAudio;
}

public class GameHandlerZeroEnergy : MonoBehaviour
{
    [Header("Phases")]
    public Phase[] phases;

    [Header("Shared Overlay Trio")]
    public OverlayTrio sharedOverlayTrio;

    [Header("Audio Settings")]
    public AudioSource phaseAudioSource;
    public AudioClip zeroEnergyClip;
    public AudioClip breathingClip;
    public AudioClip alarmClip;

    public float zeroEnergyMinPitch = 0.8f;
    public float zeroEnergyMaxPitch = 1.2f;
    public float breathingMinPitch = 0.8f;
    public float breathingMaxPitch = 1.2f;
    public float alarmMinPitch = 0.8f;
    public float alarmMaxPitch = 1.2f;

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float displayDuration = 1.5f;
    public float waitBetweenPhases = 2f;

    [Header("Temperature Settings")]
    public int startTemperature = 100;
    public int endTemperature = -10;
    public float fallSpeed = 50f;

    private bool zeroEnergyRunning = false;
    private bool[] phaseTriggered;

    private void Awake()
    {
        phaseTriggered = new bool[phases.Length];

        // Hide all phase images
        foreach (var phase in phases)
        {
            if (phase.phaseImage != null)
                phase.phaseImage.gameObject.SetActive(false);
        }

        // Hide shared overlay trio
        if (sharedOverlayTrio != null)
        {
            foreach (var img in sharedOverlayTrio.images)
                if (img != null) img.gameObject.SetActive(false);

            foreach (var txt in sharedOverlayTrio.texts)
                if (txt != null) txt.gameObject.SetActive(false);
        }
    }

    public void StartZeroEnergySequence()
    {
        if (!zeroEnergyRunning)
            StartCoroutine(ZeroEnergySequence());
    }

    public void StopZeroEnergySequence()
    {
        if (zeroEnergyRunning)
            StopAllCoroutines();

        zeroEnergyRunning = false;

        // Hide shared overlay trio
        if (sharedOverlayTrio != null)
        {
            foreach (var img in sharedOverlayTrio.images)
                if (img != null) img.gameObject.SetActive(false);

            foreach (var txt in sharedOverlayTrio.texts)
                if (txt != null) txt.gameObject.SetActive(false);
        }
    }

    private IEnumerator ZeroEnergySequence()
    {
        zeroEnergyRunning = true;

        for (int i = 0; i < phases.Length; i++)
        {
            if (phaseTriggered[i]) continue;

            phaseTriggered[i] = true;
            Phase currentPhase = phases[i];

            // Show phase image
            if (currentPhase.phaseImage != null)
                currentPhase.phaseImage.gameObject.SetActive(true);

            // Play audio
            if (phaseAudioSource != null)
            {
                if (currentPhase.playZeroEnergyAudio && zeroEnergyClip != null)
                {
                    phaseAudioSource.pitch = Random.Range(zeroEnergyMinPitch, zeroEnergyMaxPitch);
                    phaseAudioSource.PlayOneShot(zeroEnergyClip);
                }

                if (currentPhase.playBreathingAudio && breathingClip != null)
                {
                    phaseAudioSource.pitch = Random.Range(breathingMinPitch, breathingMaxPitch);
                    phaseAudioSource.PlayOneShot(breathingClip);
                }

                if (currentPhase.playAlarmAudio && alarmClip != null)
                {
                    phaseAudioSource.pitch = Random.Range(alarmMinPitch, alarmMaxPitch);
                    phaseAudioSource.PlayOneShot(alarmClip);
                }
            }

            // Fade in shared overlay trio if enabled for this phase
            if (currentPhase.showOverlayTrio && sharedOverlayTrio != null)
            {
                yield return StartCoroutine(FadeOverlayTrio(sharedOverlayTrio, true));

                // Start temperature drop if assigned
                if (sharedOverlayTrio.temperatureIndex >= 0 &&
                    sharedOverlayTrio.temperatureIndex < sharedOverlayTrio.texts.Length &&
                    sharedOverlayTrio.texts[sharedOverlayTrio.temperatureIndex] != null)
                {
                    StartCoroutine(FallTemperature(sharedOverlayTrio.texts[sharedOverlayTrio.temperatureIndex]));
                }
            }

            yield return new WaitForSeconds(displayDuration);

            // Fade out shared overlay trio
            if (currentPhase.showOverlayTrio && sharedOverlayTrio != null)
            {
                yield return StartCoroutine(FadeOverlayTrio(sharedOverlayTrio, false));
            }

            yield return new WaitForSeconds(waitBetweenPhases);

            if (GameHandler.gotTokens > 0)
            {
                StopZeroEnergySequence();
                yield break;
            }
        }

        zeroEnergyRunning = false;
        FindObjectOfType<GameHandler>().playerDies();
    }

    private IEnumerator FadeOverlayTrio(OverlayTrio trio, bool fadeIn)
    {
        float t = 0f;
        float[] originalAlphas = new float[trio.images.Length];

        for (int i = 0; i < trio.images.Length; i++)
        {
            originalAlphas[i] = trio.images[i] != null ? trio.images[i].color.a : 1f;
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float factor = fadeIn ? Mathf.Lerp(0f, 1f, t / fadeDuration) : Mathf.Lerp(1f, 0f, t / fadeDuration);

            for (int i = 0; i < trio.images.Length; i++)
            {
                if (trio.images[i] != null)
                {
                    Color c = trio.images[i].color;
                    c.a = originalAlphas[i] * factor;
                    trio.images[i].color = c;
                    trio.images[i].gameObject.SetActive(true);
                }
            }

            for (int i = 0; i < trio.texts.Length; i++)
            {
                if (trio.texts[i] != null)
                {
                    Color c = trio.texts[i].color;
                    c.a = factor;
                    trio.texts[i].color = c;
                    trio.texts[i].gameObject.SetActive(true);
                }
            }

            yield return null;
        }

        if (!fadeIn)
        {
            foreach (var img in trio.images)
                if (img != null) img.gameObject.SetActive(false);

            foreach (var txt in trio.texts)
                if (txt != null) txt.gameObject.SetActive(false);
        }
    }

    private IEnumerator FallTemperature(TMP_Text tempText)
    {
        float currentTemp = startTemperature;
        while (currentTemp > endTemperature)
        {
            currentTemp -= fallSpeed * Time.deltaTime;
            tempText.text = $"Temperature DROPPING: {Mathf.FloorToInt(currentTemp)}°";
            yield return null;
        }
        tempText.text = $"Temperature DROPPING: {endTemperature}°";
    }
}
