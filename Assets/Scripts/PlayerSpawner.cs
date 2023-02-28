using UnityEngine;

namespace BreakBricks2D
{
    public class PlayerSpawner : MonoBehaviour
    {
        private GameObject playerParent;
        private GameObject playerInstance;

        [Header("References")]
        [SerializeField] private GameObject playerPrefab;

        [Header("Parameters")]
        [SerializeField] private float respawnTimer = 3.0f; // time to wait before respawning
        [SerializeField] private int numLives = 3;
        private bool lifeLost;

        public int NumLives { get { return numLives; } }

        private void Awake()
        {
            playerParent = GameObject.FindWithTag("PlayerParent");
            playerInstance = Instantiate(playerPrefab, transform.position, Quaternion.identity, playerParent.transform);
        }

        private void Update()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            if(playerInstance == null && numLives > 0)
            {
                LoseLive(); // decrease player lives by 1
                respawnTimer -= Time.deltaTime;

                if(respawnTimer <= 0)
                {
                    respawnTimer = 3.0f; // reset respawn timer

                    if(numLives > 0)
                    {
                        lifeLost = false; // reset
                        playerInstance = Instantiate(playerPrefab, transform.position, Quaternion.identity, playerParent.transform); // respawn player
                    }
                }
            }
        }

        private void LoseLive()
        {
            if(lifeLost == false)
            {
                numLives--; // decrease player lives by 1
                lifeLost = true; // prevent the player from losing multiple lives at once
            }
        }
    }
}