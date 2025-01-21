using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Fader
{
    private Action onFadeComplete;

    public void OnComplete(Action action)
    {
        onFadeComplete -= action;
        onFadeComplete += action;
    }

    public Fader RemoveAction(Action action)
    {
        onFadeComplete -= action;
        
        return this;
    }

    public IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float startAlpha, float targetAlpha,  float duration)
    {
        var time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        onFadeComplete?.Invoke();
        onFadeComplete = null;
    }
}