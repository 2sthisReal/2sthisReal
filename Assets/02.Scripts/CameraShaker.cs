using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private System.Collections.IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = transform.position + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
