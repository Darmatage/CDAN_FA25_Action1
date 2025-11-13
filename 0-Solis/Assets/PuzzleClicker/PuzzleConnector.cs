using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PuzzleConnector : MonoBehaviour
{
	public bool isConnectedToPower = false;

    void Start(){
    }

	//requires a Rigidbody2d to register collisins. Set gravity = 0:
    void OnTriggerStay2D(Collider2D other){

        if (other.gameObject.tag=="Connector"){
			if (other.gameObject.GetComponentInParent<PuzzleClicker>().isPowered){
				isConnectedToPower = true;
				
				if (GetComponentInParent<PuzzleClicker>().isEnd == true){
					GetComponentInParent<PuzzleMASTER>().PuzzleComplete();
					GetComponentInParent<PuzzleClicker>().EndYellow();
				}
				if (GetComponentInParent<PuzzleClicker>().isBomb == true){
					//add a consquence. Boom, lose? Or are wetrying to detnate the bombs?
					GetComponentInParent<PuzzleClicker>().EndRed();
					Debug.Log("You powered a bomb!");
				}
			}
			else{
				isConnectedToPower = false;
			}
			//Debug.Log("I am connected to power!");
		} 
    }

	void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag=="Connector"){
			isConnectedToPower = false; // [problematic logic?]
		} 
    }
}
