using EnumTypes;
using UnityEngine;

public class TurnEndPhase : BasePhase
{
    public TurnEndPhase(GamePhaseMachine machine) : base(machine)
    {
        InitConditions();
    }

    private CombatConditionHandler combatConditionHandler;

    public override void Enter()
    {
        Proceed();
    }

    public void InitConditions()
    {
        combatConditionHandler = new CombatConditionHandler();
        combatConditionHandler.InitConditions(StageManager.Instance.stageData.winConditions,
            StageManager.Instance.stageData.lostConditions);
    }

    public override void Exit() { }

    public override void Proceed()
    {
        if (CheckGameEnd(out var isPlayerWin))
        {
            if (isPlayerWin)
            {
                Core.UIManager.OpenUI<UICombatResult>();
                
                // Core.DataManager.StageClearDict[StageManager.Instance.stageData] = true;
                // Core.SaveStageCleared(StageManager.Instance.stageData.stageName, true);
                Core.DataManager.StageClearData[StageManager.Instance.stageData.stageData.StageKey] = true;

                Core.EventManager.Publish(new AchievementEvent(ExternalEnums.AchActionType.Clear,
                    ExternalEnums.AchTargetType.Stage, StageManager.Instance.stageData.stageData.StageKey));
            }
            else
            {
                UIGameOver uiGameOver = Core.UIManager.GetUI<UIGameOver>();
                uiGameOver.UpdateUI(StageManager.Instance.stageData.stageName, combatConditionHandler.CurSimpleDesc);
                uiGameOver.Open();
            }

            Machine.ChangePhase(Machine.EndPhase);
        }
        else
        {
            GameManager.Instance.nextPhase = GameManager.Instance.currentPhase == GamePhase.PlayerTurn ?
                    GamePhase.EnemyTurn : GamePhase.PlayerTurn;

            Machine.ChangePhase(Machine.TurnStartPhase);
        }
    }


    bool CheckGameEnd(out bool isPlayerWin)
    {
        isPlayerWin = combatConditionHandler.WinConditionsMet();

        bool result = isPlayerWin ||
                      combatConditionHandler.LoseConditionsMet();

        return result;
    }
}