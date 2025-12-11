using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cloud : MonoBehaviour
{
	public float speed = 0.2f;
	private Vector2 destination;
	public int distance = 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       destination = new Vector2(transform.position.x + distance, transform.position.y); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 pos = Vector2.Lerp ((Vector2)transform.position, destination, speed * Time.fixedDeltaTime);
		transform.position = new Vector2(pos.x, pos.y);

		float distanceToEnd = Vector3.Distance(transform.position, destination);
		if (distanceToEnd <= 2)
		{
			Destroy(gameObject);
		}

    }
}
