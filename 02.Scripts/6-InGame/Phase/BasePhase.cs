public abstract class BasePhase : IGamePhase
{
    public GamePhaseMachine Machine { get; private set; }

    public BasePhase(GamePhaseMachine machine)
    {
        Machine = machine;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Proceed();
}
