using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WallToggleButton : MonoBehaviour
{
    public int channel = 1; //match button/s channel with dor/channel
    public List<WallToggle> wallList = new List<WallToggle>();

    // NOT USED in game,but another script references: List of objects to disable
    public List<ChannelDisabler> disablerList = new List<ChannelDisabler>();

    public GameObject buttonUp;
    public GameObject buttonDown;
    public bool snapToNearestGridSpace = true;

    [Header("Audio")]
    public AudioSource audioSource;   
    public AudioClip soundOn;         
    public AudioClip soundOff;        

    [Header("Lights")]
    public Light2D onLight;     
    public Light2D offLight;    

    private bool pressed = false;
    private bool setup = true;

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

    void Update()
    {
        if (setup)
        {
            foreach (WallToggle wall in wallList)
            {
                wall.SetColor(GetComponentInChildren<SpriteRenderer>().color);
            }
            setup = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            pressed = !pressed;

            buttonUp.SetActive(!pressed);
            buttonDown.SetActive(pressed);

            // Toggle doors tht have the same channel
            foreach (WallToggle wall in wallList)
            {
                wall.ToggleState();
            }

            // Disable channel objects(old)
            
			foreach (ChannelDisabler disabler in disablerList)
            {
                disabler.DisableObject();
            }
			

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
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
    {
        // Button state persists, so no action needed
    }

    private void UpdateLights()
    {
        if (onLight != null)
            onLight.enabled = pressed;

        if (offLight != null)
            offLight.enabled = !pressed;
    }

    

    // NOT USED in game,but another script references: Method for ChannelDisabler to register itself
	
    public void RegisterDisabler(ChannelDisabler disabler)
    {
        if (!disablerList.Contains(disabler))
            disablerList.Add(disabler);
    }
	

}
