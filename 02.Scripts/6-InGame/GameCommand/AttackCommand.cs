using System;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using Structs;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class AttackCommand : IUnitCommand
{
    Unit subject;
    int skill;
    List<Unit> targets;
    List<int> damages;

    private Action onComplete;
    
    Skill skills;
    
    CommandContext context;

    public Unit GetUnit()
    {
        return subject;
    }
    public Skill GetSkill()
    {
        return skills;
    }

    public List<Unit> GetAttackTarget()
    {
        return targets;
    }

    public CommandContext GetContext()
    {
        return context;
    }

    public AttackCommand(Unit unit, int skillIndex, List<Unit> targets, List<int> damages, Action callback = null)
    {
        subject = unit;
        skill = skillIndex;
        this.targets = targets;
        this.damages = damages;
        
        if (targets.Count > 0)
            context.TargetUnit = targets?.First();
        
        skills = subject.SkillSystem.GetSkill(skillIndex);
        
        onComplete += callback;
    }


    public virtual IEnumerator Execute()
    {

        // 스킬, 공격 사용
        yield return subject.StartCoroutine(subject.SkillSystem.ActivateSKill(skill, targets, damages));
        
        onComplete?.Invoke();
        
        //Core.EventManager.Publish(new ExecuteAttackCommandEvent(true, subject,targets));
    }

    // TODO: 언두 기능 개발
    // public void Undo()
    // {
    //     skills.ActionHandler.UndoActions();
    // }
}