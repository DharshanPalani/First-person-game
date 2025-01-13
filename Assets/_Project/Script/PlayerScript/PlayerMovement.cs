using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Variables to use in editor
    [SerializeField] private CharacterController _controller;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private float _groundDistance = 0.5f;

    private Vector3 _velocity; // Handles vertical movement
    private bool _isGrounded; // Checks if player is on the ground

    private void Update()
    {
        // Check if player is grounded
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, LayerMask.GetMask("Ground"));

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Reset velocity when grounded
        }

        // Get input for horizontal and vertical movement
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");

        // Calculate movement direction
        Vector3 move = transform.right * xAxis + transform.forward * zAxis;

        // Move the character
        _controller.Move(move * _speed * Time.deltaTime);

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
