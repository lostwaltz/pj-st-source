namespace UnitBT
{
    public abstract class ConditionBaseNode : IBehaviourNode
    {
        protected IBehaviourNode success;
        protected IBehaviourNode failure;

        public ConditionBaseNode(IBehaviourNode onSuccess, IBehaviourNode onFail = null)
        {
            success = onSuccess;
            failure = onFail;
        }

        public abstract BehaviourStatus Execute();
    }
}