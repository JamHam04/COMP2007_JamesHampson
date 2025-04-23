using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX = 5f;
    public float sensY = 5f;

    // Camera Rotation
    private float xRotation = 0f;
    private float yRotation = 0f;

    // Target Camera Rotation (For Lerp effect)
    private float cameraDrunkSpeed = 5f;
    private float targetXRotation = 0f;
    private float targetYRotation = 0f;

    public Transform playerBody; 

    private Vector3 stumbleRotation;

    public bool isStartingUp = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject player = GameObject.FindWithTag("Player");
        playerBody = player.transform;


        // Initialize rotation seperately
        Vector3 initialRotation = transform.localEulerAngles;
        xRotation = initialRotation.x;
        yRotation = initialRotation.y;
    }

    void LateUpdate()
    {
        if (isStartingUp) return;
        // Player input
        float mouseX = Input.GetAxis("Mouse X") * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * sensY;

        // Store target
        targetXRotation -= mouseY;
        targetYRotation += mouseX;

        // Rotate vertically up to 90 degrees each direction
        targetXRotation = Mathf.Clamp(targetXRotation, -90f, 90f);

        // Smoothly lag behind
        xRotation = Mathf.Lerp(xRotation, targetXRotation, Time.deltaTime * cameraDrunkSpeed);
        yRotation = Mathf.Lerp(yRotation, targetYRotation, Time.deltaTime * cameraDrunkSpeed);

        // Apply rotation to camera and player
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f) * Quaternion.Euler(stumbleRotation);
        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void StumbleDrift(Vector3 stumbleModifier)
    {

        stumbleRotation = Vector3.Lerp(stumbleRotation, new Vector3(-stumbleModifier.z * 5f, 0f, -stumbleModifier.x * 5f), 2f * Time.deltaTime);
    }
}
