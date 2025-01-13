using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [SerializeField] private float _intensity = 0.02f;
    [SerializeField] private float _smooth = 15f;
    [SerializeField] private float _tiltIntensity = 0.01f;

    private Quaternion _originRotation;

    private void Start()
    {
        _originRotation = transform.localRotation;
    }

    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate sway rotation
        Quaternion swayX = Quaternion.AngleAxis(-_intensity * mouseX, Vector3.up);
        Quaternion swayY = Quaternion.AngleAxis(_intensity * mouseY, Vector3.right);
        Quaternion swayRotation = _originRotation * swayX * swayY;

        // Calculate tilt rotation
        Quaternion tiltRotation = Quaternion.AngleAxis(-_tiltIntensity * mouseX, Vector3.forward);

        // Combine sway and tilt
        Quaternion targetRotation = swayRotation * tiltRotation;

        // Smoothly rotate towards target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * _smooth);
    }
}