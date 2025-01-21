public class GamePhaseMachine
{
    public int TurnCount { get; set; }
    
    public GameCommandController CommandController { get; private set;}

    public IGamePhase CurrentPhase { get; private set; }
    
    public IGamePhase StartPhase { get; private set; }
    public IGamePhase TurnStartPhase { get; private set; }
    public IGamePhase PlayerTurnPhase { get; private set; }
    public IGamePhase EnemyTurnPhase { get; private set; }
    public IGamePhase TurnEndPhase { get; private set; }
    public IGamePhase EndPhase { get; private set; }

    public GamePhaseMachine()
    {
        StartPhase = new StartPhase(this);
        TurnStartPhase = new TurnStartPhase(this);
        PlayerTurnPhase = new PlayerTurnPhase(this);
        EnemyTurnPhase = new EnemyTurnPhase(this);
        TurnEndPhase = new TurnEndPhase(this);
        EndPhase = new EndPhase(this);
        
        CommandController = GameManager.Instance.CommandController;
    }

    public void ChangePhase(IGamePhase newPhase)
    {
        CurrentPhase?.Exit();

        CurrentPhase = newPhase;

        CurrentPhase?.Enter();
    }

    
}
