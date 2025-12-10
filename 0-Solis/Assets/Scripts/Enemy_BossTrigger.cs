using UnityEngine;
using System.Collections.Generic;

public class Enemy_BossTrigger : MonoBehaviour
{
    public Enemy_BossSystem bossSystem;
    [HideInInspector] public bool BossExists = false;
    private bool hasTriggered = false;

    [Header("Audio Manager")]
    public AudioInterrupt audioManager; // single AudioInterrupt with multiple tracks as children
    public string bossTrackName = "BossTrack";      // name of the boss track child
    public string normalTrackName = "NormalTrack";  // name of the normal track child

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player hit trigger");

            // Spawn the boss
            bossSystem.SpawnBoss();
            BossExists = true;
            hasTriggered = true;

            // Play boss music via AudioManager
            if (audioManager != null)
                audioManager.PlayTrackByName(bossTrackName);
        }
    }

    public void ResetBosses()
    {
        hasTriggered = false;

        if (bossSystem != null)
        {
            // Destroy single boss
            if (bossSystem.theBoss != null)
            {
                Destroy(bossSystem.theBoss);
                bossSystem.theBoss = null;
            }

            // Destroy all horde bosses
            if (bossSystem.theBosses != null)
            {
                foreach (GameObject boss in bossSystem.theBosses)
                    if (boss != null) Destroy(boss);

                bossSystem.theBosses.Clear();
            }
        }

        BossExists = false;
        Debug.Log("Bosses reset and BossExists = false");

        // Play normal music via AudioManager
        if (audioManager != null)
            audioManager.PlayTrackByName(normalTrackName);
    }
}
