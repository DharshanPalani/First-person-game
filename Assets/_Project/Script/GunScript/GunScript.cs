using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour
{
    private float _damage;
    private float _range;
    private float _cooldownTime;
    private float _currentMagazine;
    private float _currentClip;
    private float _currentAmmo;
    private float _maxMagazine;
    private float _maxClip;
    private float _maxAmmo;

    public UnityEvent cameraShakeEvent;

    [SerializeField] private GunScriptableObject _currentGun;
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private LayerMask _hitLayers;

    [SerializeField] private float autoAimStrength = 5f;
    [SerializeField] private float redDotDuration = 0.5f;

    private float _nextFireTime;

    private void Start()
    {
        _damage = _currentGun.damage;
        _range = _currentGun.range;
        _cooldownTime = _currentGun.coolDown;
        _maxMagazine = _currentGun.magazineSize;
        _maxClip = _currentGun.clipSize;

        _currentMagazine = _maxMagazine;
        _currentClip = _maxClip;
    }

    void Update()
    {
        // Continuously shoot a ray to check for enemies
        PerformRaycast();


        if (Input.GetKeyDown(KeyCode.R)) Reload();
        if (Cursor.lockState != CursorLockMode.Locked) return;
        if (!Input.GetMouseButton(0)) return;
        if (Time.time <= _nextFireTime) return;
        Shoot();

    }

    private void PerformRaycast()
    {
        Vector3 cameraPosition = _raycastCamera.transform.position;
        Vector3 cameraForward = _raycastCamera.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cameraPosition, cameraForward, out hit, _range, _hitLayers))
        {
            if (hit.transform.GetComponent<Health>() is { entityType: Health.EntityType.Enemy })
            {
                // Place a red dot on the enemy
                GunRedDot gunRedDot = GetComponent<GunRedDot>();
                if (gunRedDot != null)
                {
                    gunRedDot.PlaceRedDot(hit);
                }

                // Auto-aim: Slightly adjust aim toward the enemy
                Vector3 targetDirection = (hit.point - cameraPosition).normalized;
                Vector3 adjustedDirection = Vector3.Lerp(cameraForward, targetDirection, autoAimStrength * Time.deltaTime);

                _raycastCamera.transform.rotation = Quaternion.LookRotation(adjustedDirection);
            }
        }
    }

    


    private void Shoot()
    {
        _nextFireTime = Time.time + _cooldownTime;
        if (_currentMagazine == 0) return;

        _currentMagazine -= 1;

        cameraShakeEvent.Invoke();
        FindObjectOfType<AudioManager>().Play("AK-47");

        Vector3 cameraPosition = _raycastCamera.transform.position;
        Vector3 cameraForward = _raycastCamera.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cameraPosition, cameraForward, out hit, _range, _hitLayers))
        {
            GameObject hitEffect = Instantiate(_hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffect, 1f);

            Health healthComponent = hit.transform.GetComponent<Health>();
            if (healthComponent != null && healthComponent.entityType != Health.EntityType.Player)
            {
                healthComponent.UpdateHealth(-_damage);
            }
        }
    }

    private void Reload()
    {
        float reloadAmt = _maxClip - _currentClip;
    }
}
