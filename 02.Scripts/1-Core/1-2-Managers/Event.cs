using System;
using System.Collections.Generic;
using System.Data;
using EnumTypes;

public class EventBase : EventArgs
{

}

public class InteractionUIUnitInstance : EventBase
{
    public readonly UnitInstance UnitInstance;
    public InteractionUIUnitInstance(UnitInstance unitInstance)
    {
        UnitInstance = unitInstance;
    }
}

public class ShowEndTurnCanvasUIEvent : EventBase
{
    public readonly UnitType UnitType;

    public ShowEndTurnCanvasUIEvent(UnitType unitType)
    {
        UnitType = unitType;
    }
}

public class ExecuteCommandEvent : EventBase
{
    public readonly IUnitCommand Command;
    public readonly CommandPhase CommandPhase;
    public readonly List<IUnitCommand> PushCommandList;
    
    public ExecuteCommandEvent(IUnitCommand command, List<IUnitCommand> pushCommandList, CommandPhase commandPhase)
    {
        Command = command;
        PushCommandList = pushCommandList;
        CommandPhase = commandPhase;
    }
}

public class TurnEnterCommandEvent : EventBase
{
    public readonly GamePhase GamePhase;
    public readonly Queue<IUnitCommand> CommandQueue;

    public TurnEnterCommandEvent(GamePhase gamePhase, Queue<IUnitCommand> commandQueue)
    {
        this.GamePhase = gamePhase;
        CommandQueue = commandQueue;
    }
}

public class ExecuteAttackCommandEvent : EventBase
{
    public readonly bool IsDone;
    public readonly Unit PlayerUnit;
    public readonly List<Unit> TargetUnits;

    public ExecuteAttackCommandEvent(bool isDone, Unit playerUnit, List<Unit> targets)
    {
        PlayerUnit = playerUnit;
        IsDone = isDone;
        TargetUnits = targets;
    }
}

public class ExecutePassiveAttackCommandEvent : EventBase
{
    public readonly bool IsDone;
    public readonly Unit PlayerUnit;
    public readonly Unit PassiveTargetUnit;

    public ExecutePassiveAttackCommandEvent(bool isDone, Unit playerUnit,Unit passiveTargetUnit)
    {
        PlayerUnit = playerUnit;
        PassiveTargetUnit = passiveTargetUnit;
        IsDone = isDone;
    }
}


public class AchievementEvent : EventBase
{
    public readonly ExternalEnums.AchActionType AchAction;
    public readonly ExternalEnums.AchTargetType AchTarget;
    
    public readonly float Amount;
    public readonly int ID;

    public AchievementEvent(ExternalEnums.AchActionType achAction, ExternalEnums.AchTargetType achTarget, float amount, int id = -1)
    {
        AchAction = achAction;
        AchTarget = achTarget;
        Amount = amount;
        ID = id;
    }
}

public class UnitOnDeathEvent : EventBase
{
    public readonly UnitType UnitType;

    public UnitOnDeathEvent(UnitType unitType)
    {
        UnitType = unitType;
    }
}

public class GuidePopUpEvent : EventBase
{
    public GuideTrigger Trigger;
    public GuidePopUpEvent(GuideTrigger trigger)
    {
        Trigger = trigger;
    }
}

public class GameStartEvent : EventBase
{
    
}