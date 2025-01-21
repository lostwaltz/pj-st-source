using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

[CreateAssetMenu(fileName = "Select Self Unit", menuName = "SOs/Selector/Self Unit")]
public class SelectSelf : SelectorSO
{
    public override bool Interact(RaycastHit hit, UnitType type, out Vector2 targetCoord)
    {
        // 기본 상호작용 대상 : 유닛
        if (hit.collider.TryGetComponent(out Unit unit) &&
            unit.type == type)
        {
            targetCoord = unit.curCoord;
            return true;
        }
        
        if (hit.collider.TryGetComponent(out HologramController hologram))
        {
            targetCoord = hologram.Coord;
            if (IsInRange(targetCoord))
                return true;
        }
        
        
        targetCoord = Vector2.zero;
        return false;
    }
    
    public override void Select(Skill skill, ref Vector2 start , ref Vector2 target, ref List<Unit> targets)
    {
        targets.Add(skill.OwnerUnit);
        
        ShowTargetIndicator(start, target, skill.OwnerUnit, ref targets);
    }

    public override void ShowSelectionRange(Skill skill, Vector2 coord)
    {
        const int range = 0;
        StageManager.Interaction.ShowSkillRangeCells(coord, range);
    }

    protected override void ShowTargetIndicator(Vector2 start, Vector2 dest, Unit subject, ref List<Unit> targets)
    {
        GameManager.Instance.Indicator.ShowAttackEstimate(subject, start, ref targets); // OwnerUnit, attackPoint, ref Targets
    }
}