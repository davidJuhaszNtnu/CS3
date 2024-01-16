using System.Collections;
using UnityEngine;

public class FadePortal : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Get the CanvasGroup component
        canvasGroup = GetComponent<CanvasGroup>();

        // Start with fully transparent (fade-out)
        canvasGroup.alpha = 0f;
    }

    public void FadeIn(float duration)
    {
        // Fade-in animation
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 1f, duration));
    }

    public void FadeOut(float duration)
    {
        // Fade-out animation
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, duration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float startTime = Time.time;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed = Time.time - startTime;
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        cg.alpha = end; // Ensure the alpha is exactly the target value
    }
}
