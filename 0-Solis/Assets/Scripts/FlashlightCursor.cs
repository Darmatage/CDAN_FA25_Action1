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

	//batteries:
	GameObject lastBattery;

    void Start()
    {
        distanceMultiplier = distanceMultiplierStart;
        Physics2D.queriesStartInColliders = false;
        lineOfSight.gameObject.SetActive(false);
        Glow.gameObject.SetActive(false);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
           
            if (!lightsOn && GameHandler.gotTokens <= 0)
            {
                Debug.Log("Not enough energy to turn on the flashlight!");
            }
            else
            {
                lightsOn = !lightsOn;
            }
        }

       
        if (lightsOn)
        {
            energyTimer += Time.deltaTime;

            if (energyTimer >= 1f) 
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

                    Debug.Log("Flashlight turned off — no energy left!");
                }
            }
        }

      
        Vector2 direction = (flashlight.position - transform.position).normalized;
        Vector3 offsetVector = direction * distanceMultiplier;
        Vector2 lightDistance = flashlight.position + offsetVector;

        RaycastHit2D hitInfo = Physics2D.Raycast(flashlight.position, direction, distance);

        if (lightsOn)
        {
            lineOfSight.gameObject.SetActive(true);
            Glow.gameObject.SetActive(true);

            if (hitInfo.collider != null)
            {
                Debug.DrawLine(flashlight.position, hitInfo.point, Color.red);
                lineOfSight.SetPosition(1, hitInfo.point);
                lineOfSight.colorGradient = redColor;

				if (hitInfo.collider.CompareTag("Battery"))
                {
					lastBattery = hitInfo.collider.gameObject;
                    Debug.Log("I hit a battery: " + hitInfo.collider.gameObject.name);
					hitInfo.collider.gameObject.GetComponent<PickUp>().enableBatteryForPickup();
                }
				else
				{
					if (lastBattery != null)
					{
						lastBattery.GetComponent<PickUp>().disableBatteryPickup();
					}
				}

                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    Debug.Log("I hit an enemy: " + hitInfo.collider.gameObject.name);
                }

            }
            else
            {
                Debug.DrawLine(flashlight.position, lightDistance, Color.green);
                lineOfSight.SetPosition(1, lightDistance);
                lineOfSight.colorGradient = greenColor;
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
