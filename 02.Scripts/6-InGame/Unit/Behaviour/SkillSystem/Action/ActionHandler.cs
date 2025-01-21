using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler : MonoBehaviour
{
    public List<ActionBase> SkillActions;

    public void ActivateSkillActions(ActiveSkill skill, List<Unit> targets, List<int> damages)
    {
        if (targets.Count == 0) 
            return;
        
        foreach (var action in SkillActions)
            action.ActivateAction(skill, targets, damages);
    }

    public void ActivateSkillPostActions(ActiveSkill skill)
    {
        foreach (var action in SkillActions)
            action.ActivatePostAction(skill);
    }
    
    public void UndoActions()
    {
        foreach (var action in SkillActions)
            action.UndoAction();
    }
}
