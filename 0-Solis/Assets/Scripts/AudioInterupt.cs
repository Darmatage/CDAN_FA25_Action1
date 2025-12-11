using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

        public AudioSource audioSource;
        private float stopTimestamp = 12.5f;
       
        void Update(){
               // if (Input.GetKeyDown("i")) {
                      //  PlayMusicAtBegin();
               // }
              //  if (Input.GetKeyDown("o")) {
                     //  StopMusic();
              //  }
               // if (Input.GetKeyDown("p")) {
                       // PlayMusicAtTime(stopTimestamp);
               // }
       }

        public void PlayMusicAtBegin(){
                audioSource.time = 0.0f;
                audioSource.Play();
        }

        public void StopMusic(){
                stopTimestamp = audioSource.time;
                Debug.Log("Stopped audio at: " + stopTimestamp);
                audioSource.Stop();
        }

        public void PlayMusicAtTime(float timeStamp){
                if (timeStamp > audioSource.clip.length){
                        return;
                } else {
                        audioSource.time = timeStamp;
                        audioSource.Play();
                }
        }
        public void PlayTrackByName(string trackName)
{
    foreach (Transform child in transform)
    {
        if (child.name == trackName)
        {
            child.gameObject.SetActive(true); // turn on the track
            AudioSource src = child.GetComponent<AudioSource>();
            if (src != null)
            {
                src.time = 0f;
                src.Play();
            }
        }
        else
        {
            AudioSource src = child.GetComponent<AudioSource>();
            if (src != null) src.Stop();
            child.gameObject.SetActive(false); // turn off other tracks
        }
    }
}
}
