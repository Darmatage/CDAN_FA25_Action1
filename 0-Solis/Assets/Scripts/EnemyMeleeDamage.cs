using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyMeleeDamage : MonoBehaviour {
       private Renderer rend;
       private Animator anim;
       public GameObject healthLoot;
       public int maxHealth = 6;
       public int currentHealth;


	//public retreat variable when hit by flashlight:
	public bool isFlashlit = false;


	public bool isHurt = false;

       void Start(){
              rend = GetComponentInChildren<Renderer> ();
              anim = GetComponentInChildren<Animator> ();
              currentHealth = maxHealth;
       }

       public void TakeDamage(int damage){
			Debug.Log("enemy hurt for: " + damage);
			currentHealth -= damage;
			isHurt = true;
			rend.material.color = new Color(2.4f, 0.9f, 0.9f, 1f);
			//rend.material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			StartCoroutine(ResetColor());
			
			if (currentHealth <= 0){
				Die();
			}
			else
			{
				anim.SetTrigger ("getHurt");
			} 
       }

	public void Flash_Lit(){
		isFlashlit = true;
		rend.material.color = new Color(0.9f, 0.9f, 2.4f, 1f);
	}
	public void Flash_Unlit(){
		isFlashlit = false;
		rend.material.color = Color.white;
	}

       void Die(){
              Instantiate (healthLoot, transform.position, Quaternion.identity);
			  
              GetComponent<Collider2D>().enabled = false;
			  GetComponent<Rigidbody2D>().isKinematic = true;
              StartCoroutine(Death());
       }

       IEnumerator Death(){
			anim.SetTrigger ("Die");
			yield return new WaitForSeconds(1f);
			anim.SetBool ("isDead", true);
			Debug.Log("You Killed a baddie. You deserve loot!");
			yield return new WaitForSeconds(5f);
			Destroy(gameObject);
       }

       IEnumerator ResetColor(){
              yield return new WaitForSeconds(0.5f);
			  isHurt = false;
              rend.material.color = Color.white;
       }
}
