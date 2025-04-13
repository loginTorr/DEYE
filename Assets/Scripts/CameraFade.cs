using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    internal static object fadeInstace;

    public static CameraFade fadeInstance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        fadeInstance = this;
    }

    public IEnumerator FadeToBlack()
    {
        yield return StartCoroutine(Fade(0f, 1f));
    }

    public IEnumerator FadeFromBlack()
    {
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elasped = 0f;
        Color color = fadeImage.color;

        while (elasped < fadeDuration)
        {
            elasped += Time.deltaTime;
            float t = Mathf.Clamp01(elasped / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;
            yield return null;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeToBlack());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeFromBlack());
    }
}
