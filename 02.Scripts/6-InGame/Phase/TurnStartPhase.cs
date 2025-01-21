using System.Collections;
using EnumTypes;
using UnityEngine;

public class TurnStartPhase : BasePhase
{
    WaitForSeconds waitForSeconds = new (1f);
    
    public TurnStartPhase(GamePhaseMachine machine) : base(machine) {}
    
    public override void Enter()
    {
        // UI 이벤트 호출
        Machine.TurnCount++;
        GameManager.Instance.SetGamePhase(GameManager.Instance.nextPhase);
        
        GameManager.Instance.StartCoroutine(TurnStartRoutine());
    }

    public override void Exit() {}

    public override void Proceed()
    {
        if (GameManager.Instance.currentPhase == GamePhase.PlayerTurn)
            Machine.ChangePhase(Machine.PlayerTurnPhase);
        else if (GameManager.Instance.currentPhase == GamePhase.EnemyTurn)
            Machine.ChangePhase(Machine.EnemyTurnPhase);
    }

    IEnumerator TurnStartRoutine()
    {
        yield return waitForSeconds;
        Proceed();
    }
}