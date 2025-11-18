using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerMoveAimShoot : MonoBehaviour{

      //public Animator animator;
      public float moveSpeed = 5f;
      private Rigidbody2D rb;
      public Camera cam;
      public Vector2 movement;
      public Vector2 mousePos;
      public Transform fireBase;
      public Transform firePoint;
     
      public GameObject projectilePrefab;
      public float projectileSpeed = 10f;
      public float attackRate = 2f;
      private float nextAttackTime = 0f;
      //public GameObject muzzleFlash;

      void Start(){
           //animator = gameObject.GetComponentInChildren<Animator>();
           rb = GetComponent<Rigidbody2D>();
           cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>() as Camera;
      }

      void Update(){
           movement.x = Input.GetAxisRaw("Horizontal");
           movement.y = Input.GetAxisRaw("Vertical");
           mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

           if (Time.time >= nextAttackTime){
                  //if (Input.GetKeyDown(KeyCode.Space))
                 if (Input.GetAxis("Attack") > 0){
                        playerFire();
                        nextAttackTime = Time.time + 1f / attackRate;
                  }
            }
      }


      void FixedUpdate(){
            //actual movement uses Rigidbody2D, so goes in FixedUpdate:
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            Vector2 lookDir = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            //rb.rotation = angle;
            fireBase.transform.eulerAngles = new Vector3(0, 0, angle);
      }

      void playerFire(){
            //animator.SetTrigger ("Fire");
            Vector2 fwd = (firePoint.position - this.transform.position).normalized;

            /*
            //MUZZLEFLASH:
            GameObject muzFlash = Instantiate(muzzleFlash, firePoint.position, Quaternion.identity);
            Vector2 muzDirection = firePoint.position - transform.position;
            Quaternion muzRotation = Quaternion.LookRotation(muzDirection, Vector3.up);
            muzFlash.transform.rotation = muzRotation;
            */

            //PROJECTILE
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().AddForce(fwd * projectileSpeed, ForceMode2D.Impulse);
      }
}