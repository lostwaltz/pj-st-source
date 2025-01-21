using UnityEngine;
using System.Collections.Generic;

public class BossEffectHandler : MonoBehaviour
{
    [Header("Death Effect")]
    [SerializeField] private PointEffect deathEffect;
    [SerializeField] private Transform effectPoints;

    public void PlayDeathEffect(int pointIndex = 0)
    {
        PointEffect effect = Effect.GetOrCreateEffect<PointEffect>(
            Core.ObjectPoolManager,
            deathEffect
        );

        effect.transform.position = effectPoints.position;
        effect.transform.rotation = effectPoints.rotation;

        var context = new EffectContext(effectPoints.position, effectPoints.rotation, transform, null, new List<int>(), 1, 1, 0);
        effect.Play(null, ref context);
    }
}