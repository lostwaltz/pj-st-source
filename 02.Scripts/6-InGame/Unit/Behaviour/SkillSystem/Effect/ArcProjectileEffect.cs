using System;
using DG.Tweening;
using UnityEngine;
public class ArcProjectileEffect : BasicProjectileEffect
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] protected PointEffect explosionEffect;
    
    
    public float noiseIntensity = 0.3f; // 노이즈 강도
    public float noiseSpeed = 1f;      // 노이즈 속도

    public float randValue = 0.01f;
    
    private bool isPlaying = false;
    private Vector3 initialPosition;
    
    
    public override void Play(Unit owner, ref EffectContext context)
    {
        initialPosition = transform.position;
        isPlaying = true;

        Owner = owner;
        Context = context;
        soundHandler.PlaySound(transform.position);

        if (context.StartPos == null) return;
        if (context.TargetTransform == null) return;

        var direction = context.TargetTransform.position - context.StartPos.Value;
        var distance = direction.magnitude;
        var duration = distance / Speed;

        var midPoint = Vector3.Lerp(context.StartPos.Value, context.TargetTransform.position, 0.5f);
        midPoint.y += (distance / 3f); // TODO: 매직넘버 수정

        Vector3[] path = new Vector3[]
        {
            context.StartPos.Value,
            midPoint,
            context.TargetTransform.position
        };

        transform.position = context.StartPos.Value;

        transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(OnComplete);
    }

    private void Update()
    {
        if(false == isPlaying) return;

        trailRenderer.transform.position =  transform.position.AddRandom(randValue);
        initialPosition = transform.position;
    }

    protected override void OnComplete()
    {
        base.OnComplete();

        PointEffect effect = GetOrCreateEffect(Core.ObjectPoolManager, explosionEffect);

        var context = new EffectContext(transform.position + Vector3.up * 2f, Quaternion.identity, Context.TargetTransform, Context.MultiTarget, Context.Damages,
            Context.AttackCount, Context.HitPerAttack, Context.Method);
        
        effect.Play(Owner, ref context);
        
        isPlaying = false;
        trailRenderer.Clear();
    }
}
