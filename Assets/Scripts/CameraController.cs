using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX = 5f;
    public float sensY = 5f;

    private float xRotation = 0f;
    private Transform playerBody;

    private Vector3 stumbleRotation;
    

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
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f) * Quaternion.Euler(stumbleRotation);


        // Rotate player horizontally
        playerBody.Rotate(Vector3.up * mouseX);


    }

    public void StumbleDrift(Vector3 stumbleModifier)
    {
        stumbleRotation = Vector3.Lerp(stumbleRotation, new Vector3(stumbleModifier.z * 5f, stumbleModifier.x * 5f, stumbleModifier.x * 5f), 2f * Time.deltaTime);
    }
}

