using System.Collections;
using EnumTypes;
using Structs;
using UnityEngine;

public class MoveCommand : IUnitCommand
{
    Unit subject;
    StageCell startCell;
    StageCell destinationCell;

    private Quaternion preRotation;
    public Unit GetUnit()
    {
        return subject;
    }

    public CommandContext GetContext()
    {
        return new CommandContext();
    }

    public UnitType UnitType { get; }

    public MoveCommand(Unit unit, StageCell to)
    {
        subject = unit;
        startCell = StageManager.Instance.cellMaps[unit.curCoord];
        destinationCell = to;
    }


    public IEnumerator Execute()
    {
        preRotation = subject.transform.rotation;
        
        yield return subject.StartCoroutine(subject.Movement.Move(destinationCell));
    }

    // TODO: 언두 기능 개발
    // public void Undo()
    // {
    //     subject.Movement.InstantMove(startCell);
    //     subject.transform.rotation = preRotation;
    //     
    //     var animation = subject.GetComponent<UnitAnimation>();
    //     var cover = subject.GetComponent<UnitCoverSystem>();
    //     
    //     animation.SetIdle();
    //     
    //     Transform coverPoint = cover.CheckCoverPoint(subject.curCoord, UnitType.PlayableUnit, out int obstacleType);
    //     if(coverPoint != null)
    //         cover.Cover(coverPoint, obstacleType);
    // }
}