using System;
using DG.Tweening;
using UnityEngine;

public class ScaleTo : BehaviorBase
{
    private readonly Vector3 startScale;
    private readonly Vector3 endScale;
    
    private readonly Ease ease;

    private readonly bool freezeX;
    private readonly bool freezeY;

    private readonly bool ignoreStartValue;
    private readonly bool turnOff;
    
    private Tween tween;
    
    public ScaleTo(Vector3 startScale, Vector3 endScale, Ease ease, bool freezeX, bool freezeY, bool ignoreStartValue, bool turnOff)
    {
        this.startScale = startScale;
        this.endScale = endScale;
        this.ease = ease;
        this.freezeX = freezeX;
        this.freezeY = freezeY;
        this.ignoreStartValue = ignoreStartValue;
        this.turnOff = turnOff;
    }

    public override BehaviorBase BehaviorExecute(Transform target, float duration)
    {
        Vector3 resultStartScale = target.localScale;
        Vector3 resultEndScale = target.localScale;

        if (!freezeX)
        {
            resultStartScale.x = startScale.x;
            resultEndScale.x = endScale.x;
        }
        if (!freezeY)
        {
            resultStartScale.y = startScale.y;
            resultEndScale.y = endScale.y;
        }

        if (ignoreStartValue)
            resultStartScale = target.localScale;
        
        target.localScale = resultStartScale;

        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
        
        
        tween = target.DOScale(resultEndScale, duration).SetEase(ease).OnComplete(() =>
        {
            Action?.Invoke();
            if (turnOff) target.gameObject.SetActive(false);
        });
            
        
        return this;
    }
}