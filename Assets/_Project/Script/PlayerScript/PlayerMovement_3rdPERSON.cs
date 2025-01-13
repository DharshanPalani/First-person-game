using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_3rdPERSON : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _cam;

    [SerializeField] private Transform _groundCheck; // Ground check transform
    [SerializeField] private LayerMask _groundMask;  // Layer mask for ground detection

    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundDistance = 0.4f;

    private float _turnSmoothVelocity;
    private Vector3 _velocity;
    private bool _isGrounded;

    void Update()
    {
        // Ground check
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Reset velocity when grounded
        }

        // Get input for movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Move the player
        if (inputDirection.magnitude >= 0.1f)
        {
            // Calculate movement direction relative to the camera
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;

            // Smoothly rotate player to face the movement direction
            float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

            // Calculate movement direction
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDirection.normalized * _moveSpeed * Time.deltaTime);
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        // Apply gravity
        _velocity.y += _gravity * Time.deltaTime;

        // Move the character vertically
        _controller.Move(_velocity * Time.deltaTime);
    }
}
