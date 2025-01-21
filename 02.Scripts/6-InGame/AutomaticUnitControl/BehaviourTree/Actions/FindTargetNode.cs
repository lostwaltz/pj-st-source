using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;

namespace UnitBT
{
    public class FindTargetNode : IBehaviourNode
    {
        // 우선 순위
        // 정보 기입할 곳 필요
        
        // 정렬
        // 체력 순
        // 거리 순

        public BehaviourStatus Execute()
        {
            Unit unit = AutomaticUnitController.Context.Subject;

            UnitType oppositeType = unit.type == UnitType.PlayableUnit ? UnitType.EnemyUnit : UnitType.PlayableUnit;
            List<Unit> opposites = GameUnitManager.Instance.Units[oppositeType];
            
            Unit target = opposites.
                OrderBy(n=>n.HealthSystem.Health).
                ThenBy(n=>(unit.curCoord - n.curCoord).sqrMagnitude).
                FirstOrDefault();

            if (Random.Range(0, 100) < 40 || opposites.Count != 0)
                target = opposites[Random.Range(0, opposites.Count)];
            
            if (target == null)
                return BehaviourStatus.Failure;
            
            AutomaticUnitController.Context.Target = target;
            return BehaviourStatus.Success;
            
            // 지금 현재 내 타입을 기반으로 내 적을 정렬된 기준으로 찾는다
            // 체력 낮은놈 우선 
            // 제일 가까운 놈
        }
    }
}