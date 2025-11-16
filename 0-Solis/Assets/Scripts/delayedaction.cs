using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DelayedAction1 : MonoBehaviour {
       public float timer =2f;       //set the number of seconds here
       private float theTimer = 0f;
       public bool doTheThing = false;

      void Start(){
           theTimer = timer;
      }

      void Update(){
            if (Input.GetKeyDown("8")){
                  doTheThing = true;
            }
      }

       void FixedUpdate(){
            if (doTheThing == true){
                  theTimer -= 0.01f;
                  Debug.Log("time: " + theTimer);
                  if (theTimer <= 0){
                        theTimer = timer;
                        Debug.Log("I do the thing!");       //can be replaced with the desired commands
                        doTheThing = false;
                    }
              }
       }
}