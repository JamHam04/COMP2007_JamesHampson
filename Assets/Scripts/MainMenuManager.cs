using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject tutorialPanel;

    public void Start()
    {
        // Unlock cursor to select options
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void LoadEasyDifficulty()
    {
        DifficultySettings.stumbleAmount = 2.5f;
        DifficultySettings.maxStamina = 5f;
        DifficultySettings.currentStamina = 5f;
        SceneManager.LoadScene("MainGame");
    }

    public void LoadNormalDifficulty()
    {
        DifficultySettings.stumbleAmount = 5f;
        DifficultySettings.maxStamina = 3f;
        DifficultySettings.currentStamina = 3f;
        SceneManager.LoadScene("MainGame");
    }
    public void LoadHardDifficulty()
    {
        DifficultySettings.stumbleAmount = 7.5f;
        DifficultySettings.maxStamina = 1f;
        DifficultySettings.currentStamina = 1f;
        SceneManager.LoadScene("MainGame");
    }

    public void showTutorial ()
    {
        tutorialPanel.SetActive(true);
    }

    public void hideTutorial () 
    {  
        tutorialPanel.SetActive(false); 
    }
}
