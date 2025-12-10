using UnityEngine;
using System.Collections.Generic;

public class Enemy_BossTrigger : MonoBehaviour
{
    public Enemy_BossSystem bossSystem;
    [HideInInspector] public bool BossExists = false; // single source of truth
    private bool hasTriggered = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            Debug.Log("Player hit trigger");
            bossSystem.SpawnBoss();
            BossExists = true;
            hasTriggered = true;
        }
    }

    public void ResetBosses()
    {
        hasTriggered = false;  // allow trigger to fire again

        if (bossSystem != null)
        {
            // Destroy single boss if exists
            if (bossSystem.theBoss != null)
            {
                Destroy(bossSystem.theBoss);
                bossSystem.theBoss = null;
            }

            // Destroy all horde bosses
            if (bossSystem.theBosses != null)
            {
                foreach (GameObject boss in bossSystem.theBosses)
                    if (boss != null)
                        Destroy(boss);

                bossSystem.theBosses.Clear();
            }
        }

        BossExists = false;
        Debug.Log("Bosses reset and BossExists = false");
    }
}
