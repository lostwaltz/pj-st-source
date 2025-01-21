using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Select Single Unit", menuName = "SOs/Selector/Single Unit")]
public class SelectSingleUnit : SelectorSO
{
    public override void Select(Skill skill, ref Vector2 start , ref Vector2 target, ref List<Unit> targets)
    {
        // 범위 내 단일 대상 선택
        if (IsInRange(target))
        {
            UnitType targetType = (UnitType)skill.Data.SkillBase.TargetType;
            int idx = StageManager.Instance.cellMaps[target].unitIndexInCell;
            UnitType cellType = StageManager.Instance.cellMaps[target].unitTypeInCell;
            
            if (IsValidUnit(idx, targetType, cellType))
                targets.Add(GameUnitManager.Instance.UnitDic[targetType][idx]);
        }
        
        ShowTargetIndicator(start, target, skill.OwnerUnit, ref targets);
    }
}