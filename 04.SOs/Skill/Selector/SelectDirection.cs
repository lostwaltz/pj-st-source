using System.Collections.Generic;
using EnumTypes;
using UnityEngine;


[CreateAssetMenu(fileName = "Select Direction", menuName = "SOs/Selector/Direction")]
public class SelectDirection : SelectorSO
{
    public override void ShowSelectionRange(Skill skill, Vector2 coord)
    {
        GameManager.Instance.Indicator.HideStageCell();
        
        int[,] fourDir = Define.FourDirection;
        for (int i = 0; i < fourDir.GetLength(1); i++)
        {
            Vector2 dirCoord = coord + new Vector2(fourDir[0, i], fourDir[1, i]);
            if (StageManager.Instance.cellMaps.TryGetValue(dirCoord, out StageCell cell))
            {
                var comp = GameManager.Instance.Indicator.Get<IndicatorDirectionCell>();
                Vector3 dir = cell.placement.position - StageManager.Instance.cellMaps[coord].placement.position;
                comp.transform.rotation = Quaternion.LookRotation(dir);
                comp.Show(dirCoord);
            }
        }
    }

     public override void Select(Skill skill, ref Vector2 start, ref Vector2 target, ref List<Unit> targets)
     {
         // 기존 인디케이터 숨기기
         GameManager.Instance.Indicator.HideAttackRelatedIndicator();
      
         Vector2 direction = (target - start).normalized; // (0,-1),(0,1),(-1,0),(1,0)
         
         var maps = StageManager.Instance.cellMaps;
         UnitType targetType = (UnitType)skill.Data.SkillBase.TargetType;

         Vector2 startPoint = target;  // 시작 좌표
         int horizontalRange = skill.Data.SkillBase.ScopeWidth; // 가로 범위
         int verticalRange = skill.Data.SkillBase.ScopeHeight;  // 세로 범위

         List<Vector2> frontCoords = StageManager.Interaction.GetFrontCoord(startPoint, horizontalRange, verticalRange, direction);
         
         foreach (Vector2 coord in frontCoords)
         {
             if (!maps.ContainsKey(coord)) continue;
             
             GameManager.Instance.Indicator.Show<IndicatorRangeCell>(coord); // 바닥에 생기는 Indicator

             // 유닛 데이터 확인
             int idx = maps[coord].unitIndexInCell;
             UnitType inCell = maps[coord].unitTypeInCell;
             
             if (IsValidUnit(idx, targetType, inCell) && maps[coord].unitTypeInCell.Equals(targetType))
             {
                 targets.Add(GameUnitManager.Instance.UnitDic[targetType][idx]);
             }
         }
         
         GameManager.Instance.Indicator.ShowAttackIndicator(start, target);
         ShowTargetIndicator(start, target, skill.OwnerUnit, ref targets);
     }

    public override bool Interact(RaycastHit hit, UnitType type, out Vector2 targetCoord)
    {
        // 기본 상호작용 대상 : 유닛
        if (hit.collider.TryGetComponent(out IndicatorDirectionCell cell))
        {
            targetCoord = cell.Coord;
            return true;
        }
        
        targetCoord = Vector2.zero;
        return false;
    }
}