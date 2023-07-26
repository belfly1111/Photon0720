using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOut : MonoBehaviour
{
    public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간
    public bool fadeInOnStart = true; // 시작할 때 페이드 인 여부

    private Image fadePanel;
    private Color originalColor;
    private Color targetColor;
    private float elapsedTime = 0f;
    private bool isFading = false;

    private void Start()
    {
        fadePanel = GetComponent<Image>();
        originalColor = fadePanel.color;
        targetColor = originalColor;
        if (fadeInOnStart)
        {
            StartFadeIn();
        }
    }

    public void StartFadeIn()
    {
        targetColor.a = 0f;
        StartFade();
    }

    public void StartFadeOut()
    {
        targetColor.a = 1f;
        StartFade();
    }

    private void StartFade()
    {
        elapsedTime = 0f;
        isFading = true;
    }

    private void Update()
    {
        if (isFading)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;
            fadePanel.color = Color.Lerp(originalColor, targetColor, normalizedTime);

            if (normalizedTime >= 1f)
            {
                isFading = false;
            }
        }
    }
}