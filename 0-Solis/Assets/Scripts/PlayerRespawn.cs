using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameHandler gameHandler;
    public Transform pSpawn;

    void Start()
    {
        gameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
    }

    public void SetSpawnPoint(Transform newSpawn)
    {
        pSpawn = newSpawn;
        Debug.Log("New spawn point set: " + pSpawn.name);
    }

    void Update()
    {
        if (pSpawn != null && GameHandler.playerHealth <= 0 && GameHandler.Lives > 0)
        {
            transform.position = new Vector3(pSpawn.position.x, pSpawn.position.y, transform.position.z);
            GameHandler.playerHealth = gameHandler.StartPlayerHealth;
        }
    }
}