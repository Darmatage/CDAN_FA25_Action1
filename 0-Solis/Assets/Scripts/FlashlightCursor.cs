using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FlashlightCursor : MonoBehaviour
{
    public Transform flashlight;
    public static float distanceMultiplier;
    public float distanceMultiplierStart = 6;

    public float rotationSpeed = 30;
    public float distance = 50;
    public LineRenderer lineOfSight;
    public UnityEngine.Rendering.Universal.Light2D Glow;
    public Gradient redColor;
    public Gradient greenColor;

    bool lightsOn = false;

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
        // Prevent turning on flashlight if player is dead
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (GameHandler.playerHealth <= 0)
            {
                Debug.Log("Cannot turn on flashlight — player is dead!");
            }
            else
            {
                lightsOn = !lightsOn;

                // Play the appropriate sound only when turning on
                if (lightsOn && flashlightOnSound != null) flashlightOnSound.Play();
            }
        }

        // Automatically turn off flashlight and glow if player health is 0 (no sound)
        if (lightsOn && GameHandler.playerHealth <= 0)
        {
            lightsOn = false;
            lineOfSight.gameObject.SetActive(false);
            Glow.gameObject.SetActive(false);
            lastHitObject = null; // Clear reference
            Debug.Log("Flashlight turned off — player is dead!");
        }

        // Energy drain logic
        if (lightsOn)
        {
            energyTimer += Time.deltaTime;

            if (energyTimer >= 0.5f) 
            {
                energyTimer = 0f;

                if (GameHandler.gotTokens > 0)
                {
                    GameHandler.SpendTokens(1);
                }
                else
                {
                    lightsOn = false;
                    lineOfSight.gameObject.SetActive(false);
                    Glow.gameObject.SetActive(false);

                    if (flashlightOffSound != null) flashlightOffSound.Play();

                    lastHitObject = null; // Clear reference
                    Debug.Log("Flashlight turned off — no energy left!");
                }
            }
        }

        // FLASHLIGHT RAYCAST COLLISIONS
        Vector2 direction = (flashlight.position - transform.position).normalized;
        Vector3 offsetVector = direction * distanceMultiplier;
        Vector2 lightDistance = flashlight.position + offsetVector;

        RaycastHit2D hit = Physics2D.Raycast(flashlight.position, direction, distance);

        if (lightsOn)
        {
            lineOfSight.gameObject.SetActive(true);
            Glow.gameObject.SetActive(true);

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
                    hit.collider.gameObject.GetComponent<PickUp>().enableBatteryForPickup();
                if (hit.collider.CompareTag("Enemy"))
                    hit.collider.gameObject.GetComponent<EnemyMeleeDamage>().Flash_Lit();
            }
            else
            {
                Debug.DrawLine(flashlight.position, lightDistance, Color.green);
                lineOfSight.SetPosition(1, lightDistance);
                lineOfSight.colorGradient = greenColor;
                lastHitObject = null;
            }

            lineOfSight.SetPosition(0, flashlight.position);
        }
        else
        {
            lineOfSight.gameObject.SetActive(false);
            Glow.gameObject.SetActive(false);
        }
    }
}
