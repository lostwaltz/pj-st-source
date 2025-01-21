using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;

public abstract class TurnPhase : BasePhase
{
    public abstract List<Unit> Units { get; }
    
    public TurnPhase(GamePhaseMachine machine) : base(machine){}

    public override void Enter()
    {
        for (int i = 0; i < Units.Count; i++)
            Units[i].CommandSystem.Standby();
    }
    
    public int GetNextUnit()
    {
        for (int i = 0; i < Units.Count; i++)
        {
            if (!Units[i].CommandSystem.IsComplete)
                return i; // 행동이 남은 유닛 반환
        }

        return -1;
    }

    public bool CheckTurnEnd(out int nextUnit)
    {
        nextUnit = -1;
        int remainOppositeCount = GameManager.Instance.currentPhase == GamePhase.PlayerTurn ? 
            GameUnitManager.Instance.Units[GameUnitManager.Enemy].Count : 
            GameUnitManager.Instance.Units[GameUnitManager.Playable].Count;
    
        // 1. 살아있는 적이 없는 경우
        if (remainOppositeCount == 0)
            return true;
        
        // 행동이 완료되지 않은 유닛 탐색
        // -1 반환 : 모든 캐릭터가 완료 
        nextUnit = GetNextUnit();
        
        // 2. 유닛들의 행동이 완료 여부 확인
        return nextUnit < 0;
    }
}