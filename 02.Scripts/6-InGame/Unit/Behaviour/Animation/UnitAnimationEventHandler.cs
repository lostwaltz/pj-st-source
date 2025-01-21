using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Unit ownerUnit;
    private BossEffectHandler bossEffectHandler;

    private void Start()
    {
        bossEffectHandler = GetComponentInChildren<BossEffectHandler>();
    }

    public event Action OnAnimationEventTriggered = null;
    public event Action OnAnimationEventEndTriggered;

    public void AnimationEventTrigger()
    {
        OnAnimationEventTriggered?.Invoke();
    }

    public void AnimationEventEndTrigger()
    {
        OnAnimationEventEndTriggered?.Invoke();
    }

    public void PlayDeathEffect(int pointIndex = 0)
    {
        bossEffectHandler.PlayDeathEffect(pointIndex);
    }
}
