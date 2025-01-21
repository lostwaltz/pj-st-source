namespace UnitBT
{
    public class CheckAttackRangeNode : ConditionBaseNode
    {
        public CheckAttackRangeNode(IBehaviourNode onSuccess, IBehaviourNode onFail = null) : base(onSuccess, onFail){}

        public override BehaviourStatus Execute()
        {
            Unit unit = AutomaticUnitController.Context.Subject;
            Unit target = AutomaticUnitController.Context.Target;

            if (unit.IsInAttackRange(target.curCoord, AutomaticUnitController.Context.AttackCoord))
                return success?.Execute() ?? BehaviourStatus.Failure;
            else
                return failure?.Execute() ?? BehaviourStatus.Failure;
        }
    }
}