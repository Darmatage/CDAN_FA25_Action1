using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
  private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     anim = GetComponentInChildren<Animator>();
     anim.SetBool("Crawl", true);   
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "Player")
		{
      anim.SetBool("Bite", true);
			GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>().playerGetHit(500);
			GameObject.FindWithTag("BossSystem").GetComponent<Enemy_BossSystem>().PlayerDead();
		}
    }
}
