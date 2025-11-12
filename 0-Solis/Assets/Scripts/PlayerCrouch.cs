using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour {

      //public Animator animator;
      public Rigidbody2D rb2D;
      public GameObject torso;
      public Transform feet;
      public bool isAlive = true;
      public LayerMask groundLayer;
      public LayerMask enemyLayer;

      void Start(){
            //animator = gameObject.GetComponentInChildren<Animator>();
            rb2D = GetComponent<Rigidbody2D>();
      }

     void Update() {
           if ((Input.GetButtonDown("Crouch")) && (IsGrounded()) && (isAlive==true)) {
                  torso.SetActive(false);
                  //animator.SetBool("Crouch", true);
            }
           if (Input.GetButtonUp("Crouch")) {
                  torso.SetActive(true);
                  //animator.SetBool("Crouch", false);
            }
      }

      public bool IsGrounded() {
            Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 2f, groundLayer);
            Collider2D enemyCheck = Physics2D.OverlapCircle(feet.position, 2f, enemyLayer);
           if ((groundCheck != null) || (enemyCheck != null)) {
                  return true;
                  //Debug.Log("I can crouch now!");
            }
            return false;
      }
}