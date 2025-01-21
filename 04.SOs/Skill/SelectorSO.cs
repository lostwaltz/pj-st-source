using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

public abstract class SelectorSO : ScriptableObject
{
    public LayerMask layerMask = 1 << 11 | 1 << 12 | 1 << 22;
    
    public abstract void Select(Skill skill, ref Vector2 start, ref Vector2 target, ref List<Unit> targets);

    public virtual bool Interact(RaycastHit hit, UnitType type, out Vector2 targetCoord)
    {
        // 기본 상호작용 대상 : 유닛
        if (hit.collider.TryGetComponent(out Unit unit) &&
            unit.type == type)
        {
            targetCoord = unit.curCoord;
            if (IsInRange(targetCoord))
                return true;
        }
        
        targetCoord = Vector2.zero;
        return false;
    }

    public virtual void ShowSelectionRange(Skill skill, Vector2 coord)
    {
        StageManager.Interaction.ShowSkillRangeCells(coord, skill.Data.SkillRange);
    }

    public virtual void HideSelectionRange()
    {
        GameManager.Instance.Indicator.HideAttackRelatedIndicator();
    }
    
    public bool IsInRange(Vector2 coord)
    {
        return StageManager.Interaction.IsActiveCell(coord);
    }

    protected bool IsValidUnit(int unitId, UnitType type, UnitType cellUnitType)
    {
        if (type != cellUnitType)
            return false;
        
        return unitId >= 0 && GameUnitManager.Instance.UnitIsAlive[type][unitId];
    }

    protected virtual void ShowTargetIndicator(Vector2 start, Vector2 dest, Unit subject, ref List<Unit> targets)
    {
        // 선택된 대상 인디케이터 표기
        GameManager.Instance.Indicator.ShowAttackIndicator(start, dest); // attackPoint, CommandCoord
        GameManager.Instance.Indicator.ShowAttackEstimate(subject, start, ref targets); // OwnerUnit, attackPoint, ref Targets    
    }
    
}