using System;
using System.Collections.Generic;

public enum CameraEventTrigger
{
    OnPlayerTurn,
    OnPlayerClicked,
    // OnEnemyTurn, // 다시 생각해보니 필요없을 듯합니다. 적들은 바로 행동을 개시할 거라서 별도로 호출하지 않을게요
    OnUnitMove,
    OnUnitActivateSkill,
    OnUnitActivatePassive,
    
    COUNT
}

public struct CameraEventContext
{
    public Unit Subject;
    public IUnitCommand Command;

    public CameraEventContext(Unit subject, IUnitCommand command)
    {
        Subject = subject;
        Command = command;
    }
    
    public CameraEventContext(Unit subject)
    {
        Subject = subject;
        Command = null;
    }
    
    public CameraEventContext(IUnitCommand command)
    {
        Subject = null;
        Command = command;
    }
}

public class CameraEventHandler
{
    private Dictionary<CameraEventTrigger, Action<CameraEventContext>> events = new();

    public CameraEventHandler()
    {
        // 초기화
        int cnt = (int)CameraEventTrigger.COUNT;
        
        for (int i = 0; i < cnt; i++)
            events.Add((CameraEventTrigger)i, null);
         
    }
    
    public void Subscribe(CameraEventTrigger eventType, Action<CameraEventContext> action)
    {
        if (events.TryGetValue(eventType, out Action<CameraEventContext> eventAction))
            events[eventType] += action;
    }

    public void Unsubscribe(CameraEventTrigger eventType, Action<CameraEventContext> action)
    {
        if (events.TryGetValue(eventType, out Action<CameraEventContext> eventAction))
            events[eventType] -= action;
    }

    public void Publish(CameraEventTrigger eventType, CameraEventContext context)
    {
        events[eventType]?.Invoke(context);
    }
}
