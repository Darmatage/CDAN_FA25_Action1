using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Enemy_BossSystem : MonoBehaviour
{
	public Transform[] bossPoints;
	public GameObject bossPrefab;
	Transform nextPoint;
	public float speed = 1f;
	public int nextPointNum = 1;

	private CameraShake cameraShake; 
	public GameObject theBoss;

	Transform player;

	bool playerDead = false;
	
    void Start()
    {
        nextPoint = bossPoints[1];
		cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
		player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (theBoss != null && !playerDead)
		{
			float distToNext = Vector3.Distance(theBoss.transform.position, nextPoint.position);

			if (distToNext <= 1f)
			{
				nextPointNum ++;
				nextPoint= bossPoints[nextPointNum];
			}
			Debug.Log("moving boss");
			theBoss.transform.position = Vector3.MoveTowards(theBoss.transform.position, nextPoint.position, speed);
			//theboss.transform.LookAt(player, Vector3.up);

			Vector2 direction = player.position - theBoss.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            theBoss.transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}
    }


	public void SpawnBoss()
	{
		Debug.Log("spwning boss");
		theBoss = Instantiate(bossPrefab, bossPoints[0].position, Quaternion.identity);
		cameraShake.ShakeCamera(1f, 0.3f);
	}

	public void PlayerDead()
	{
		playerDead = true;
	}


}
