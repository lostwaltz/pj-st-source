using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetProjectileEffect : Effect
{
    [SerializeField] private BasicProjectileEffect bulletEffect;
    [SerializeField] private float randValue;
    
    public override void Play(Unit owner, ref EffectContext context)
    {
        if(null == context.MultiTarget) return;

        for (int i = 0; i < context.MultiTarget.Count; i++)
        {
            Unit target = context.MultiTarget[i];
            List<Unit> singleTarget = new (){ target };
            List<int> singleDamage = new (){ context.Damages[i] };
            
            BasicProjectileEffect basicBullet = GetOrCreateEffect(Core.ObjectPoolManager, bulletEffect);
            basicBullet.UpdateHitEvent(OnHit);
            var localContext = new EffectContext(context.StartPos, null, target.transform, singleTarget, singleDamage,
                context.AttackCount, context.HitPerAttack, context.Method);
            
            basicBullet.RandValue = randValue;
            basicBullet.Play(owner, ref localContext);
        }
    }

    public override void Stop(EffectContext context)
    {
    }

    protected override void OnComplete()
    {
        base.OnComplete();
        
        Core.ObjectPoolManager.ReleaseObject(Key, this);
    }
}