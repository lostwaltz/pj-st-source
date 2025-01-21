namespace UnitBT
{
    public class ExecuteNode : IBehaviourNode
    {
        // 가장 최후에 실행될 것
        public BehaviourStatus Execute()
        {
            AutomaticUnitController.Context.Subject.CommandSystem.ExecuteCommand();
            return BehaviourStatus.Success;
        }
    }
}