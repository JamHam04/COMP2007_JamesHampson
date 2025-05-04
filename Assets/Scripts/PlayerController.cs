using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float sprintSpeed = 6f;
    public float currentSpeed;
    public float gravity = 9.81f;
    public float jumpForce = 5f;
    public float stumbleAmount = 5f;
    private bool isOnGround = true;

    // Stamina
    public float maxStamina = 3f;
    public float currentStamina = 3f;
    public bool sprinting;
    public Image staminaBar;

    // Respawn Locations
    public Vector3 riverRespawn = new Vector3(93, 9, 44);
    public Vector3 carRespawn = new Vector3(163, 15, 44);
    public Vector3 respawnRotation = new Vector3(0, 90, 0);


    private Vector3 moveDirection;
    private Vector3 stumbleModifier;

    public int lives = 3;
    public List<Image> livesIcons;

    private Rigidbody playerRb;
    private CameraController cameraController;

    Animator playerAnimator;
    Animator canvasAnimator;
    Animator sunlightAnimator;

    public bool isStartingUp = true;
    private bool isSleeping = false;
    public GameObject wakeUpPrompt;

    private float timer;
    public TextMeshProUGUI timerCount;

    public AudioSource jumpSound;
    private AudioSource gameMusic;
    public EndGame endGame;



    void Start()
    {
        // Set components
        stumbleAmount = DifficultySettings.stumbleAmount;
        maxStamina = DifficultySettings.maxStamina;       
        currentStamina = DifficultySettings.currentStamina; 
        playerRb = GetComponent<Rigidbody>();
        jumpSound = GetComponent<AudioSource>();
        gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
        playerRb.freezeRotation = true;

        cameraController = GetComponentInChildren<CameraController>();

        // Start animation
        playerAnimator = GetComponent<Animator>();
        canvasAnimator = GameObject.Find("Canvas/EyeClose").GetComponent<Animator>();
        sunlightAnimator = GameObject.Find("Directional Light").GetComponent<Animator>();

        Invoke("Sleep", 2f);

    }



    // Close eyes at start of game
    public void Sleep()
    {
        canvasAnimator.SetTrigger("Sleep");
        Invoke("Asleep", 6f);
    }

    // Delay for eye closing animation to play at start
    public void Asleep()
    {
        isSleeping = true;
        wakeUpPrompt.SetActive(true);
    }

    // Start sit up animation
    public void StandUp()
    {
        playerAnimator.SetTrigger("WakeUp");

        Invoke("WokenUp", 3f);
    }

    // Allow movement once woken up
    public void WokenUp()
    {
        isStartingUp = false;
        cameraController.isStartingUp = false;

        // Activate Lives
        foreach (Image life in livesIcons)
        {
            life.gameObject.SetActive(true); 
        }

        staminaBar.gameObject.SetActive(true);
        gameMusic.Play();

    }



    void Update()
    {
        if (Input.GetKey(KeyCode.E) && isSleeping == true)
        {
            sunlightAnimator.SetTrigger("Night"); // Set time to night
            canvasAnimator.SetTrigger("Wake"); // Start eye open animation
            Invoke("StandUp", 6f); // Start sit up animation
            wakeUpPrompt.SetActive(false);
            isSleeping = false;
        }

        // Adjust speed based on sprinting
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            currentSpeed = sprintSpeed; // Sprint speed
            sprinting = true;
            currentStamina -= Time.deltaTime;
        }
        else
        {
            currentSpeed = speed; // Normal speed
            sprinting = false;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                currentStamina += Time.deltaTime * 0.5f;
            }
        }

        if (currentStamina >= maxStamina - 0.01f)
        {
            staminaBar.gameObject.SetActive(false);
        }
        else
        {
            staminaBar.gameObject.SetActive(true);
        }

        staminaBar.fillAmount = currentStamina / maxStamina; // Update stamina bar
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Keep stamina from going over max



        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !isStartingUp)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z); // Jump
            isOnGround = false; // Player is in air
            jumpSound.Play();
        }

        // Get input axis
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        Vector3 stumbleRange;

        if (isOnGround)
        {

            if (x != 0 || z != 0)
            {
                // Calculate stumble range when moving
                stumbleRange = new Vector3(
                    Random.Range(-stumbleAmount, stumbleAmount) * currentSpeed / 2f, // More on X
                    0,
                    Random.Range(-stumbleAmount, stumbleAmount) * currentSpeed / 4f // Less on Z 
                );
            }
            else
            {
                // Calculate stumble range when standing still
                stumbleRange = new Vector3(
                    Random.Range(-stumbleAmount, stumbleAmount),
                    0,
                    Random.Range(-stumbleAmount, stumbleAmount)
                );
            }

            // Set smooth stumble modifier
            stumbleModifier = Vector3.Lerp(stumbleModifier, stumbleRange, 1f * Time.deltaTime);
        }

        // Timer
        if (isStartingUp == false)
        {
            timerCount.gameObject.SetActive(true);
            timer += Time.deltaTime;

            // Convert to int
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerCount.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Show in 00:00 format
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When player hits the ground, mark as grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        // When player hits water
        if (collision.gameObject.CompareTag("Water"))
        {
            RiverTeleport();
        }

        // When player hit by car
        if (collision.gameObject.CompareTag("Car"))
        {
            CarTeleport();
        }


    }

    private void RiverTeleport()
    {
        deductLife();

        teleportPlayer(riverRespawn);
    }

    private void CarTeleport()
    {
        deductLife();

        teleportPlayer(carRespawn);

    }

    // Teleport player to respawn at section
    private void teleportPlayer(Vector3 spawnLocation)
    {
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        transform.position = spawnLocation;
        transform.rotation = Quaternion.Euler(respawnRotation);
    }

    private void deductLife()
    {
        // Subtract life
        lives--;

        // Update life counter
        for (int i = 0; i < livesIcons.Count; i++)
        {
            livesIcons[i].enabled = i < lives;
        }

        // End game when running out of lives
        if (lives <= 0)
        {
            endGame.GameOver();
            return;
        }
    }


    void FixedUpdate()
    {
        if (isStartingUp) return;

        // Get player movement input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        // Calculate movement direction and apply stumble modifier
        moveDirection = transform.right * (x + stumbleModifier.x) + transform.forward * (z + stumbleModifier.z);
        moveDirection.y = 0; // Keep the movement on the horizontal plane

        // Apply movement, handling gravity automatically through Rigidbody
        Vector3 velocity = new Vector3(moveDirection.x * currentSpeed, playerRb.velocity.y, moveDirection.z * currentSpeed);
        playerRb.velocity = velocity;

        // Apply stumble drift to camera 
        cameraController.StumbleDrift(stumbleModifier);


    }
}