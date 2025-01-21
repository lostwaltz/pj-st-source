using System.Collections;
using UnityEngine;

public class AutoMultipleProjectileEffect : Effect
{
    [SerializeField] private MultipleTargetProjectileEffect bulletEffect;
    [SerializeField] private float fireDelay;

    private Coroutine coroutine;
    private Unit owner;

    public override void Play(Unit owner, ref EffectContext context)
    {
        this.owner = owner;

        owner.AnimationEventHandler.OnAnimationEventEndTriggered -= OnComplete;
        owner.AnimationEventHandler.OnAnimationEventEndTriggered += OnComplete;

        if (coroutine == null)
            coroutine = StartCoroutine(AutoFire(owner, context));
    }

    public override void Stop(EffectContext context)
    {
    }

    private IEnumerator AutoFire(Unit owner, EffectContext context)
    {
        int count = 0;
        int maxCount = context.AttackCount;
        while (count < maxCount)
        {
            Effect basicBullet = GetOrCreateEffect(Core.ObjectPoolManager, bulletEffect);
            basicBullet.UpdateHitEvent(OnHit);
            basicBullet.Play(owner, ref context);
            
            yield return new WaitForSeconds(fireDelay);
            count++;
        }
        
        OnComplete();
    }

    protected override void OnComplete()
    {
        base.OnComplete();

        if (coroutine == null) return;

        StopCoroutine(coroutine);
        coroutine = null;

        owner.AnimationEventHandler.OnAnimationEventEndTriggered -= OnComplete;
        Core.ObjectPoolManager.ReleaseObject(Key, this);
    }
}