using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuHandler : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer mixer;
    public static float volumeLevel = 1.0f;
    public GameObject button;
    private Slider sliderVolumeCtrl;

    [Header("Pause Sound")]
    public AudioSource sfxSource;     // AudioSource to play sound
    public AudioClip pauseSFX;        // Sound played on pause/resume toggle

    void Awake()
{
    // Set initial volume
    if (mixer != null)
        SetLevel(volumeLevel);
    else
        Debug.LogWarning("AudioMixer not assigned in PauseMenuHandler");

    // Search for slider inside pauseMenuUI
    if (pauseMenuUI != null)
    {
        sliderVolumeCtrl = pauseMenuUI.GetComponentInChildren<Slider>();
        if (sliderVolumeCtrl != null)
        {
            sliderVolumeCtrl.value = volumeLevel;

            // Optional: hook slider change to SetLevel automatically
            sliderVolumeCtrl.onValueChanged.AddListener(SetLevel);
        }
        else
        {
            Debug.LogWarning("No Slider found inside PauseMenuUI!");
        }
    }
    else
    {
        Debug.LogWarning("PauseMenuUI not assigned in PauseMenuHandler!");
    }
}


    void Start()
    {
        pauseMenuUI.SetActive(false);
        GameisPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayPauseSound();
            if (GameisPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (!GameisPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameisPaused = true;

            if (button != null)
                button.SetActive(false);
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;

        if (button != null)
            button.SetActive(true);
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        volumeLevel = sliderValue;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

        // Reset static variables if needed
        GameisPaused = false;
        volumeLevel = 1f;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void PlayPauseSound()
    {
        if (sfxSource != null && pauseSFX != null)
        {
            sfxSource.PlayOneShot(pauseSFX);
        }
    }
}
