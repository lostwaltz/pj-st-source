using UnityEngine;
using System.Collections.Generic;

public class ParticleProjectileEffect : Effect
{
    [SerializeField] private ParticleSystem projectileParticle;
    [SerializeField] private PointEffect hitEffect;
    [SerializeField] private SoundHandler soundHandler;
    [SerializeField] private float randValue = 0.15f;

    protected Unit Owner;
    protected EffectContext Context;
    private Vector3 direction;
    private Vector3 hitPosition;

    public override void Play(Unit owner, ref EffectContext context)
    {
        Owner = owner;
        Context = context;

        if (context.StartPos == null || context.TargetTransform == null) return;

        Vector3 randomTargetPos = context.TargetTransform.position +
           (Vector3.up * 1f).AddRandom(randValue, randValue, 0f);

        transform.position = context.StartPos.Value;
        direction = randomTargetPos - context.StartPos.Value;
        transform.rotation = Quaternion.LookRotation(direction);

        projectileParticle.Clear();

        var collision = projectileParticle.collision;
        collision.enabled = true;
        collision.sendCollisionMessages = true;

        projectileParticle.Play();

        soundHandler?.PlaySound(transform.position);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<Unit>(out var hitUnit) && hitUnit == Context.TargetTransform.GetComponent<Unit>())
        {
            List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
            projectileParticle.GetCollisionEvents(other, collisionEvents);

            if (collisionEvents.Count > 0)
            {
                hitPosition = collisionEvents[0].intersection;
                OnComplete();
            }
        }
    }

    protected override void OnComplete()
    {
        base.OnComplete();
        CallHitEvent(Context);

        PointEffect effect = GetOrCreateEffect<PointEffect>(Core.ObjectPoolManager, hitEffect);
        var context = new EffectContext(hitPosition, transform.rotation,
            Context.TargetTransform, Context.MultiTarget, Context.Damages,
            Context.AttackCount, Context.HitPerAttack, Context.Method);
        effect.Play(Owner, ref context);

        projectileParticle.Stop();
        Core.ObjectPoolManager.ReleaseObject(Key, this);
    }

    public override void Stop(EffectContext context)
    {
        projectileParticle.Stop();
    }

}