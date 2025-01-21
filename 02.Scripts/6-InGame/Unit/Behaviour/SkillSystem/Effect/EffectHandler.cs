using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class EffectContext
{
    public readonly Vector3? StartPos;
    public readonly Quaternion? StartRot;
    [CanBeNull] public readonly Transform TargetTransform;
    [CanBeNull] public readonly List<Unit> MultiTarget;
    [CanBeNull] public List<int> Damages;
    [CanBeNull] public List<int> CalculatedDamages;
    
    public readonly int AttackCount;  // 총 발사 횟수
    public readonly int HitPerAttack;  // 공격 당 히트 횟수
    public readonly int Method; // 단발인지 연사인지
    
    // public int CurrentProjectileIndex { get; set; } = 0;  // 현재 발사 인덱스
    

    public EffectContext(
        Vector3? startPos, Quaternion? startRot, 
        Transform targetTransform, List<Unit> multiTarget, List<int> damages,
        int attackCount = 1, int hitPerAttack = 1, int attackMethod = 0)
    {
        StartPos = startPos;
        TargetTransform = targetTransform;
        StartRot = startRot;
        MultiTarget = multiTarget; //MultiTarget != null ? new List<Transform>(multiTargetTransforms) : null;
        Damages = damages != null ? new List<int>(damages) : null;
        CalculatedDamages = Damages != null ? new List<int>(damages) : new List<int>();
        
        AttackCount = attackCount;
        HitPerAttack = hitPerAttack;
        Method = attackMethod;
    }
}

[Serializable]
public class EffectContainer
{
    public List<Effect> Effects;
    public List<bool> IsHitEffect;
}

public enum StartPosType
{
    None = 0,
    GunPoint,
    HandPoint,
}

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private StartPosType startPosType;
    [SerializeField] private List<EffectContainer> effectContainers;
    
    public Action<EffectContext> OnHitActivated;
    
    public void Play(Unit owner, EffectContext context)
    {
        StartCoroutine(ActiveEffect(owner, context));
    }

    private IEnumerator ActiveEffect(Unit owner, EffectContext context)
    {
        Vector3 startPos = context.StartPos ?? owner.transform.position + (Vector3.up * 0.5f);
        Quaternion startRot = context.StartRot ?? Quaternion.identity;

        switch (startPosType)
        {
            case StartPosType.GunPoint:
                startPos = owner.Requirement.firePoint.position;
                startRot = owner.Requirement.firePoint.rotation;
                break;
            case StartPosType.HandPoint:
                startPos = owner.Requirement.rightHand.position;
                startRot = context.StartRot ?? Quaternion.identity;
                break;
            case StartPosType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        EffectContext localContext = new EffectContext(
            startPos,
            startRot,
            context.TargetTransform,
            context.MultiTarget,
            context.Damages,
            context.AttackCount,
            context.HitPerAttack,
            context.Method
            );

        InitializePooledEffects(out List<EffectContainer> pooledEffectContainers);

        if (effectContainers == null || effectContainers.Count == 0) yield break;

        foreach (var effectContainer in pooledEffectContainers)
        {
            var isPlaying = true;

            var firstEffect = effectContainer.Effects.First();
            firstEffect.Complete += OnComplete;

            for (int i = 0; i < effectContainer.Effects.Count; i++)
            {
                var effect = effectContainer.Effects[i];
                if (effectContainer.IsHitEffect[i])
                    effect.UpdateHitEvent(OnHitActivated);
                
                effect.gameObject.SetActive(true);
                effect.Play(owner, ref localContext);
            }

            yield return new WaitWhile(() => isPlaying);

            firstEffect.Complete -= OnComplete;

            continue;

            void OnComplete() => isPlaying = false;
        }
    }

    private void InitializePooledEffects(out List<EffectContainer> pooledEffects)
    {
        pooledEffects = new List<EffectContainer>();

        ObjectPoolManager poolManager = Core.ObjectPoolManager;

        foreach (var effectContainer in effectContainers)
        {
            var newContainer = new EffectContainer()
            {
                Effects = new List<Effect>(),
                IsHitEffect = new List<bool>()
            };
            pooledEffects.Add(newContainer);

            for (int i = 0; i < effectContainer.Effects.Count; i++)
            {
                Effect poolEffect = GetOrCreateEffect(poolManager, effectContainer.Effects[i]);
                newContainer.Effects.Add(poolEffect);
                newContainer.IsHitEffect.Add(effectContainer.IsHitEffect[i]);
            }
        }
    }

    public Effect GetOrCreateEffect(ObjectPoolManager poolManager, Effect template)
    {
        Effect poolEffect = poolManager.GetPooledObject<Effect>(template.Key);

        if (poolEffect == null)
        {
            poolManager.CreateNewPool(template.Key, template, 10, 10);
            poolEffect = poolManager.GetPooledObject<Effect>(template.Key);
        }

        poolEffect.gameObject.SetActive(false);
        return poolEffect;
    }
}
