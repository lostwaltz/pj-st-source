using System.Linq;
using UnityEngine;

public class MultipleProjectileEffect : Effect
{
    [SerializeField] private BasicProjectileEffect bulletEffect;
    [SerializeField] private int multipleCount;
    [SerializeField] private float randValue;
    
    public override void Play(Unit owner, ref EffectContext context)
    {
        context.Damages = context.Damages.Select(n => n / multipleCount).ToList();
        
        for (var i = 0; i < multipleCount; i++)
        {
            BasicProjectileEffect basicBullet = GetOrCreateEffect(Core.ObjectPoolManager, bulletEffect);
            basicBullet.UpdateHitEvent(OnHit);
            basicBullet.RandValue = randValue;
            basicBullet.Play(owner, ref context);
        }

        OnComplete();
    }

    public override void Stop(EffectContext context)
    {
        //noop
    }

    protected override void OnComplete()
    {
        base.OnComplete();
        
        Core.ObjectPoolManager.ReleaseObject(Key, this);
    }
}