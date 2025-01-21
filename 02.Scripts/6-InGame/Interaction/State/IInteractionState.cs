namespace Refactor
{
    public interface IInteractionState
    {
        InteractionStateMachine Machine { get; set; }
        
        void Enter();
        void Exit();
    
        // void Interact();
        
    }    
}
