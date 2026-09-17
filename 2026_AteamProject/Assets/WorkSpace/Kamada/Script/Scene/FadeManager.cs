using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage = null;
    [SerializeField] private GameObject fadeObject = null;
    [SerializeField] private float fadeDuration = 1.0f;
    private bool isFading = false;
    private void Start()
    {
        fadeObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0.0f;
        isFading = true;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeDuration;
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(1.0f);
        isFading = false;
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        isFading = true;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1.0f - elapsedTime / fadeDuration;
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(0.0f);
        isFading = false;
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }

    public bool GetIsFading()
    {
        return isFading;
    }
}