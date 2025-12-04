using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerBottomRespawn : MonoBehaviour {

    public GameHandler gameHandler;
    public Transform playerPos;
    public Transform pSpawnFall;
    public int damage = 10;

    private bool hasKilled = false;   // <-- NEW

    void Start() {
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    void Update() {
        if (playerPos == null) return;

        // update spawn if checkpoints change
        pSpawnFall = playerPos.GetComponent<PlayerRespawn>().pSpawn;

        if (!hasKilled && transform.position.y >= playerPos.position.y) {
            hasKilled = true; // <-- prevent more kills
            Debug.Log("Kill floor triggered once");
            gameHandler.playerDies();
        }
    }

    // ---- RESET ON LEVEL RELOAD OR RESPAWN ----
    public void ResetKill()
    {
        hasKilled = false;
    }
}