using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Select All Units Around", menuName = "SOs/Selector/All Units Around")]
public class SelectAllUnitsAround : SelectorSO
{
    public override void Select(Skill skill, ref Vector2 start, ref Vector2 target, ref List<Unit> targets)
    {
        GameManager.Instance.Indicator.HideAttackRelatedIndicator();
        
        // 유닛 주변 반경 내 모든 유닛 선택하기
        UnitType targetType = (UnitType)skill.Data.SkillBase.TargetType;
        var maps = StageManager.Instance.cellMaps;
        
        StageManager.Interaction.GetNearCoord(skill.OwnerUnit.curCoord, out List<Vector2> range, skill.Data.Scope);

        for (int i = 0; i < range.Count; i++)
        {
            GameManager.Instance.Indicator.Show<IndicatorRangeCell>(range[i]);
            
            int idx = maps[range[i]].unitIndexInCell;
            
            if (IsValidUnit(idx, targetType, maps[range[i]].unitTypeInCell) && maps[range[i]].unitTypeInCell.Equals(targetType))
                targets.Add(GameUnitManager.Instance.UnitDic[targetType][idx]);
        }
        
        ShowTargetIndicator(start, target, skill.OwnerUnit, ref targets);
    }
}