using System;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public string Key;
    public event Action Complete;
    public Action<EffectContext> OnHit;
    
    public abstract void Play(Unit owner, ref EffectContext context);
    public abstract void Stop(EffectContext context);
    
    protected virtual void OnComplete()
    {
        Complete?.Invoke();
        Complete = null;
    }
    
    public static T GetOrCreateEffect<T>(ObjectPoolManager poolManager, T template, bool active = true) where T : Effect
    {
        T poolEffect = poolManager.GetPooledObject<T>(template.Key);

        if (poolEffect != null)
        {
            poolEffect.gameObject.SetActive(active);
            return poolEffect;
        }
        
        poolManager.CreateNewPool(template.Key, template, 10, 10);
        poolEffect = poolManager.GetPooledObject<T>(template.Key);
        poolEffect.gameObject.SetActive(active);

        return poolEffect;
    }

    public void CallHitEvent(EffectContext context)
    {
        OnHit?.Invoke(context);
    }

    public void UpdateHitEvent(Action<EffectContext> newHitEvent)
    {
        OnHit = null;
        OnHit = newHitEvent;
    }
}