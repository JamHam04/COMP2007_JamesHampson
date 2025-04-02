using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 10f;
    public float currentSpeed; 
    public float gravity = 9.81f; 
    public float jumpForce = 5f;
    public float stumbleAmount = 2f;
    private bool isOnGround = true;

    private Rigidbody playerRb;
    private Vector3 moveDirection;
    private Vector3 stumbleModifier;

    private CameraController cameraController;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;

        cameraController = GetComponentInChildren<CameraController>();
    }

    private void Update()
    {
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
            isOnGround = false; // Player in air
            // Check prototype 3 for jumping animation and sound fx
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true; // Only jump on ground
        }
    }

    void FixedUpdate()
    {
        // Get player movement input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Calculate random "Stumble" movement
        Vector3 stumbleRange = new Vector3(
            Random.Range(-stumbleAmount, stumbleAmount),
            0,
            Random.Range(-stumbleAmount, stumbleAmount)
        );

        // Calculate stumble modifier smoothly
        stumbleModifier = Vector3.Lerp(stumbleModifier, stumbleRange, 1f * Time.fixedDeltaTime);

        // Calculate movement direction + apply stumble modifier
        moveDirection = transform.right * (x + stumbleModifier.x) + transform.forward * (z + stumbleModifier.z);
        moveDirection.y = 0;

        // Apply movement
        playerRb.velocity = new Vector3(moveDirection.x * currentSpeed, playerRb.velocity.y - (gravity * Time.fixedDeltaTime), moveDirection.z * currentSpeed);

        cameraController.StumbleDrift(stumbleModifier);
    }
}
