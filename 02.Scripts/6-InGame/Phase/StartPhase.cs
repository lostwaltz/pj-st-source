using EnumTypes;
using UnityEngine;

public class StartPhase : BasePhase
{
    public StartPhase(GamePhaseMachine machine) : base(machine) { }

    public override void Enter()
    {
        //게임시작 시 바로 스타트 스테이트!
        GameManager.Instance.SetGamePhase(GamePhase.Start);

        // 맵 전체 셀 표시
        //StageManager.Instance.ShowStageCells(0, Vector2.zero, 3);

        //스테이지so의 플레이어배치정보 셀만 켜주게 넣어놨음
        StageManager.Instance.HideStageCells();
        StageSO stageData = Core.DataManager.SelectedStage;
        var interaction = StageManager.Interaction;
        foreach (var placement in stageData.playerPlacements)
        {
            if (!placement.isPlaced)
            {
                interaction.ShowCells(placement.coord, 0, interaction.AddCostForMove, interaction.FilterForMove);
            }
        }

        Core.EventManager.Subscribe<GameStartEvent>(GameStart);
    }

    private void GameStart(GameStartEvent eventData)
    {
        Proceed();
    }
    

    public override void Exit()
    {
        StageManager.Instance.HideStageCells();
    }

    public override void Proceed()
    {
        GameManager.Instance.nextPhase = GamePhase.PlayerTurn;
        Machine.ChangePhase(Machine.TurnStartPhase);
    }
}
