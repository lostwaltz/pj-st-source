using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHeal : ActionBase
{
    private ActiveSkill ownerSkill;
    private List<int> amount = new List<int>();
    private List<Unit> targets;
    
    public override void ActivateAction(ActiveSkill skill, List<Unit> targets, List<int> damages)
    {
        ownerSkill = skill;
        this.targets = targets;
        
        for (int i = 0; i < targets.Count; i++)
        {
            int tempAmount = (int)(targets[i].StatsSystem.Stats.Health * skill.Data.SkillPower);
            amount.Add(targets[i].HealthSystem.RecoverHealth(tempAmount));
        }
    }

    public override void UndoAction()
    {
    }

    public override void ActivatePostAction(ActiveSkill skill)
    {
    }
}
