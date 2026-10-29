using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor; // Needed to stop play mode in Editor
#endif

public class GameOverMenu : MonoBehaviour
{
    // Go to Main Menu Scene
    public void RestartGame()
    {
        SceneManager.LoadScene("Main Menu (Mobile)");
    }

    // Quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Stop play mode in the Editor
        EditorApplication.isPlaying = false;
#else
            // Quit the built application
            Application.Quit();
#endif
    }

    void Update()
    {
        // Keyboard shortcuts
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }
}
