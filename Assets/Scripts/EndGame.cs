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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            print("Game Win");
            playerController.isStartingUp = true;
            canvasAnimator = GameObject.Find("Canvas/EyeClose").GetComponent<Animator>();
            canvasAnimator.SetTrigger("Sleep");
            Invoke("showReturn", 2f);
        }

        if (Input.GetKeyDown(KeyCode.E) && endGame)
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.E) && gameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void showReturn()
    {
        returnToMenuText.gameObject.SetActive(true);
        winText.gameObject.SetActive(true);
        endGame = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactPromptUI.SetActive(true);
        }
    }

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

    private void showRestart()
    {
        gameOver = true;
        loseText.gameObject.SetActive(true);
        returnToStartText.gameObject.SetActive(true);
    }
}
