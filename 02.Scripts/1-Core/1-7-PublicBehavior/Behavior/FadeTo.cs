using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class FadeTo : BehaviorBase
{
    private readonly float startAlpha;
    private readonly float endAlpha;
    private readonly Ease ease;
    private Tween tween;
    
    private readonly bool ignoreStartValue;
    private readonly bool turnOff;
    private readonly bool destroy;
    
    private CanvasGroup canvasGroup;
    private Image image;
    
    public Tween Tween => tween;
    
    public FadeTo(float startAlpha, float endAlpha, Ease ease, bool ignoreStartValue, bool turnOff, bool destroyOnFinish)
    {
        this.startAlpha = startAlpha;
        this.endAlpha = endAlpha;
        this.ease = ease;
        this.ignoreStartValue = ignoreStartValue;
        this.turnOff = turnOff;
        this.destroy = destroyOnFinish;
    }

    public override BehaviorBase BehaviorExecute(Transform target, float duration)
    {
        canvasGroup ??= target.gameObject.TryGetComponent<CanvasGroup>(out var cg) ? cg : null;
        image ??= target.gameObject.TryGetComponent<Image>(out var ig) ? ig : null;

        if (canvasGroup != null)
        {
            var resultStartAlpha = canvasGroup.alpha;
        
            if (!ignoreStartValue)
                resultStartAlpha = startAlpha;

            canvasGroup.alpha = resultStartAlpha;
        
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }

            tween = canvasGroup.DOFade(endAlpha, duration).SetEase(ease).OnComplete(() =>
            {
                Action?.Invoke();
                if (turnOff) target.gameObject.SetActive(false);
                if (destroy) UnityEngine.Object.Destroy(target.gameObject);
            });

            return this;
        }

        if (image != null)
        {
            var resultStartAlpha = image.color.a;
        
            if (!ignoreStartValue)
                resultStartAlpha = startAlpha;

            image.color = new Color(image.color.r, image.color.b, image.color.g, resultStartAlpha);
        
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }

            tween = image.DOFade(endAlpha, duration).SetEase(ease).OnComplete(() =>
            {
                Action?.Invoke();
                if (turnOff) target.gameObject.SetActive(false);
                if (destroy) UnityEngine.Object.Destroy(target.gameObject);
            });
            
            return this;
        }
        
        return this;
    }
}