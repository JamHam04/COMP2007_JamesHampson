using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseScreen;
    private bool gamePaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Either pause or unpause the game
            if (gamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        gamePaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f; // Stop game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f; // Resume game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Reset time 
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Load main menu
    }
}
