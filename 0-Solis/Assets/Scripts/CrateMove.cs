using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CrateMove : MonoBehaviour {

       //Set the crate's velocity = zero OnCollisionExit, to prevent sliding:
       void OnCollisionExit2D(Collision2D other){
              gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
              if ((other.gameObject.GetComponent<Rigidbody2D>() != null) && (other.gameObject.GetComponent<Rigidbody2D>().bodyType ==RigidbodyType2D.Dynamic)){
                    gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
              }
        }

}