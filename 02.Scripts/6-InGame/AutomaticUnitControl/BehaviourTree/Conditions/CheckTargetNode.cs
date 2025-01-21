namespace UnitBT
{
    public class CheckTargetNode : ConditionBaseNode
    {
        public CheckTargetNode(IBehaviourNode onSuccess, IBehaviourNode onFail = null) : base(onSuccess, onFail)
        {
        }
        
        public override BehaviourStatus Execute()
        {
            if (AutomaticUnitController.Context.Target != null)
                return success?.Execute() ?? BehaviourStatus.Failure;
            else
                return failure?.Execute() ?? BehaviourStatus.Failure;
        }
    }
}