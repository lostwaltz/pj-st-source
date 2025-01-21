using DG.Tweening;
using UnityEngine;

public class BasicProjectileEffect : Effect
{
    [SerializeField] public float Speed;
    [SerializeField] private PointEffect hitEffect;
    [SerializeField] protected SoundHandler soundHandler;

    protected Unit Owner;
    protected EffectContext Context;
    public float RandValue = 0.15f;


    public override void Play(Unit owner, ref EffectContext context)
    {
        Owner = owner;
        Context = context;

        soundHandler.PlaySound(transform.position);

        if (context.StartPos == null) return;
        if (context.TargetTransform == null) return;

        var direction = context.StartPos.Value - context.TargetTransform.position;
        var distance = direction.magnitude;
        var duration = distance / Speed;

        //TODO: 매직넘버 수정
        transform.position = context.StartPos.Value;
        transform.DOMove((context.TargetTransform.position + (Vector3.up * 1f).AddRandom(RandValue, RandValue, 0f) + direction.normalized * 0.2f), duration)
            .SetEase(Ease.Linear).OnComplete(OnComplete);
    }

    public override void Stop(EffectContext context)
    {
    }


    protected override void OnComplete()
    {
        base.OnComplete();
        Debug.Log("Test OnComplete : BasicProjectileEffect");
        CallHitEvent(Context); // 타격 시점

        // 타격 이펙트
        PointEffect effect = GetOrCreateEffect<PointEffect>(Core.ObjectPoolManager, hitEffect);
        var hitContext = new EffectContext(
            transform.position,
            transform.rotation,
            Context.TargetTransform,
            Context.MultiTarget,
            Context.Damages,
            Context.AttackCount,
            Context.HitPerAttack,
            Context.Method
            );
        
        effect.Play(Owner, ref hitContext);

        Core.ObjectPoolManager.ReleaseObject(Key, this);
    }
}