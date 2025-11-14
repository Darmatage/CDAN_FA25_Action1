using UnityEngine;
using System;
using System.Collections;

public class PlayerAimShoot :   MonoBehaviour {

      //public float moveSpeed = 5f;
      private Rigidbody2D rb;
      private Camera cam;
      //public Vector2 movement;
      public Vector2 mousePos;
      public Transform fireBase;

	public Transform shoulderFront;
	public Transform shoulderBack;

	public float rotationSpeed = 720f;

      void Awake(){
            //Assign Rigidbody2D and Camera to variables:
            rb = GetComponent <Rigidbody2D>();
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>() as Camera;
      }

      void Update(){
            if (gameObject != null) {
                  //for position, get axis inputs
                  //movement.x = Input.GetAxisRaw ("Horizontal");
                  //movement.y = Input.GetAxisRaw ("Vertical");
                  //for rotation: get mouse position
                  mousePos = cam.ScreenToWorldPoint (Input.mousePosition);
            }
      }

      void FixedUpdate(){
            //actual movement uses Rigidbody2D, so goes in FixedUpdate
            //rb.MovePosition(rb.position + movement * moveSpeed * Tsime.fixedDeltaTime);
            //actual rotation uses vector math to calculate angle, then rotates character to mouse
            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
			shoulderFront.rotation = Quaternion.Euler(0, 0f, angle);
			shoulderBack.rotation = Quaternion.Euler(0, 0f, angle);
      }
} 