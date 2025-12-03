using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyMoveHit : MonoBehaviour {

       private Animator anim;
       private Rigidbody2D rb2D;
	   private bool FaceRight = false; 
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

       private Collider2D myCollider;   // <-- needed for ignoring collisions

	void Start () {
              anim = GetComponentInChildren<Animator> ();
              rb2D = GetComponent<Rigidbody2D> ();
              scaleX = gameObject.transform.localScale.x;
              myCollider = GetComponent<Collider2D>();   // <-- cache collider

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
				transform.position = Vector2.MoveTowards (transform.position, target.position, speed *-1.2f * Time.deltaTime);
				anim.SetBool("Walk", true);

				if ((target.position.x > transform.position.x && !FaceRight) ||
				    (target.position.x < transform.position.x && FaceRight))
				{
					FlipEnemy();	
				}
			}
		}

	//move towards:
		else if ((distToPlayer <= attackRange)&&(GetComponent<EnemyMeleeDamage>().isHurt== false)){
			transform.position = Vector2.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
			anim.SetBool("Walk", true);

			if ((target.position.x > transform.position.x && !FaceRight) ||
			    (target.position.x < transform.position.x && FaceRight))
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

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void OnCollisionEnter2D(Collision2D other){

		// ✅ IGNORE CAR collisions
		if (other.gameObject.CompareTag("Car"))
		{
			Physics2D.IgnoreCollision(myCollider, other.collider, true);
			return; // do NOT continue with normal collision logic
		}

        // Normal collision with player
        if (other.gameObject.tag == "Player") {
            isAttacking = true;
            anim.SetBool("Bite", true);
            gameHandler.playerGetHit(damage);
            Instantiate(bloodSplatter, other.transform.position, Quaternion.identity);
        }
	}

	public void OnCollisionExit2D(Collision2D other){

        // (Not needed) but kept if Car exits collision
        if (other.gameObject.CompareTag("Car"))
		{
			Physics2D.IgnoreCollision(myCollider, other.collider, true);
		}

        if (other.gameObject.tag == "Player") {
            isAttacking = false;
            anim.SetBool("Bite", false);
        }
	}

	void OnDrawGizmosSelected(){
		Gizmos.DrawWireSphere(transform.position, attackRange);
		Gizmos.DrawWireSphere(transform.position, retreatDistance);
	}
}
