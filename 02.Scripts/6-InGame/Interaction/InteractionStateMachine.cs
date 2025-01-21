namespace Refactor
{
    public class InteractionStateMachine
    {
        public PlayerInteraction Interaction => GameManager.Instance.Interaction;

        public IInteractionState CurState { get; private set; }
        public SelectState SelectState { get; private set; }
        public CommandState CommandState { get; private set; }
        public PauseState PauseState { get; private set; }

        public InteractionStateMachine()
        {
            SelectState = new SelectState(this);
            CommandState = new CommandState(this);
            PauseState = new PauseState(this);
        }

        public void ChangeState(IInteractionState newState)
        {
            CurState?.Exit();
            
            CurState = newState;
            
            CurState.Enter();
        }

    }
}