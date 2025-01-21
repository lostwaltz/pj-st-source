using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;

namespace UnitBT
{
    public class FindLowHealthTeamNode : IBehaviourNode
    {
        private IBehaviourNode behaviourNodeImplementation;
        

        public BehaviourStatus Execute()
        {
            Unit unit = AutomaticUnitController.Context.Subject;
            
            List<Unit> teamUnit = GameUnitManager.Instance.Units[unit.type];
            
            Unit target = teamUnit.
                OrderBy(n=>n.HealthSystem.Health).
                ThenBy(n=>(unit.curCoord - n.curCoord).sqrMagnitude).
                FirstOrDefault();
            
            if (target == null || target.HealthSystem.Health >= target.HealthSystem.MaxHealth)
                return BehaviourStatus.Failure;
            
            AutomaticUnitController.Context.Target = target;
            return BehaviourStatus.Success;
        }
    }
}
