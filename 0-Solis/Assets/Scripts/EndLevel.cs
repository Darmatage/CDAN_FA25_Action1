using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene switching
using UnityEngine.Rendering.Universal;

public class EndLevel : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to switch to

    public GameObject buttonUp;
    public GameObject buttonDown;
    public bool snapToNearestGridSpace = true;

    [Header("Audio")]
    public AudioSource audioSource;   
    public AudioClip soundOn;         
    public AudioClip soundOff;        

    [Header("Lights")]
    public Light2D onLight;     // Light when button is pressed
    public Light2D offLight;    // Light when button is released

    private bool pressed = false;

    void Start()
    {
        buttonUp.SetActive(true);
        buttonDown.SetActive(false);

        UpdateLights();

        if (snapToNearestGridSpace)
        {
            float newX = Mathf.Round(transform.position.x - 0.5f) + 0.5f;
            float newY = Mathf.Round(transform.position.y - 0.5f) + 0.5f;
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            pressed = !pressed;

            buttonUp.SetActive(!pressed);
            buttonDown.SetActive(pressed);

            // Play sound
            if (audioSource != null)
            {
                if (pressed && soundOn != null)
                    audioSource.PlayOneShot(soundOn);
                else if (!pressed && soundOff != null)
                    audioSource.PlayOneShot(soundOff);
            }

            // Update light states
            UpdateLights();

            // Switch scene
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void UpdateLights()
    {
        if (onLight != null)
            onLight.enabled = pressed;

        if (offLight != null)
            offLight.enabled = !pressed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Button state persists, so no action needed
    }
}
