using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [Header("Audio Manager")]
    public AudioManager audioManager;        // The AudioManager
    public string enterTrackName = "BossTrack"; // Track to play when player enters
    //public string exitTrackName = "NormalTrack"; // Track to play when player exits (optional)

    private bool hasTriggered = false;

void Start()
	{
		audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            Debug.Log("Player entered music trigger");

            if (audioManager != null)
                audioManager.PlayTrackByName(enterTrackName);
        }
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
        //if (other.CompareTag("Player") && hasTriggered)
      //  {
    //        hasTriggered = false;
  //          Debug.Log("Player exited music trigger");
//
          //  if (audioManager != null && !string.IsNullOrEmpty(exitTrackName))
        //        audioManager.PlayTrackByName(exitTrackName);
      //  }
    //}
    

    // Optional: Reset trigger manually
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
