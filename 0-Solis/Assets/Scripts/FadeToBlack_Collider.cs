using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeToBlack_Collider : MonoBehaviour
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
			GameObject.FindWithTag("FadeToBlack").GetComponent<FadeToBlack_Canvas>().ActivateFade();
			Debug.Log("Player is fading away to black");
		}
    }

}
