using System.Collections;
using UnityEngine;

public class FadingCanvas : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float fadeDuration = 5.0f;

    void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, fadeDuration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup canvasGroup,
        float start,
        float end,
        float duration
    )
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = end;
        canvasGroup.gameObject.SetActive(false);
    }
}
