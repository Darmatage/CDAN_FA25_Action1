using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
      [Header("Components")]
      public Animator animator;
      public Rigidbody2D rb2D;
      public PlayerJump jumpScript;

      [Header("Movement")]
      private bool FaceRight = false;
      public static float runSpeed = 10f;
      public bool isAlive = true;
      private Vector3 hMove;

      [Header("Footsteps")]
      public AudioSource[] SFX_Steps;        // Assign all footstep AudioSources
      private AudioSource StepToPlay;
      public float stepInterval = 0.35f;     // Time between footsteps
      private float stepTimer = 0f;
      public float minPitch = 0.95f;         // Pitch variation
      public float maxPitch = 1.05f;

      void Start(){
           animator = gameObject.GetComponentInChildren<Animator>();
           rb2D = transform.GetComponent<Rigidbody2D>();
      }

      void Update(){
           hMove = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);

           if (isAlive){
                  transform.position += hMove * runSpeed * Time.deltaTime;

                  animator.SetBool("Walk", Input.GetAxis("Horizontal") != 0);

                  // Footstep logic: only play if on ground (canJump) and moving
                  if (jumpScript != null && jumpScript.canJump && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)){
                        stepTimer += Time.deltaTime;
                        if (stepTimer >= stepInterval){
                              PlaySteps();
                              stepTimer = 0f;
                        }
                  } else {
                        StopSteps();
                        stepTimer = 0f; // reset timer when stopping
                  }
           }
      }

      public void PlaySteps(){
             if (SFX_Steps.Length == 0) return;

             int StepNum = Random.Range(0, SFX_Steps.Length);
             StepToPlay = SFX_Steps[StepNum];

             // Random pitch variation
             StepToPlay.pitch = Random.Range(minPitch, maxPitch);
             StepToPlay.Play();
      }

      public void StopSteps(){
             if ((StepToPlay != null) && (StepToPlay.isPlaying)){
                   StepToPlay.Stop();
             }
      }

      void FixedUpdate(){
            // Slow down on hills / stops sliding from velocity
            if (hMove.x == 0){
                  rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x / 1.1f, rb2D.linearVelocity.y);
            }
      }

      private void playerTurn(){
            FaceRight = !FaceRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
      }
}
