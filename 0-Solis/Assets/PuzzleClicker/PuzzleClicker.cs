using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PuzzleClicker : MonoBehaviour {

	private bool selected;
	public SpriteRenderer theSquare;
	public Color makeWhite = new Color(2.5f, 2.5f, 2.5f, 1);
	public Color makeGreen = new Color(1.5f, 2.5f, 1.5f, 1);
	public Color makeYellow = new Color(2.5f, 1.5f, 0.5f, 1);

	private bool canTurn = true;
	int currentRotation = 0;
	float[] rotations = {0f, -90f, 180f, 90f};

	public PuzzleConnector[] myConnectors;

	public bool isPowered = false;
	public bool isPowerStart = false;
	public bool isEnd = false;
	public bool isBomb = false;

	void Start(){
	}

	void Update () {
		if (!isPowerStart){
			IsPieceConnected();
		}
		if (Input.GetMouseButtonUp (0)) {
				canTurn = true;
				if (isPowered){
					theSquare.color = makeYellow;
				} else {
					theSquare.color = makeWhite;
				}
		}
	}

	void OnMouseOver(){
		theSquare.color = makeGreen;
		if (canTurn){
			if (Input.GetMouseButtonDown (0)) {
			
				if (currentRotation <=2){
					currentRotation++;
				}
				else {
					currentRotation= 0;
				}
				RotatePiece();
			}
			if (Input.GetMouseButtonDown(1)){
				if (currentRotation >=1){
					currentRotation--;
				}
				else {
					currentRotation= 3;
				}
				RotatePiece();
			}
    	}	 
	}

	void RotatePiece(){
		Vector3 myRotation = new Vector3(0,0,rotations[currentRotation]);
		transform.localEulerAngles = myRotation; 
		canTurn = false;
		GetComponentInParent<PuzzleMASTER>().ResetAll();
	}

	void OnMouseExit(){
		if (isPowered && !isEnd && !isBomb){
			theSquare.color = makeYellow;
		} else {
			theSquare.color = makeWhite;
		}
	}

	void IsPieceConnected(){
		
		int poweredConnectors = 0;
		for (int i = 0; i < myConnectors.Length; i++){
			if (myConnectors[i].isConnectedToPower==true){
				poweredConnectors++;
			}
		}
		if (poweredConnectors > 0 && !isEnd && !isBomb){
			isPowered= true;
			theSquare.color = makeYellow;
		} else{
			isPowered= false;
			//theSquare.color = makeWhite;
		} 
		/*
		if (!isPowerStart && !isEnd){
			if (myConnectors[0].isConnectedToPower==true || myConnectors[1].isConnectedToPower==true){
				isPowered = true;
				theSquare.color = makeYellow;
			}else{
				isPowered = false;
			}
		}
		*/
	}

	//reset power (command sent by PuzzleMASTER)
	public void TurnOffPower(){
		if (isPowerStart == false){
			isPowered = false;
			for (int i = 0; i < myConnectors.Length; i++){
				myConnectors[i].isConnectedToPower = false;
			}
			IsPieceConnected();
		}
	} 

	//End Complete:
	public void EndYellow(){
		theSquare.color = makeYellow;
		StartCoroutine(EndYellowComplete());
	}
	IEnumerator EndYellowComplete(){
		yield return new WaitForSeconds(0.4f);
		theSquare.color = makeWhite;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeYellow;
		yield return new WaitForSeconds(0.4f);
		theSquare.color = makeWhite;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeYellow;
		yield return new WaitForSeconds(0.4f);
		theSquare.color = makeWhite;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeYellow;
	}


	//If a bomb is triggered:
	public void EndRed(){
		theSquare.color = makeYellow; //I justst this to rd fr this bomb piece
		StartCoroutine(BombDestroy());
	}
	IEnumerator BombDestroy(){
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeWhite;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeYellow;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeWhite;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeYellow;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeWhite;
		yield return new WaitForSeconds(0.2f);
		theSquare.color = makeYellow;
		Destroy(gameObject);
	}


}