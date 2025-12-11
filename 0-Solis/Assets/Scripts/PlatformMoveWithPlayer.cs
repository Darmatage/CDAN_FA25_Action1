using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class PlatformMoveWithPlayer : MonoBehaviour {
public AudioSource movingSFX;
       [SerializeField]
       private Vector3 velocity;

       private bool moving = false;

       private void FixedUpdate(){
              if (moving){
                     transform.position += (velocity * Time.deltaTime);
                     movingSFX.Play();
              }
       }
       public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "ElevatorStop" ){
			moving = false;
                     movingSFX.Stop();
		}
	}
       private void OnCollisionEnter2D(Collision2D other){
              if (other.gameObject.tag == "Player"){
                     moving = true;
                     other.collider.transform.SetParent(transform); // so Player moves with platform
                     
              }
       }

       private void OnCollisionExit2D(Collision2D other){
              if (other.gameObject.tag == "Player"){
                     moving = false;
                     other.collider.transform.SetParent(null); // Player not parented when off platform
              }
      }
}