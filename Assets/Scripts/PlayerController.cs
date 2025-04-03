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

        // Calculate random "Stumble" movement (to be applied in FixedUpdate)
        Vector3 stumbleRange = new Vector3(
            Random.Range(-stumbleAmount, stumbleAmount),
            0,
            Random.Range(-stumbleAmount, stumbleAmount)
        );

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

        // Apply stumble drift to camera (now in LateUpdate for smoother rotation)
        cameraController.StumbleDrift(stumbleModifier);
    }
}
