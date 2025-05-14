using System.Collections;
using UnityEngine;

public class FadingCanvas : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _fadeDuration = 5.0f;

    void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.alpha = 1;
        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 0, _fadeDuration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 1, _fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup canvasGroup,
        float start,
        float end,
        float duration
    )
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = end;
        canvasGroup.gameObject.SetActive(false);
    }
}
