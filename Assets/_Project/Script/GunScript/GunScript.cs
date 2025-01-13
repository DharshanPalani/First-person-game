using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour
{
    private float _damage;
    private float _range;
    private float _cooldownTime;
    private float _currentClip;
    private float _currentAmmo;
    private float _maxClip;
    private float _maxAmmo;

    public UnityEvent cameraShakeEvent;

    [SerializeField] private GunScriptableObject _currentGun;
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private LayerMask _hitLayers;

    [SerializeField] private float autoAimStrength = 5f;
    [SerializeField] private float redDotDuration = 0.5f;

    private GameObject _currentRedDot;
    private float _nextFireTime;

    private void Start()
    {
        _damage = _currentGun.damage;
        _range = _currentGun.range;
        _cooldownTime = _currentGun.coolDown;
        _maxClip = _currentGun.clipSize;
        _maxAmmo = _currentGun.ammoSize;

        _currentClip = _maxClip;
        _currentAmmo = _maxAmmo;
    }

    void Update()
    {
        // Continuously shoot a ray to check for enemies
        PerformRaycast();


        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadDelay());
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
                PlaceRedDot(hit);

                // Auto-aim: Slightly adjust aim toward the enemy
                Vector3 targetDirection = (hit.point - cameraPosition).normalized;
                Vector3 adjustedDirection = Vector3.Lerp(cameraForward, targetDirection, autoAimStrength * Time.deltaTime);

                _raycastCamera.transform.rotation = Quaternion.LookRotation(adjustedDirection);
            }
        }
    }

    private void PlaceRedDot(RaycastHit hit)
    {
        float offsetDistance = 0.2f;

        if (_currentRedDot == null)
        {
            _currentRedDot = new GameObject("RedDot");
            var spriteRenderer = _currentRedDot.AddComponent<SpriteRenderer>();

            Texture2D texture = new Texture2D(16, 16);
            Color[] pixels = new Color[16 * 16];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.red;
            texture.SetPixels(pixels);
            texture.Apply();

            Sprite redDotSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = redDotSprite;

            _currentRedDot.transform.localScale = Vector3.one * 1f;
        }

        float distanceToHit = Vector3.Distance(_raycastCamera.transform.position, hit.point);

        if (distanceToHit <= _range)
        {
            Vector3 offsetPosition = hit.point + hit.normal * offsetDistance;
            _currentRedDot.transform.position = offsetPosition;
            _currentRedDot.SetActive(true);
        }
        else
        {
            _currentRedDot.SetActive(false);
        }

        Destroy(_currentRedDot, redDotDuration);
    }


    private void Shoot()
    {
        _nextFireTime = Time.time + _cooldownTime;
        if (_currentClip <= 0) return;

        _currentClip -= 1;

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

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(3f);
        Reload();
    }

    private void Reload()
    {
        float reloadAmt = _maxClip - _currentClip;
        reloadAmt = (_currentAmmo - reloadAmt) >= 0 ? reloadAmt : _currentAmmo;

        _currentClip += reloadAmt;
        _currentAmmo -= reloadAmt;
    }
}
