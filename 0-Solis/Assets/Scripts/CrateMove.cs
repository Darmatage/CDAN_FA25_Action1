using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CrateMove : MonoBehaviour {

	public LayerMask groundLayer;
	public Transform crateBottom;
	float groundRange = 0.1f;
	public bool isGrounded = false;

	//For pulling crate, create a joint:
	GameObject thePlayer;
	FixedJoint2D fixedJoint;
	public bool isTouchingPlayer = false;
	public bool canPull = false;

	public Transform jointNode;

	void Update()
	{

		if (Input.GetKeyDown("e")){
			if (isTouchingPlayer && fixedJoint == null){
				jointNode.position = thePlayer.transform.position;
				jointNode.parent = thePlayer.transform;
				gameObject.transform.parent = jointNode;

				fixedJoint = jointNode.gameObject.AddComponent<FixedJoint2D>();
				fixedJoint.connectedBody = thePlayer.GetComponent<Rigidbody2D>();
				fixedJoint.autoConfigureConnectedAnchor=false; 
				canPull = true;
			}
			else if (fixedJoint != null)
			{
				// Release the crate
				Destroy(fixedJoint);
				thePlayer = null;
				canPull = false;
				gameObject.transform.parent = null;
				jointNode.parent = gameObject.transform;
				
			}
		}

	if (fixedJoint!= null)
		{
			if (fixedJoint.connectedBody.gameObject.transform.localScale.x < 0)
			{
				Debug.Log("player turned");
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Debug.Log("Hit [E] to pull");
			isTouchingPlayer = true;
			thePlayer = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player")
		{
			isTouchingPlayer = false;
		}
	}

	//Set the crate's velocity = zero OnCollisionExit, to prevent sliding:
	void OnCollisionExit2D(Collision2D other){
		if (IsGroundedCheck()){
			gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
			if ((other.gameObject.GetComponent<Rigidbody2D>() != null) && (other.gameObject.GetComponent<Rigidbody2D>().bodyType ==RigidbodyType2D.Dynamic)){
				gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
			}
		}
	}

	public bool IsGroundedCheck() {
            Collider2D groundCheck = Physics2D.OverlapCircle(crateBottom.position, groundRange, groundLayer);
            if (groundCheck != null) {
                  //Debug.Log("crate is trouching ground!");
				  isGrounded = true;
                  return true;
            }
			isGrounded=false;
            return false;
	}


	void OnDrawGizmosSelected(){
            Gizmos.DrawWireSphere(crateBottom.position, groundRange);
      }


}

/*
	//check if touching ground:
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		//if (other.gameObject.layer == groundLayer)
		{
			
		}
	}
*/