using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelDisabler : MonoBehaviour
{
    public int channel = 0; // The channel this object listens to
    private bool isDisabled = false;

    void Start()
    {
        // Find all buttons in the scene
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("WallToggleButton");
        foreach (GameObject button in buttons)
        {
            WallToggleButton wtb = button.GetComponent<WallToggleButton>();
            if (wtb != null && wtb.channel == channel)
            {
                // Add this object to the button's callback list
                wtb.RegisterDisabler(this);
            }
        }
    }

    // Called by the lever/button when activated
    public void DisableObject()
    {
        if (!isDisabled)
        {
            gameObject.SetActive(false);
            isDisabled = true;
        }
    }
}
