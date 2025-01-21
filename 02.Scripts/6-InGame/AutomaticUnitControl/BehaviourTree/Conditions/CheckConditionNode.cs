using System;

namespace UnitBT
{
    public class CheckConditionNode : ConditionBaseNode
    {
        private readonly Func<Unit, bool> condition;
        
        public CheckConditionNode(Func<Unit, bool> condition, IBehaviourNode onSuccess, IBehaviourNode onFail = null)
            : base(onSuccess, onFail)
        {
            this.condition = condition;
        }
        
        public override BehaviourStatus Execute()
        {
            Unit unit = AutomaticUnitController.Context.Subject;
            
            if (unit == null)
                return BehaviourStatus.Failure;

            if (condition(unit))
                return success?.Execute() ?? BehaviourStatus.Failure;

            return failure?.Execute() ?? BehaviourStatus.Failure;
        }
    }
}