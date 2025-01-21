using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : ActionBase
{
    private List<Unit> targets = new();
    private List<int> damages = new();
    
    public override void ActivateAction(ActiveSkill skill, List<Unit> targets, List<int> damages)
    {
        this.targets = targets;
        this.damages = damages;
        
        for (int i = 0; i < targets.Count; i++)
        {
            // 일반 데미지
            if (targets[i].TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(damages[i]);
        }
    }

    public override void ActivatePostAction(ActiveSkill skill)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            // 안정 지수 데미지
            if (targets[i].TryGetComponent(out UnitStabilitySystem statbility))
                statbility.TakeStableDamage(skill.Data.SkillBase.StabilityDamage);
        }
    }

    public override void UndoAction()
    {
        for (int i = 0; i < targets.Count; i++)
            targets[i].HealthSystem.RecoverHealth(damages[i], true);
    }
}
