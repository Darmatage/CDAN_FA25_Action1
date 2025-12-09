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
	
	//horde!
	public bool isHorde = false;
	public int numHorde = 10;
	public List<int> nextPointNums = new List<int>();
	public List<GameObject> theBosses = new List<GameObject>();

    void Start()
    {
        nextPoint = bossPoints[nextPointNum];
		cameraShake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
		player = GameObject.FindWithTag("Player").GetComponent<Transform>();

		//horde: initialize arrsy of locatons: not nextPointNums.Count
		for (int i =0; i < numHorde; i++)
		{
			nextPointNums.Add(1);
		}
    }

    void FixedUpdate()
    {
		if (isHorde == false){
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

				//rotate the boss to look  the player
				Vector2 direction = player.position - theBoss.transform.position;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				theBoss.transform.rotation = Quaternion.Euler(0f, 0f, angle);
			}
		}
		else
		{
			if (theBosses.Count != 0 && !playerDead){
				for (int i =0; i < numHorde; i++)
				{
					Transform nextLoc = bossPoints[nextPointNums[i]];
					if (theBosses.Count >= i+1){
						float distToNext = Vector3.Distance(theBosses[i].transform.position, nextLoc.position);
						if (distToNext <= 1f)
						{
							nextPointNums[i] ++;
							if (nextPointNums[i] >= bossPoints.Length)
							{
								nextPointNums[i] = bossPoints.Length -2;
							}
							nextLoc= bossPoints[nextPointNums[i]];
						}
						float newspeed = Random.Range(speed/4, speed *4);
						theBosses[i].transform.position = Vector3.MoveTowards(theBosses[i].transform.position, nextLoc.position, newspeed);
					}
				}
			}
		}
    }


	public void SpawnBoss()
	{
		if (isHorde == false){
		Debug.Log("spwning boss");
		theBoss = Instantiate(bossPrefab, bossPoints[0].position, Quaternion.identity);
		}
		else
		{
			StartCoroutine(SpawnHorde());
		}
		cameraShake.ShakeCamera(1f, 0.3f);
	}

	public void PlayerDead()
	{
		playerDead = true;
	}

	IEnumerator SpawnHorde()
	{
		for (int i = 0; i < numHorde; i++)
		{
			//instaniate new boss, add to boss list
			GameObject newBoss = Instantiate(bossPrefab, bossPoints[0].position, Quaternion.identity);
			theBosses.Add(newBoss);
			yield return new WaitForSeconds(0.2f);
		}
	}


}
