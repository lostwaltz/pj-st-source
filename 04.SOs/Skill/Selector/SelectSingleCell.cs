using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Select Single Cell", menuName = "SOs/Selector/Single Cell")]
public class SelectSingleCell : SelectorSO
{
    public override void Select(Skill skill, ref Vector2 start, ref Vector2 target, ref List<Unit> targets)
    {
        GameManager.Instance.Indicator.HideAttackRelatedIndicator();
        
        // instance.Scope : 탄착 범위
        StageManager.Interaction.GetNearCoord(target, out List<Vector2> scope, skill.Data.Scope);
        var maps = StageManager.Instance.cellMaps;

        UnitType targetType = (UnitType)skill.Data.SkillBase.TargetType;
        for (int i = 0; i < scope.Count; i++)
        {
            int idx = maps[scope[i]].unitIndexInCell;
            UnitType inCell = maps[scope[i]].unitTypeInCell;
            
            GameManager.Instance.Indicator.Show<IndicatorRangeCell>(scope[i]);
            
            if (IsValidUnit(idx, targetType, inCell) && maps[scope[i]].unitTypeInCell.Equals(targetType))
                targets.Add(GameUnitManager.Instance.UnitDic[targetType][idx]);
        }
        
        ShowTargetIndicator(start, target, skill.OwnerUnit, ref targets);
    }

    public override bool Interact(RaycastHit hit, UnitType type, out Vector2 targetCoord)
    {
        // 기본 상호작용 대상 : 유닛
        targetCoord = Vector2.zero;
        if (hit.collider.TryGetComponent(out StageCell cell))
        {
            targetCoord = cell.placement.coord;
            return true;
        }
        
        return false;
    }
}