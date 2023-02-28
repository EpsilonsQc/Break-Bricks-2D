using UnityEngine;

namespace BreakBricks2D
{
    public class SpecialAbility : MonoBehaviour
    {
        private PhysicalWorld physicalWorld;
        private PhysicalBody physicalBody;
        private GameManager gameManager;
        private DamageHandler enemyDamageHandler;
        private GUIManager guiManager;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private bool isSystem; // special system brick
        [SerializeField] private bool isEnemy; // special AI brick
        private bool isEnemyDead; // check if the enemy is dead

        private void Awake()
        {
            physicalWorld = FindObjectOfType<PhysicalWorld>();
            gameManager = FindObjectOfType<GameManager>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            enemyDamageHandler = GameObject.Find("Enemy_AI").GetComponent<DamageHandler>();
            guiManager = GameObject.Find("GUI_Manager").GetComponent<GUIManager>();

            InvokeRepeating("EnemyStatus", 0.0f, 0.5f); // check if the enemy is dead every 0.5 seconds
        }

        private void EnemyStatus()
        {
            if (isEnemy && enemyDamageHandler.Health <= 0)
            {
                isEnemyDead = true;
                isEnemy = false; // the brick is no longer a "Disable Enemy special brick"
                spriteRenderer.color = Color.white; // change the color of the brick
            }
        }

        private void DisableEnemy()
        {
            if(isEnemy && !isEnemyDead) // if colliding with disable AI enemy brick and the AI enemy is not dead
            {
                guiManager.IsEnemyDisable = true;
                enemyDamageHandler.Health = 0; // kill the AI
            }
        }

        private void IsSystemWin()
        {
            if(isSystem)  // if colliding with system brick
            {
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("isDestructible").Length; i++)
                {
                    physicalBody = GameObject.FindGameObjectsWithTag("isDestructible")[i].GetComponent<PhysicalBody>();
                    physicalWorld.BodyList.Remove(physicalBody); // remove the object from the list
                    Destroy(GameObject.FindGameObjectsWithTag("isDestructible")[i]); // destroy the object
                }

                for (int i = 0; i < GameObject.FindGameObjectsWithTag("isPlayerMissile").Length; i++)
                {
                    physicalBody = GameObject.FindGameObjectsWithTag("isPlayerMissile")[i].GetComponent<PhysicalBody>();
                    physicalWorld.BodyList.Remove(physicalBody); // remove the object from the list
                    Destroy(GameObject.FindGameObjectsWithTag("isPlayerMissile")[i]); // destroy the object
                }

                for (int i = 0; i < GameObject.FindGameObjectsWithTag("isEnemyMissile").Length; i++)
                {
                    physicalBody = GameObject.FindGameObjectsWithTag("isEnemyMissile")[i].GetComponent<PhysicalBody>();
                    physicalWorld.BodyList.Remove(physicalBody); // remove the object from the list
                    Destroy(GameObject.FindGameObjectsWithTag("isEnemyMissile")[i]); // destroy the object
                }

                for (int i = 0; i < GameObject.FindGameObjectsWithTag("isEnemy").Length; i++)
                {
                    physicalBody = GameObject.FindGameObjectsWithTag("isEnemy")[i].GetComponent<PhysicalBody>();
                    physicalWorld.BodyList.Remove(physicalBody); // remove the object from the list
                    Destroy(GameObject.FindGameObjectsWithTag("isEnemy")[i]); // destroy the object
                }

                guiManager.IsWin = true; // Win the game GUI message
                gameManager.RestartGame(); // Restart the game by reloading the scene
            }
        }

        public void ActivateSpecialAbility()
        {
            DisableEnemy();
            IsSystemWin();
        }
    }
}
