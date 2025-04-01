using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX = 5f;
    public float sensY = 5f;

    private float xRotation = 0f;
    private Transform playerBody;

    void Start()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find player
        playerBody = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Player input
        float mouseX = Input.GetAxis("Mouse X") * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * sensY;

        // Rotate vertically up to 90 degrees each direction
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply vertical rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player horizontally
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
