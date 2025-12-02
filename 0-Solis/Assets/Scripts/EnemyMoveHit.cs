using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyMoveHit : MonoBehaviour {

       private Animator anim;
       private Rigidbody2D rb2D;
	   private bool FaceRight = false; // determine which way enemy is facing.
       public float speed = 4f;
       private Transform target;
       public int damage = 10;

       public int EnemyLives = 3;
       private GameHandler gameHandler;

       public float attackRange = 10;
       public bool isAttacking = false;
       private float scaleX;

       public GameObject bloodSplatter;

	//retreat variables when hit by flashlight:
	public float retreatDistance = 12f;

	void Start () {
              anim = GetComponentInChildren<Animator> ();
              rb2D = GetComponent<Rigidbody2D> ();
              scaleX = gameObject.transform.localScale.x;

              if (GameObject.FindGameObjectWithTag ("Player") != null) {
                     target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
              }

              if (GameObject.FindWithTag ("GameHandler") != null) {
                  gameHandler = GameObject.FindWithTag ("GameHandler").GetComponent<GameHandler> ();
              }
	}

	void Update () {
		if (target != null){
			float distToPlayer = Vector3.Distance(transform.position, target.position);

	//retreat
		if (GetComponent<EnemyMeleeDamage>().isFlashlit== true){
			if (distToPlayer <= retreatDistance)
			{
				//Debug.Log ("I am moving away from scary flashlight!: " + distToPlayer);
				transform.position = Vector2.MoveTowards (transform.position, target.position, speed *-1.2f * Time.deltaTime);
				anim.SetBool("Walk", true);
				if (//if player is on the right, and i am facing left
				(target.position.x > gameObject.transform.position.x && !FaceRight) ||
				//if player is on the left, and i am facing right
				(target.position.x < gameObject.transform.position.x && FaceRight))
				{
					FlipEnemy();	
				}
			}
		}

	//move towards:
		else if ((distToPlayer <= attackRange)&&(GetComponent<EnemyMeleeDamage>().isHurt== false)){
			transform.position = Vector2.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
			anim.SetBool("Walk", true);
			//flip enemy to face player direction. Wrong direction? Swap the * -1.
			if (//if player is on the right, and i am facing left
				(target.position.x > gameObject.transform.position.x && !FaceRight) ||
				//if player is on the left, and i am facing right
				(target.position.x < gameObject.transform.position.x && FaceRight))
				{
					FlipEnemy();	
				}
		}
		else { anim.SetBool("Walk", false);}
		}
	}


	void FlipEnemy()
	{
		FaceRight = !FaceRight;

		// Multiply player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		/*
		if (target.position.x > gameObject.transform.position.x){
			gameObject.transform.localScale = new Vector2(scaleX, gameObject.transform.localScale.y);
		} else {
			gameObject.transform.localScale = new Vector2(scaleX * -1, gameObject.transform.localScale.y);
		}
		*/
	}

	public void OnCollisionEnter2D(Collision2D other){
              if (other.gameObject.tag == "Player") {
                     isAttacking = true;
                     anim.SetBool("Bite", true);
                     gameHandler.playerGetHit(damage);
                     Instantiate(bloodSplatter, other.gameObject.transform.position, Quaternion.identity);
              }
	}

	public void OnCollisionExit2D(Collision2D other){
              if (other.gameObject.tag == "Player") {
                     isAttacking = false;
                     anim.SetBool("Bite", false);
              }
	}

       //DISPLAY the range of enemy's attack when selected in the Editor
	void OnDrawGizmosSelected(){
		Gizmos.DrawWireSphere(transform.position, attackRange);
		Gizmos.DrawWireSphere(transform.position, retreatDistance);
	}
}