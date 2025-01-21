using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionModifier : ActionBase
{
    [SerializeField] private CombatStatType statType;
    [SerializeField] private OperatorType operatorType;
    [SerializeField] private int duration;
    
    public override void ActivateAction(ActiveSkill skill, List<Unit> targets, List<int> damages)
    {
        var index = 0;
        foreach (var unit in targets)
        {
            if (true == unit.TryGetComponent<UnitStatsSystem>(out var unitStats))
            {
                unitStats.AddModifier(statType, operatorType, skill.Data.SkillBase.Power, duration);                
            }
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
