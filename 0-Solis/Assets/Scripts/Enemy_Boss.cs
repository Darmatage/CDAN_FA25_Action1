using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "Player")
		{
			GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>().playerGetHit(500);
			GameObject.FindWithTag("BossSystem").GetComponent<Enemy_BossSystem>().PlayerDead();
		}
    }
}
