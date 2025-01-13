using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _mouseSensitivity = 100f;

    private float _xRotation = 0f; // Tracks vertical rotation

    private bool _isCurserLocked;

    private void Start()
    {
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (!_isCurserLocked)
            {
                Cursor.lockState = CursorLockMode.Locked; 
                _isCurserLocked = true;
                return;
            }

            Cursor.lockState = CursorLockMode.None;
            _isCurserLocked = false;
        }

        // Only process mouse movement if the cursor is locked
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

            // Adjust vertical rotation and clamp it
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            // Apply rotations
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _playerBody.Rotate(Vector3.up * mouseX);
        }
    }


}
