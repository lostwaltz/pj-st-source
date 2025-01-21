using System.Collections.Generic;
using EnumTypes;
using Refactor;
using UnityEngine;

public class PlayerTurnPhase : TurnPhase
{
    public override List<Unit> Units => GameUnitManager.Instance.Units[UnitType.PlayableUnit];
    PlayerInteraction Interaction => GameManager.Instance.Interaction;
    AutomaticUnitController AutoController => GameManager.Instance.AutoController;

    public bool IsAutoPlay = false;
    
    private bool isInPlayerTurn;

    public PlayerTurnPhase(GamePhaseMachine machine) : base(machine)
    {
        Interaction.TurnHandler = this; // new
        GameManager.Instance.ToggleAutoBtn += ChangePlayMode;
    }

    public override void Enter()
    {
        base.Enter();

        isInPlayerTurn = true;
        
        if(IsAutoPlay)
            AutoController.TurnHandler = this;

        GameManager.Instance.CommandController.ExecuteCommandOnTurnEnter(GamePhase.PlayerTurn, CommandEvent);
    }

    private void CommandEvent()
    {
        switch (IsAutoPlay)
        {
            case true:
                AutoProceed();
                break;
            case false:
                Interaction.Activate();
                CameraSystem.EventHandler.Publish(CameraEventTrigger.OnPlayerTurn, new CameraEventContext(Units[0]));
                break;
        }
    }

    public override void Exit()
    {
        Interaction.Deactivate();
        isInPlayerTurn = false;
    }

    public override void Proceed()
    {
       // 턴 종료 확인
        if (CheckTurnEnd(out int nextUnit))
        {
            Machine.ChangePhase(Machine.TurnEndPhase);
            return;
        }
        Interaction.SelectPlayerUnit(Units[nextUnit] as PlayerUnit);
        CameraSystem.EventHandler.Publish(CameraEventTrigger.OnPlayerTurn, new CameraEventContext(Units[nextUnit]));
    }

    public void AutoProceed()
    {
        if (CheckTurnEnd(out int nextUnit))
        {
            Machine.ChangePhase(Machine.TurnEndPhase);
            return;
        }
        
        AutoController.ControlUnit(Units[nextUnit]);
    }

    private void ChangePlayMode()
    {
        Utils.ToggleTrigger(ref IsAutoPlay, ChangeAutoHandle, ChangeUserHandle);
    }

    private void ChangeUserHandle()
    {
        if(Interaction.TurnHandler != this) return;
        if(!isInPlayerTurn) return;
        if (GameManager.Instance.Interaction.StateMachine.CurState is not CommandState) return;
        
        GameManager.Instance.CommandController.OnPostProcessed -= SetInteraction;
        GameManager.Instance.CommandController.OnPostProcessed += SetInteraction;
    }

    private void SetInteraction(GamePhase _)
    {
        GameManager.Instance.CommandController.OnPostProcessed -= SetInteraction;
        
        Interaction.Activate();

        if(CheckTurnEnd(out int nextUnit))
            return;
        
        Interaction.SelectPlayerUnit(Units[nextUnit] as PlayerUnit);
    }

    private void ChangeAutoHandle()
    {
        if(Interaction.TurnHandler != this) return;
        if(!isInPlayerTurn) return;
        if(GameManager.Instance.CommandController.CommandCoroutineHandle != null)
            return;
        
        Interaction.Deactivate();
        Core.UIManager.GetUI<UIBattleCanvas>().CloseUIPlayerableUnit();
        foreach (var unit in GameUnitManager.Instance.Units[UnitType.PlayableUnit])
            unit.CommandSystem.ClearCommand();
        
        AutoProceed();
    }
}