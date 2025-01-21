using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionShield : ActionBase
{
    public int duration;
    public PointEffect shieldEffect;
    
    public override void ActivateAction(ActiveSkill skill, List<Unit> targets, List<int> damages)
    {
        Debug.Log("ActionShield Activated");
        var shieldSystemList = new List<UnitShieldSystem>();
        
        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].TryGetComponent(out UnitShieldSystem shieldSystem)) continue;
            
            shieldSystem.ChargeShield(damages[i], duration);

            PointEffect effect = Effect.GetOrCreateEffect(Core.ObjectPoolManager, shieldEffect);
            effect.transform.SetParent(shieldSystem.transform, false);
            
            EffectContext context = new EffectContext(Vector3.zero, null, null, null, null);
            
            effect.Play(targets[i], ref context);

            shieldSystem.OnDestroyShield += () =>
            {
                effect.Stop(null);
            };
        }
        
    }

    public override void UndoAction()
    {
        //TODO: 되돌리기 구현
    }

    public override void ActivatePostAction(ActiveSkill skill)
    {
    }
}
