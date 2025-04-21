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




    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;

        cameraController = GetComponentInChildren<CameraController>();
    }

    void Update()
    {
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

        if (x != 0 || z != 0)
        {
            stumbleRange = new Vector3(
                Random.Range(-stumbleAmount, stumbleAmount) * currentSpeed,
                0,
                Random.Range(-stumbleAmount, stumbleAmount) * currentSpeed
            );
        } else
        {
            // Calculate random "Stumble" movement (to be applied in FixedUpdate)
            stumbleRange = new Vector3(
                Random.Range(-stumbleAmount, stumbleAmount),
                0,
                Random.Range(-stumbleAmount, stumbleAmount)
            );
        }



        // Smoothly interpolate the stumble modifier
        stumbleModifier = Vector3.Lerp(stumbleModifier, stumbleRange, 1f * Time.deltaTime);
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
        // Subtract life
        lives--;

        if (lives <= 0)
        {
            GameOver();
            return;
        }
        // Move player and reset velocity
        playerRb.velocity = Vector3.zero; 
        playerRb.angularVelocity = Vector3.zero;
        transform.position = riverRespawn;
        transform.rotation = Quaternion.Euler(respawnRotation);
    }

    private void CarTeleport()
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

        // Move player and reset velocity
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        transform.position = carRespawn;
        transform.rotation = Quaternion.Euler(respawnRotation);
    }

    public void GameOver()
    {
        print("Game Lose");
    }

    void FixedUpdate()
    {
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
