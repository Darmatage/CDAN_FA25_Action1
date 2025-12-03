using System.Collections.Generic;
using UnityEngine;

public class WallToggle : MonoBehaviour
{
    public int channel = 0;
    public bool startOn = true;
    private bool toggledOn = true;

    public GameObject wallUp;
    public GameObject wallDown;
    public bool snapToNearestGridSpace = true;

    [Header("Audio")]
    public AudioSource audioSource;   // Drag an AudioSource here
    public AudioClip soundOn;         // When wall goes up / becomes solid
    public AudioClip soundOff;        // When wall goes down / disabled

    void Start()
    {
        // Initial state
        wallUp.SetActive(true);
        wallDown.SetActive(false);

        // Link wall to all matching buttons
        GameObject[] buttonArr = GameObject.FindGameObjectsWithTag("WallToggleButton");
        foreach (GameObject button in buttonArr)
        {
            WallToggleButton wtb = button.GetComponent<WallToggleButton>();
            if (wtb != null && wtb.channel == channel)
            {
                wtb.wallList.Add(this);
            }
        }

        // Apply start state
        if (!startOn) { ToggleState(); }

        // Snap to grid
        if (snapToNearestGridSpace)
        {
            float newX = Mathf.Round(transform.position.x - 0.5f) + 0.5f;
            float newY = Mathf.Round(transform.position.y - 0.5f) + 0.5f;
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
    }

    public void ToggleState()
    {
        toggledOn = !toggledOn;

        if (toggledOn)
        {
            // Wall ON / Up
            wallUp.SetActive(true);
            wallDown.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = true;

            // Play ON sound
            if (audioSource != null && soundOn != null)
                audioSource.PlayOneShot(soundOn);
        }
        else
        {
            // Wall OFF / Down
            wallUp.SetActive(false);
            wallDown.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;

            // Play OFF sound
            if (audioSource != null && soundOff != null)
                audioSource.PlayOneShot(soundOff);
        }
    }

    public void SetColor(Color col)
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            sr.color = col;
    }
}
