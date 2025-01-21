using Refactor;

public class PauseState : IInteractionState
{
    public InteractionStateMachine Machine { get; set; }

    public PauseState(InteractionStateMachine machine)
    {
        Machine = machine;
    }
    
    public void Enter(){}

    public void Exit(){}
}