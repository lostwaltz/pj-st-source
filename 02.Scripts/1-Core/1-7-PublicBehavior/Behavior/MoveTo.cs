using System;
using DG.Tweening;
using UnityEngine;

public class MoveTo : BehaviorBase
{
    private readonly Vector3 startPos;
    private readonly Vector3 endPos;
    
    private readonly Ease ease;

    private readonly bool freezeX;
    private readonly bool freezeY;

    private readonly bool ignoreStartValue;
    private readonly bool turnOff;
    
    private Tween tween;
    
    private Vector3? originalPosition;
    
    public MoveTo(Vector3 startPos, Vector3 endPos, Ease ease, bool freezeX, bool freezeY, bool ignoreStartValue, bool turnOff)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.ease = ease;
        this.freezeX = freezeX;
        this.freezeY = freezeY;
        this.ignoreStartValue = ignoreStartValue;
        this.turnOff = turnOff;
    }

    public override BehaviorBase BehaviorExecute(Transform target, float duration)
    {
        Vector3 resultStartPos = target.localPosition;
        Vector3 resultEndPos = target.localPosition;

        if (!freezeX)
        {
            resultStartPos.x = startPos.x;
            resultEndPos.x = endPos.x;
        }
        if (!freezeY)
        {
            resultStartPos.y = startPos.y;
            resultEndPos.y = endPos.y;
        }

        resultEndPos.z = startPos.z; // startPos.z 값이 올바른지 확인

        if (ignoreStartValue)
        {
            originalPosition ??= target.localPosition;
            
            resultStartPos = originalPosition.Value;
        }

        target.localPosition = resultStartPos;

        RectTransform rectTransform = target.GetComponent<RectTransform>();

        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }

        tween = rectTransform.DOAnchorPos((Vector2)resultEndPos, duration) // DOAnchorPos로 변경
            .SetEase(ease)
            .OnComplete(() =>
            {
                Action?.Invoke();
                if (turnOff) target.gameObject.SetActive(false);
            });

        return this;

    }
}