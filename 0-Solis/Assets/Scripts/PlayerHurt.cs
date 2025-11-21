using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerHurt: MonoBehaviour {

      //public Animator animator;
      public Rigidbody2D rb2D;
      public AudioSource audioSource;

        public AudioSource[] hurtSources;

      void Start(){
           //animator = gameObject.GetComponentInChildren<Animator>();
           rb2D = transform.GetComponent<Rigidbody2D>();           
      }

      public void playerHit(){
            //animator.SetTrigger ("GetHurt");
      if (hurtSources.Length > 0)
        {
            int index = Random.Range(0, hurtSources.Length);
            hurtSources[index].Play();
        }
    
      }

      public void playerDead(){
            rb2D.isKinematic = true;
            //animator.SetTrigger ("Dead");
      }
}