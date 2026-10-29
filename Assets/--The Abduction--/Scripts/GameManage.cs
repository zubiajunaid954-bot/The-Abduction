using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject MainKey, maintrigger, mainparticl;

    // NEW: Instruction Panel reference
    public GameObject InstructionPanel;

    // Pause Game
    public void pause()
    {
        Time.timeScale = 0.0f;
        PausePanel.SetActive(true);
    }

    // Resume Game
    public void resume()
    {
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);
        InstructionPanel.SetActive(false); // also hide instructions if open
    }

    // Go to Main Menu
    public void menu()
    {
        SceneManager.LoadScene(0);
    }

    // ?? NEW: Show Instructions
    public void ShowInstructions()
    {
        InstructionPanel.SetActive(true);
    }

    // ?? NEW: Hide Instructions
    public void HideInstructions()
    {
        InstructionPanel.SetActive(false);
    }
}
