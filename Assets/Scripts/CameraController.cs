using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensX = 5f;
    public float sensY = 5f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public Transform playerBody; 

    private Vector3 stumbleRotation;

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
        // Player input
        float mouseX = Input.GetAxis("Mouse X") * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * sensY;

        // Rotate vertically up to 90 degrees each direction
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate player horizontally 
        yRotation += mouseX;

        // Apply rotation to camera and player
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f) * Quaternion.Euler(stumbleRotation);
        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void StumbleDrift(Vector3 stumbleModifier)
    {
        stumbleRotation = Vector3.Lerp(stumbleRotation, new Vector3(stumbleModifier.z * 5f, stumbleModifier.x * 5f, stumbleModifier.x * 5f), 2f * Time.deltaTime);
    }
}
