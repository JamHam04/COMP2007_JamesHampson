using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f;
    public float currentSpeed;
    public float gravity = 9.81f;
    public float jumpForce = 5f;
    public float stumbleAmount = 2f;
    private bool isOnGround = true;

    // Respawn Locations
    public Vector3 riverRespawn = new Vector3(93, 9, 44);
    public Vector3 carRespawn = new Vector3(163, 15, 44);
    public Vector3 respawnRotation = new Vector3(0, 90, 0);


    private Vector3 moveDirection;
    private Vector3 stumbleModifier;

    public int lives = 3;
    public TextMeshProUGUI livesCounter;
    public List<Image> livesIcons;

    private Rigidbody playerRb;
    private CameraController cameraController;

    Animator playerAnimator;
    Animator canvasAnimator;
    Animator sunlightAnimator;

    private bool isStartingUp = true;
    private bool isSleeping = false;
    public GameObject wakeUpPrompt;



    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
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
    }



    void Update()
    {
        if (Input.GetKey(KeyCode.E) && isSleeping == true)
        {
            sunlightAnimator.SetTrigger("Night"); // Set time to night
            canvasAnimator.SetTrigger("Wake"); // Start eye open animation
            Invoke("StandUp", 6f); // Start sit up animation
            wakeUpPrompt.SetActive(false);
        }

        // Adjust speed based on sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed; // Sprint speed
        }
        else
        {
            currentSpeed = speed; // Normal speed
        }

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, jumpForce, playerRb.velocity.z);
            isOnGround = false; // Player is in air
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        Vector3 stumbleRange;

        if (isOnGround)
        {

            if (x != 0 || z != 0)
            {
                stumbleRange = new Vector3(
                    Random.Range(-stumbleAmount, stumbleAmount) * currentSpeed / 2,
                    0,
                    Random.Range(-stumbleAmount, stumbleAmount) * currentSpeed / 2
                );
            }
            else
            {
                stumbleRange = new Vector3(
                    Random.Range(-stumbleAmount, stumbleAmount),
                    0,
                    Random.Range(-stumbleAmount, stumbleAmount)
                );
            }


            stumbleModifier = Vector3.Lerp(stumbleModifier, stumbleRange, 1f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When player hits the ground, mark as grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            RiverTeleport();
        }

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


        livesCounter.text = "Lives: " + lives;
        for (int i = 0; i < livesIcons.Count; i++)
        {
            livesIcons[i].enabled = i < lives;
        }


        if (lives <= 0)
        {
            GameOver();
            return;
        }
    }
    public void GameOver()
    {
        print("Game Lose");
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