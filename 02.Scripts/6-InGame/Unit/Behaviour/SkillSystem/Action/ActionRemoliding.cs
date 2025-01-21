using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRemoliding : ActionBase
{
    public override void ActivateAction(ActiveSkill skill, List<Unit> targets, List<int> damages)
    {
        foreach (var target in targets)
        {
            target.GaugeSystem.Charge<ReMolding>((int)skill.Data.SkillPower);
        }
    }
    public override void UndoAction()
    {
    }

    public override void ActivatePostAction(ActiveSkill skill)
    {
    }
}
