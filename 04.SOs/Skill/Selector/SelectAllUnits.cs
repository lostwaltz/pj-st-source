using System.Collections.Generic;
using EnumTypes;
using UnityEngine;


[CreateAssetMenu(fileName = "Select All Units", menuName = "SOs/Selector/All Units")]
public class SelectAllUnits : SelectorSO
{
    public override void Select(Skill skill, ref Vector2 start, ref Vector2 coord, ref List<Unit> targets)
    {
        GameManager.Instance.Indicator.HideAttackRelatedIndicator();
        
        // 스킬 타겟으로부터 주변 지역 유닛 선택
        var maps = StageManager.Instance.cellMaps;
        UnitType targetType = (UnitType)skill.Data.SkillBase.TargetType;
        
        StageManager.Interaction.GetNearCoord(coord, out List<Vector2> range, skill.Data.Scope);
        
        for (int i = 0; i < range.Count; i++)
        {
            GameManager.Instance.Indicator.Show<IndicatorRangeCell>(range[i]);
            
            int idx = maps[range[i]].unitIndexInCell;
            if (IsValidUnit(idx, targetType, maps[range[i]].unitTypeInCell) && maps[range[i]].unitTypeInCell.Equals(targetType))
                targets.Add(GameUnitManager.Instance.UnitDic[targetType][idx]);
        }
        
        ShowTargetIndicator(start, coord, skill.OwnerUnit, ref targets);
    }
}