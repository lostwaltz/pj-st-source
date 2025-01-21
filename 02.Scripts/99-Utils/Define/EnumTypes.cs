namespace EnumTypes
{
    public enum UIEvent
    {
        Click,
        Drag,
        BeginDrag,
        EndDrag,
        Enter,
        Exit,
        Down,
        Up
    }
    
    public enum Sound
    { 
        BGM,
        Effect
    }
    public enum GamePhase
    {
        Start,
        PlayerTurn,
        EnemyTurn,
        ProcessCommand,
        End,
        PhaseCount
    }

    public enum GameResult
    {
        None,
        Win,
        Lose,    
    }
    
    public enum UnitType
    {
        PlayableUnit = 1, 
        EnemyUnit = 0
    }
    
    public enum PlayerCommandType
    {
        None = -1,
        Move = 0,
        UseSkill = 1,
    }
    
    // 행동 개시, 턴 시작 등 공통된 타이밍에 발동하는 패시브를 위함
    public enum CommandPhase
    {
        BeforeCommand,
        AfterCommand,
    }

}
