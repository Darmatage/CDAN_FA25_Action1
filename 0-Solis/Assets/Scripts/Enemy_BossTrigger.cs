using UnityEngine;

public class Enemy_BossTrigger : MonoBehaviour
{

	public Enemy_BossSystem bossSystem;
	bool hasBoss = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasBoss = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && !hasBoss)
		{
			Debug.Log("Player hit trigger");
			bossSystem.SpawnBoss();
			hasBoss = true;
		}
	}


}
