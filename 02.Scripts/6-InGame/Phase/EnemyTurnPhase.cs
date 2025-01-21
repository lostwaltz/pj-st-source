using System.Collections.Generic;
using EnumTypes;
using Structs;
using UnityEngine;

public class EnemyTurnPhase : TurnPhase
{
    public override List<Unit> Units => GameUnitManager.Instance.Units[UnitType.EnemyUnit];
    AutomaticUnitController AutoController => GameManager.Instance.AutoController;
    
    
    public EnemyTurnPhase(GamePhaseMachine machine) : base(machine) {}

    
    public override void Enter()
    {
        base.Enter();
        
        AutoController.TurnHandler = this;
        
        GameManager.Instance.CommandController.ExecuteCommandOnTurnEnter(GamePhase.EnemyTurn, Proceed);
    }

    public override void Exit()
    {
    }

    public override void Proceed()
    {
        if (CheckTurnEnd(out int nextUnit))
        {
            Machine.ChangePhase(Machine.TurnEndPhase);
            return;
        }
        
        ProcessEnemy(nextUnit);
    }

    void ProcessEnemy(int enemyIndex)
    {
        AutoController.ControlUnit(Units[enemyIndex]);    
    }

}
