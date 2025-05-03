using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject interactPromptUI;
    public TextMeshProUGUI returnToMenuText;
    public TextMeshProUGUI returnToStartText;
    private bool isPlayerNear = false;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    private bool endGame = false;
    private bool gameOver = false;

    Animator canvasAnimator;

    public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        // If the player is by the end door
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            playerController.isStartingUp = true;
            // Fade to black
            canvasAnimator = GameObject.Find("Canvas/EyeClose").GetComponent<Animator>();
            canvasAnimator.SetTrigger("Sleep");
            Invoke("showReturn", 2f); // Show end text after screen is fully faded
        }

        // Return to Menu
        if (Input.GetKeyDown(KeyCode.E) && endGame)
        {
            SceneManager.LoadScene("MainMenu");
        }

        // Return to Start
        if (Input.GetKeyDown(KeyCode.E) && gameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Display return to menu text
    public void showReturn()
    {
        returnToMenuText.gameObject.SetActive(true);
        winText.gameObject.SetActive(true);
        endGame = true; // Enable ability to return to menu
    }

    // Show Enter text
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactPromptUI.SetActive(true);
        }
    }

    // Hide Enter text
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactPromptUI.SetActive(false);
        }
    }

    
    public void GameOver()
    {
        playerController.isStartingUp = true;
        
        canvasAnimator = GameObject.Find("Canvas/EyeClose").GetComponent<Animator>();
        canvasAnimator.SetTrigger("Sleep");
        
        Invoke("showRestart", 2f); 
    }

    // Show restart game text
    private void showRestart()
    {
        gameOver = true;
        loseText.gameObject.SetActive(true);
        returnToStartText.gameObject.SetActive(true);
    }
}
