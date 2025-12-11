using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonTween_AlphaMove : MonoBehaviour{
       public AnimationCurve curveMove = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
       float elapsedMove = 0f;

       public bool isButton1 = false;
       bool doButton1 = false;
       public bool isButton2 = false;
       bool doButton2 = false;
       public bool isButton3 = false;
       bool doButton3 = false;

       float timer = 0;
       float button1Timer = 0.5f;
       float button2Timer = 1.5f;
       float button3Timer = 2f;

       float preOffsetPos;
       float startOffset = 200f;
       Vector3 startButtonPos;

	public bool isVertical;

	void Start(){
		
		if (isVertical){
			preOffsetPos = transform.position.y; //save the destination
			startButtonPos = transform.position;
			startButtonPos.y += startOffset;
		}
		else
		{
			preOffsetPos = transform.position.x; //save the destination
			startButtonPos = transform.position;
			startButtonPos.x -= startOffset;
		} 
		transform.position = startButtonPos; //set the start position

	}

	void FixedUpdate () {
		timer += Time.fixedDeltaTime;
		if (timer >= button1Timer){doButton1 = true;}
		if (timer >= button2Timer){doButton2 = true;}
		if (timer >= button3Timer){doButton3 = true;}

		if (
			((isButton1) && (doButton1))
			|| ((isButton2) && (doButton2))
			|| ((isButton3) && (doButton3))
		){
			// Tween Move:
			if (isVertical){
				if(startButtonPos.y >= preOffsetPos){
					startButtonPos.y -= curveMove.Evaluate(elapsedMove) * startOffset;
					transform.position = startButtonPos;
				}
			}
			else
			{
				if(startButtonPos.x <= preOffsetPos){
					startButtonPos.x += curveMove.Evaluate(elapsedMove) * startOffset;
					transform.position = startButtonPos;
				}
			}
		}
	}

} 