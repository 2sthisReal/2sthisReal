using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenController : MonoBehaviour
{
    [SerializeField] private Image fade;
    [SerializeField] private float fadeDuration = 1f;

    public void SetBlackScreen()
    {
        Color color = fade.color;
        color.a = 1f;
        fade.color = color;
    }
    
    public IEnumerator FadeOut()
    {
        float time = 0f;
        Color color = fade.color;
        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fade.color = color;
            time += Time.deltaTime;
            yield return null;
        }
        color.a = 1f;
        fade.color = color;
    }

    public IEnumerator FadeIn()
    {
        float time = 0f;
        Color color = fade.color;
        while (time < fadeDuration)
        {
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fade.color = color;
            time += Time.deltaTime;
            yield return null;
        }
        color.a = 0f;
        fade.color = color;
    }
}
