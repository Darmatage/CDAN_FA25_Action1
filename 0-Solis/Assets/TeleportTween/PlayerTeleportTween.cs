using UnityEngine;

public class PlayerTeleportTween : MonoBehaviour
{
	public Transform point1;
	public Transform point2;
	private bool isAt1= false;

//Tween variables:
	public AnimationCurve curveMove = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	public Vector3 tweenOrigin;
	public Vector3 tweenTarget;
	public float tweenElapsed = 0;
	public float tweenSpeed = 1f;
	public bool canMove;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        gameObject.transform.position = point2.position;
    }

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			if (isAt1)
			{
				//gameObject.transform.position = point2.position;
				tweenOrigin = point1.position;
				tweenTarget = point2.position;
				canMove = true;
			}
			else
			{
				//gameObject.transform.position = point1.position;
				tweenOrigin = point2.position;
				tweenTarget = point1.position;
				canMove = true;
			}
			isAt1 = !isAt1;
		}
	}

	void FixedUpdate()
	{

		// Tween Move:
		if (canMove)
		{
			tweenElapsed += Time.fixedDeltaTime * tweenSpeed;
			float height = curveMove.Evaluate(tweenElapsed);
			Vector3 originWithHeight = new Vector3(tweenOrigin.x, tweenOrigin.y + height, tweenOrigin.z);
			Vector3 targetWithHeight = new Vector3(tweenTarget.x, tweenTarget.y + height, tweenTarget.z);
			transform.position = Vector3.Lerp(originWithHeight, targetWithHeight, tweenElapsed);
			if (Vector3.Distance(transform.position, tweenTarget) <= 0.5f)
			{
				canMove = false;
				tweenElapsed = 0f;
			}
		}


	}


}
