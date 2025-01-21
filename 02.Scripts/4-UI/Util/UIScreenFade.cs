using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFadeable
{
    public IFadeable FadeTo(float startAlpha, float targetAlpha, float duration);
    public void OnComplete(Action action);
    public void Release();
}

public class UIScreenFade : UIBase, IFadeable
{
    [SerializeField] private CanvasGroup CanvasGroup;
    private Fader fader;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        fader = new Fader();
    }

    public IFadeable FadeTo(float startAlpha, float targetAlpha, float duration)
    {
        Open();
        
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        
        fadeCoroutine = StartCoroutine(fader.FadeCoroutine(CanvasGroup, startAlpha, targetAlpha, duration));

        return this;
    }

    public void OnComplete(Action action)
    {
        fader.OnComplete(action);
    }

    public void Release()
    {
        Core.UIManager.ReleaseUI<UIScreenFade>();
        
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        
        Destroy(gameObject);
    }
}
