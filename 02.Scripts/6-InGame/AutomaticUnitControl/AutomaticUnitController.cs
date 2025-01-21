using System.Collections.Generic;
using EnumTypes;
using UnitBT;
using UnityEngine;

public enum BehaviourType
{
    Naive,      // 단순
    General,    // 일반
    Support,
    Boss,       // 보스
}

public class AutomaticUnitController
{
    public static BehaviourContext Context { get; private set; }

    public TurnPhase TurnHandler { get; set; }

    // 이 컨트롤러에서 로직으로 명령을 구성
    // TODO : BT 관리자나 빌더 필요
    Dictionary<BehaviourType, IBehaviourNode> behaviours = new();

    public AutomaticUnitController()
    {
        // TODO : 플레이어 자동 플레이때 더 고도화할 것
        GameManager.Instance.CommandController.OnPostProcessed += (phase) =>
        {
            if (phase == GamePhase.EnemyTurn)
                ProceedTurn();
        };

        Context = new BehaviourContext();

        IBehaviourNode naive = BehaviourFactory.CreateBehaviour(BehaviourType.Naive);
        IBehaviourNode general = BehaviourFactory.CreateBehaviour(BehaviourType.General);
        IBehaviourNode support = BehaviourFactory.CreateBehaviour(BehaviourType.Support);

        IBehaviourNode boss = BossBehaviourFactory.CreateBehaviour();

        behaviours.Add(BehaviourType.Boss, boss);
        behaviours.Add(BehaviourType.Naive, naive);
        behaviours.Add(BehaviourType.General, general);
        behaviours.Add(BehaviourType.Support, support);
    }

    public void ControlUnit(Unit unit)
    {
        Context.Subject = unit;
        Context.Target = null;
        Context.AttackCoord = unit.curCoord;

        BehaviourType type = (BehaviourType)unit.data.UnitBase.Behaviour;
        
        behaviours[type].Execute();
        Context.Subject.CommandSystem.ExecuteCommand();
    }

    public void ProceedTurn()
    {
        TurnHandler.Proceed();
    }

}