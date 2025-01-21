using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class AutoFireProjectileEffect : Effect
{
    [SerializeField] private BasicProjectileEffect bulletEffect;
    [SerializeField] private float fireDelay;
    [SerializeField] private int defaultProjectileCount = 10;

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
            var localContext = new EffectContext(
                context.StartPos,
                context.StartRot,
                context.TargetTransform,
                context.MultiTarget,
                context.Damages, // shotCount == projectileCount - 1 ?  : new List<int> { 0 },
                context.AttackCount,
                context.HitPerAttack,
                context.Method
            );

            Effect basicBullet = GetOrCreateEffect(Core.ObjectPoolManager, bulletEffect);
            basicBullet.UpdateHitEvent(OnHit);
            basicBullet.Play(owner, ref localContext);
            
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