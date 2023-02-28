using UnityEngine;

namespace BreakBricks2D
{
    public class GUIManager : MonoBehaviour
    {
        private GameManager gameManager;
        private GameObject playerSpawner;
        private GameObject playerInstance;
        private GameObject enemyInstance;

        private int playerHealth;
        private int playerLives;
        private int enemyHealth;

        private bool isEnemyDisable;
        private bool isWin;
        private float restartGameTimer;

        public bool IsEnemyDisable { set { isEnemyDisable = value; } }
        public bool IsWin { set { isWin = value; } }

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            restartGameTimer = gameManager.TimeToRestart;

            playerSpawner = GameObject.Find("Player_Spawner");
            enemyInstance = GameObject.Find("Enemy_AI");

            InvokeRepeating("FindPlayerInstance", 0.0f, 0.5f);
        }

        private void Update()
        {
            RestartGameTimer();
        }

        private void OnGUI()
        {
            PlayerStatus();
            EnemyStatus();
            GameStatus();
        }

        private void FindPlayerInstance()
        {
            if(playerInstance == null)
            {
                playerInstance = GameObject.FindWithTag("Player");
            }
        }

        private void PlayerStatus()
        {
            playerLives = playerSpawner.GetComponent<PlayerSpawner>().NumLives;

            // if player is not null
            if(playerInstance != null)
            {
                playerHealth = playerInstance.GetComponent<DamageHandler>().Health;
            }

            if(playerLives > 0 && playerInstance != null)
            {
                GUI.skin.label.fontSize = 36;
                GUI.Label(new Rect(25, 20, 350, 100), "Health: " + playerHealth);
                GUI.Label(new Rect(25, 60, 350, 100), "Life x" + playerLives);
            }
            else if(playerLives > 0 && playerInstance == null)
            {
                GUI.skin.label.fontSize = 36;
                GUI.Label(new Rect(25, 20, 350, 100), "Health: 0");
                GUI.Label(new Rect(25, 60, 350, 100), "Life x" + playerLives);
                GUI.skin.label.fontSize = 100;
                GUI.color = Color.red;
                GUI.Label(new Rect( Screen.width / 2 - 200 , Screen.height / 2 - 450, 500, 150), "You Died!");
            }
            else
            {
                GUI.skin.label.fontSize = 100;
                GUI.color = Color.red;
                GUI.Label(new Rect( Screen.width / 2 - 250 , Screen.height / 2 - 450, 600, 150), "Game Over");
                GUI.skin.label.fontSize = 36;
                GUI.color = Color.white;
                GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 500, 600, 150), "Press BACKSPACE to continue...");
            }
        }

        private void EnemyStatus()
        {
            if(enemyInstance != null)
            {
                enemyHealth = enemyInstance.GetComponent<DamageHandler>().Health;
            }

            if(!isWin && enemyHealth > 0 && enemyInstance != null)
            {
                GUI.skin.label.fontSize = 36;
                GUI.color = Color.red;
                GUI.Label(new Rect(1610, 20, 350, 100), "Enemy health: " + enemyHealth);
            }
            else if(!isWin && !isEnemyDisable)
            {
                GUI.skin.label.fontSize = 36;
                GUI.color = Color.yellow;
                GUI.Label(new Rect(1620, 20, 350, 100), "Enemy is dead !");
            }
            else if(!isWin && isEnemyDisable)
            {
                GUI.skin.label.fontSize = 36;
                GUI.color = Color.yellow;
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 450, 500, 150), "You have disabled the enemy by destroying the AI brick!");
            }
        }

        private void GameStatus()
        {
            if(isWin)
            {
                GUI.skin.label.fontSize = 36;
                GUI.color = Color.green;
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 450, 500, 150), "You have won the game by destroying the system brick!");
                GUI.color = Color.white;
                GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 500, 600, 150), "The game will restart in " + (int)restartGameTimer + " seconds");
            }
        }

        private void RestartGameTimer()
        {
            if(isWin)
            {
                restartGameTimer -= Time.deltaTime; // count down the seconds
            }
        }
    }
}