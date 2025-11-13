using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PuzzleMASTER : MonoBehaviour
{

	public PuzzleClicker[] thePieces; 
	public int numberClicks = 0; //track clicks
	//add timer?

    void Start()
    {
        thePieces = FindObjectsOfType<PuzzleClicker>();
    }

	//whenever a puzzlePiece is clicked: 
    public void ResetAll(){
		for(int i=0; i< thePieces.Length; i++){
				thePieces[i].TurnOffPower();
		}
		numberClicks++;
		//Debug.Log("Clicks so far: " + numberClicks);
	}


	public void PuzzleComplete(){
		Debug.Log("YOU DID IT! PUZZLE COMPLETE in " + numberClicks + " clicks!");
	}

}
