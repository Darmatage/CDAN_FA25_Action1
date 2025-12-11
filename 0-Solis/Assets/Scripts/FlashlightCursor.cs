using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightCursor : MonoBehaviour
{
    public Transform flashlight;
    public static float distanceMultiplier;
    public float distanceMultiplierStart = 6f;

    public float rotationSpeed = 30f;
    public float distance = 50f;
    public LineRenderer lineOfSight;
    public Light2D Glow;
    public Gradient redColor;
    public Gradient greenColor;

    private bool lightsOn = false;
    private float energyTimer = 0f;
    public GameObject lastHitObject;

    // AudioSources for flashlight on/off
    public AudioSource flashlightOnSound;
    public AudioSource flashlightOffSound;

    void Start()
    {
        distanceMultiplier = distanceMultiplierStart;
        Physics2D.queriesStartInColliders = false;
        lineOfSight.gameObject.SetActive(false);
        Glow.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleFlashlightToggle();
        HandleEnergyDrain();
        HandleRaycast();
    }

    private void HandleFlashlightToggle()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GameHandler.playerHealth <= 0)
            {
                Debug.Log("Cannot turn on flashlight — player is dead!");
                return;
            }

            // Toggle flashlight state
            lightsOn = !lightsOn;
            lineOfSight.gameObject.SetActive(lightsOn);
            Glow.gameObject.SetActive(lightsOn);

            // Play sound on manual toggle
            if (lightsOn)
            {
                if (flashlightOnSound != null) flashlightOnSound.Stop(); // prevent stacking
                flashlightOnSound?.Play();
            }
            else
            {
                if (flashlightOffSound != null) flashlightOffSound.Stop(); // prevent stacking
                flashlightOffSound?.Play();
            }
        }

        // Auto turn-off if player is dead (silent)
        if (lightsOn && GameHandler.playerHealth <= 0)
        {
            lightsOn = false;
            lineOfSight.gameObject.SetActive(false);
            Glow.gameObject.SetActive(false);
            lastHitObject = null;
            Debug.Log("Flashlight turned off — player is dead!");
        }
    }

    private void HandleEnergyDrain()
    {
        if (!lightsOn) return;

        energyTimer += Time.deltaTime;

        if (energyTimer >= 0.75f)
        {
            energyTimer = 0f;

            if (GameHandler.gotTokens > 0)
            {
                GameHandler.SpendTokens(1);
            }
            else
            {
                // Auto-off due to no energy (silent)
                lightsOn = false;
                lineOfSight.gameObject.SetActive(false);
                Glow.gameObject.SetActive(false);
                lastHitObject = null;
                Debug.Log("Flashlight turned off — no energy left!");
            }
        }
    }

    private void HandleRaycast()
    {
        if (!lightsOn) return;

        Vector2 direction = (flashlight.position - transform.position).normalized;
        Vector3 offsetVector = direction * distanceMultiplier;
        Vector2 lightDistance = flashlight.position + offsetVector;

        RaycastHit2D hit = Physics2D.Raycast(flashlight.position, direction, distance);

        lineOfSight.SetPosition(0, flashlight.position);

        if (hit.collider != null)
        {
            Debug.DrawLine(flashlight.position, hit.point, Color.red);
            lineOfSight.SetPosition(1, hit.point);
            lineOfSight.colorGradient = redColor;

            if (hit.collider.gameObject != lastHitObject)
            {
                if (lastHitObject != null)
                {
                    if (lastHitObject.CompareTag("Battery"))
                        lastHitObject.GetComponent<PickUp>().disableBatteryPickup();
                    if (lastHitObject.CompareTag("Enemy"))
                        lastHitObject.GetComponent<EnemyMeleeDamage>().Flash_Unlit();
                }
            }

            lastHitObject = hit.collider.gameObject;

            if (hit.collider.CompareTag("Battery"))
                hit.collider.GetComponent<PickUp>().enableBatteryForPickup();
            if (hit.collider.CompareTag("Enemy"))
                hit.collider.GetComponent<EnemyMeleeDamage>().Flash_Lit();
        }
        else
        {
            Debug.DrawLine(flashlight.position, lightDistance, Color.green);
            lineOfSight.SetPosition(1, lightDistance);
            lineOfSight.colorGradient = greenColor;
            lastHitObject = null;
        }
    }
}
