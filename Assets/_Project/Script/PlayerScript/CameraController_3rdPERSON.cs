using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_3rdPERSON : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 100f;

    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate camera around the player
        transform.RotateAround(player.position, Vector3.up, mouseX);

        // Adjust camera pitch
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -30f, 60f); // Clamp vertical rotation
        transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0f);
    }
}
