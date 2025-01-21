
using UnityEngine;
using DG.Tweening;
using System;

public class DamageHUDAnimation : MonoBehaviour
{
    [Header("애니메이션 설정")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve decreaseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve increaseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Sequence currentSequence;

    public void PlayHealthAnimation(int startValue, int targetValue, Action<int> onUpdate, Action onComplete = null)
    {

        // 이전 애니메이션이 있으면 중단
        if (currentSequence != null && currentSequence.IsPlaying())
        {
            currentSequence.Kill();
        }

        bool isDecreasing = targetValue < startValue;

        currentSequence = DOTween.Sequence();


        // 애니메이션
        currentSequence.Append(
            DOTween.To(
                () => startValue,
                (value) => onUpdate?.Invoke(value),
                targetValue,
                animationDuration
            ).SetEase(isDecreasing ? decreaseCurve : increaseCurve)
        );

        // 완료 콜백
        if (onComplete != null)
        {
            currentSequence.OnComplete(() => onComplete.Invoke());
        }
    }

    private void OnDisable()
    {
        if (currentSequence != null && currentSequence.IsPlaying())
        {
            currentSequence.Kill();
        }
    }

    // private void PlayHealthDecreaseAnimation(int startHealth, int targetHealth)
    // {
    //     if (damageHUDAnimation != null)
    //     {
    //         damageHUDAnimation.PlayHealthAnimation(
    //                startHealth,
    //                targetHealth,
    //             (value) => Health = value
    //         );
    //     }
    // }
}
