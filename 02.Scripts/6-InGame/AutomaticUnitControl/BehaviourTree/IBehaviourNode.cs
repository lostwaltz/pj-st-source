namespace UnitBT
{
    public enum BehaviourStatus
    {
        Success,
        Failure,
        Running
    }

    public interface IBehaviourNode
    {
        BehaviourStatus Execute();
    }
}
