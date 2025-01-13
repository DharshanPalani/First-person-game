using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void StartCameraShake()
    {
        StartCoroutine(Shake());
    }

    public IEnumerator Shake(float duration = 0.2f, float magnitude = 0.3f)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}