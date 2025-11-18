using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour{

      public int damage = 1;
      public GameObject hitEffectAnim;
      public float SelfDestructTime = 4.0f;
      public float SelfDestructVFX = 0.5f;
      public GameObject projectileArt;
      public GameObject splatterPrefab;

      void Start(){
           //projectileArt = GetComponentInChildren<SpriteRenderer>();
           StartCoroutine(SelfDestruct());
      }

      //if the bullet hits a collider, play the explosion animation, then destroy the effect and the bullet
      void OnTriggerEnter2D(Collider2D other){
            //if (other.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
                  //gameHandlerObj.playerGetHit(damage);
              //    other.gameObject.GetComponent<EnemyMeleeDamage>().TakeDamage(damage);
            //}
           if (other.gameObject.tag != "Player") {
                  gameObject.GetComponent<BoxCollider2D>().enabled = false;
                  GameObject animEffect = Instantiate (hitEffectAnim, transform.position, Quaternion.identity);
                  projectileArt.SetActive(false);
                  //Destroy (animEffect, 0.5);
                  StartCoroutine(SelfDestructHit(animEffect));
            }
      }

      IEnumerator SelfDestructHit(GameObject VFX){
            //MakeSplat();
            yield return new WaitForSeconds(SelfDestructVFX);
            Destroy (VFX);
            Destroy (gameObject);
      }

      IEnumerator SelfDestruct(){
            yield return new WaitForSeconds(SelfDestructTime);
            Destroy (gameObject);
      }

     
}