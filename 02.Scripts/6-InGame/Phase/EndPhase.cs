using UnityEngine;
using EnumTypes;

public class EndPhase : BasePhase
{
    public EndPhase(GamePhaseMachine machine) : base(machine){}

    public override void Enter()
    {
        GameManager.Instance.SetGamePhase(GamePhase.End);
        Proceed();
    }

    public override void Exit()
    {
    }

    public override void Proceed()
    {
    }
}
