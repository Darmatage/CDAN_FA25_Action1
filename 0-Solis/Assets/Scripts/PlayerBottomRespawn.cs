using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomRespawn : MonoBehaviour
{
    public GameHandler gameHandler;
    public Transform playerPos;
    public Transform pSpawnFall;
    public int damage = 10;

    private bool canKill = true;   // <-- changed

    [Header("Cooldown")]
    public float killCooldown = 1f; // 1 second wait between kill checks

    void Start()
    {
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    void Update()
    {
        if (playerPos == null) return;

        pSpawnFall = playerPos.GetComponent<PlayerRespawn>().pSpawn;

        if (canKill && transform.position.y >= playerPos.position.y)
        {
            Debug.Log("Kill floor triggered");
            canKill = false;
            gameHandler.playerDies();

            StartCoroutine(KillCooldownTimer());
        }
    }

    IEnumerator KillCooldownTimer()
    {
        yield return new WaitForSeconds(killCooldown);
        canKill = true; // <-- ready to kill again after 1 sec
    }
}
