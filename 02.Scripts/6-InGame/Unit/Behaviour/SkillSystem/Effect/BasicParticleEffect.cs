using System;
using UnityEngine;

public class BasicParticleEffect : Effect
{
    private ParticleEmitter emitter;
    private EffectContext playingContext;

    private void Awake()
    {
        emitter = gameObject.GetOrAdd<ParticleEmitter>();
    }

    public override void Play(Unit owner, ref EffectContext context)
    {
        playingContext = context;
        emitter.OnComplete(OnParticleComplete);
        emitter.Play();
    }

    public override void Stop(EffectContext context)
    {
        emitter.Stop();
    }

    public void OnParticleComplete()
    {
        OnComplete(); 
        CallHitEvent(playingContext); 
    }
}