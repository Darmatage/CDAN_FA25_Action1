using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FlashlightCursor : MonoBehaviour
{
    //public Camera cam;

	public Transform flashlight;
	public static float distanceMultiplier;
	public float distanceMultiplierStart = 6;

//from deathgaze: which can we remov?
	public float rotationSpeed = 30;
       public float distance = 50;
       public LineRenderer lineOfSight;
       public Gradient redColor;
       public Gradient greenColor;
       //public GameObject hitEffectAnim;

       public int EnemyLives = 30;
       private Renderer rend;
	bool lightsOn = false;

    void Start()
    {
		distanceMultiplier = distanceMultiplierStart;
		Physics2D.queriesStartInColliders = false;
		lineOfSight.gameObject.SetActive(false);
    }


    void Update()
    {

		if (Input.GetKeyDown("f"))
		{
			lightsOn = !lightsOn;
		}

		Vector2 direction = (flashlight.position - transform.position).normalized;
		//distanceMultiplier = 6f; // Example distance
    	Vector3 offsetVector = direction * distanceMultiplier;
		Vector2 lightDistance = flashlight.position + offsetVector;

		//Debug.DrawLine(flashlight.position, lightDistance, Color.yellow, Time.deltaTime);


		// Raycast needs location, Direction, and length
		RaycastHit2D hitInfo = Physics2D.Raycast (flashlight.position, direction, distance);
	if (lightsOn){	
		lineOfSight.gameObject.SetActive(true);
		if (hitInfo.collider != null) {
			Debug.DrawLine(flashlight.position, hitInfo.point, Color.red);
			lineOfSight.SetPosition(1, hitInfo.point); // index 1 is the end-point of the line
			lineOfSight.colorGradient = redColor;

			if (hitInfo.collider.CompareTag ("Enemy")) {
				//make enemy move back
				//hitInfo.collider.gameObject.GetComponent<EnemyMove>().HitByLight();
				Debug.Log ("I hit an enemy: " + hitInfo.collider.gameObject.name);
			}
		} else {
			Debug.DrawLine(flashlight.position, lightDistance, Color.green);
			lineOfSight.SetPosition(1, lightDistance);
			lineOfSight.colorGradient = greenColor;
		}
		
		lineOfSight.SetPosition (0, flashlight.position); // index 0 is the start-point of the line
	} 
	else
		{
			lineOfSight.gameObject.SetActive(false);
		}  
	   
	}


}