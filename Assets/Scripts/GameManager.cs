using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreakBricks2D
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float restartGameTimer = 5.0f; // seconds

        public float TimeToRestart { get { return restartGameTimer; } }

        private void Update()
        {
            UserInput();
        }

        private void UserInput()
        {
            if (Input.GetKeyDown(KeyCode.Backspace)) // press Backspace to restart the game
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
            if(Input.GetKeyDown(KeyCode.Escape)) // press Escape to quit the game
            {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
        }

        private IEnumerator RestartingGame()
        {
            yield return new WaitForSeconds(restartGameTimer);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void RestartGame()
        {
            StartCoroutine(RestartingGame());
        }
    }
}
