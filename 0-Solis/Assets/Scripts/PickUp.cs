using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour{

      public GameHandler gameHandler;
      //public playerVFX playerPowerupVFX;
      public bool isHealthPickUp = false;
      public bool isSpeedBoostPickUp = false;
      public bool isTokenPickUp = true;

      public int healthBoost = 50;
      public float speedBoost = 2f;
      public float speedTime = 2f;
      //public int tokenBoost = 20;

	bool canPickUp = false;
	public GameObject MSG_PressE;


      void Start(){
            gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
            //playerPowerupVFX = GameObject.FindWithTag("Player").GetComponent<playerVFX>();
			MSG_PressE.SetActive(false);
      }

	void Update()
	{
		if (canPickUp && Input.GetKeyDown("e"))
		{
			//GetComponent< AudioSource>().Play();
			StartCoroutine(DestroyThis());
			if (isTokenPickUp == true) {
				GameHandler.GainTokens(20);
				//playerPowerupVFX.powerup();
			}
		}
	}

/*
      public void OnTriggerEnter2D (Collider2D other){
            if (other.gameObject.tag == "Player"){
                  GetComponent<Collider2D>().enabled = false;
                  //GetComponent< AudioSource>().Play();
                  StartCoroutine(DestroyThis());

                  //if (isHealthPickUp == true) {
                        //gameHandler.playerGetHit(healthBoost * -1);
                        //playerPowerupVFX.powerup();
                  //}

                  //if (isSpeedBoostPickUp == true) {
                        //other.gameObject.GetComponent<PlayerMove>().speedBoost(speedBoost, speedTime);
                        //playerPowerupVFX.powerup();
                  //}
                   if (isTokenPickUp == true) {
                        GameHandler.GainTokens(20);
                        //playerPowerupVFX.powerup();
                   }     
            }
      }
*/
      IEnumerator DestroyThis(){
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);
      }

	public void enableBatteryForPickup()
	{
		canPickUp = true;
		MSG_PressE.SetActive(true);
	}

	public void disableBatteryPickup(){
		canPickUp = false;
		MSG_PressE.SetActive(false);
	}

}