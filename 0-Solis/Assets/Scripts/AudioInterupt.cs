using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioInterrupt : MonoBehaviour {

        public AudioSource audioSource;
        private float stopTimestamp = 12.5f;
       
        void Update(){
                if (Input.GetKeyDown("i")) {
                        PlayMusicAtBegin();
                }
                if (Input.GetKeyDown("o")) {
                        StopMusic();
                }
                if (Input.GetKeyDown("p")) {
                        PlayMusicAtTime(stopTimestamp);
                }
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
}