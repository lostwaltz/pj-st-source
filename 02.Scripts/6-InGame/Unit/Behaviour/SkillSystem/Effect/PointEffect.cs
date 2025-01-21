
using System;
using UnityEngine;

public class PointEffect : BasicParticleEffect
{
    [SerializeField] protected SoundHandler soundHandler;
    public void InitEffectPoint(Vector3 position)
    {
        transform.position = position;
    }

    public override void Play(Unit owner, ref EffectContext context)
    {
        base.Play(owner, ref context);

        if (context.StartPos == null) return;

        transform.localPosition = context.StartPos.Value;

        if (context.StartRot == null) return;

        transform.rotation = context.StartRot.Value;

        soundHandler?.PlaySound(transform.position);
    }
}