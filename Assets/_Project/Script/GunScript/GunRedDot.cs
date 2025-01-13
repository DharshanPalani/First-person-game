using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRedDot : MonoBehaviour
{

    private GameObject _currentRedDot;

    public void PlaceRedDot(RaycastHit hit)
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
}
