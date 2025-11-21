using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DoorSwitch : MonoBehaviour{

      public GameObject SwitchOffArt;
      public GameObject SwitchOnArt;
      public GameObject DoorObj;

      void Start() {
            SwitchOffArt.SetActive(true);
            SwitchOnArt.SetActive(false);
            DoorObj = GameObject.FindWithTag("Door");
      }

      void OnTriggerEnter2D(Collider2D other){
            if (other.gameObject.tag == "Player"){
                  SwitchOffArt.SetActive(false);
                  SwitchOnArt.SetActive(true);
                  DoorObj.GetComponent<DoorExit_Switch>().DoorOpen();
            }
      }
}

