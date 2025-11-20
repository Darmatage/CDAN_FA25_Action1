using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

      public Animator anim;
      public Rigidbody2D rb;
      public float jumpForce = 20f;
      public Transform feet;
      public LayerMask groundLayer;
      public LayerMask enemyLayer;
      public bool canJump = false;
      public int jumpTimes = 0;
      public bool isAlive = true;
      //public AudioSource JumpSFX;
      public float groundRange = 0.0002f;

      void Start(){
            //anim = gameObject.GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody2D>();
      }

     void Update() {
            if ((IsGrounded()) && (jumpTimes <= 0)){
            // if ((IsGrounded()) && (jumpTimes <= 1)){ // for single jump only
                  canJump = true;
            }  //else if (jumpTimes > 1){
            else { // for single jump only
                  canJump = false;
            }

           if ((Input.GetButtonDown("Jump")) && (canJump) && (isAlive == true)) {
                  Jump();
            }
      }

      public void Jump() {
            jumpTimes += 1;
            rb.linearVelocity = Vector2.up * jumpForce;
            anim.SetTrigger("Jump");
            // JumpSFX.Play();

            //Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
            //rb.velocity = movement;
      }

      public bool IsGrounded() {
            Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, groundRange, groundLayer);
            Collider2D enemyCheck = Physics2D.OverlapCircle(feet.position, groundRange, enemyLayer);
            if ((groundCheck != null) || (enemyCheck != null)) {
                  //Debug.Log("I am trouching ground!");
                  jumpTimes = 0;
                  return true;
            }
            return false;
      }
}