using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

public class UnitMovement : MonoBehaviour, ICommandReceiver
{
    protected Unit unit;
    protected UnitAnimation animation;
    protected UnitCoverSystem cover;
    
    [SerializeField] LayerMask obstacleMask = 1 << 12;

    WaitForSeconds waitForSec = new WaitForSeconds(0.5f);
    
    public int Option { get; set; }
    public Vector2 CommandCoord { get; set; }
    public virtual ReceiverStep ReceiverStep { get; set; }

    public void Initialize(Unit owner)
    {
        unit = owner;
        animation = unit.Animation;
        cover = unit.CoverSystem;

        unit.CommandSystem.OnCommandEnded += PostAction;
    }
    
    // 즉시 이동 : Undo에서 쓰기 위함
    public void InstantMove(StageCell destination)
    {
        StageManager.Instance.cellMaps[unit.curCoord].UnitExit();

        transform.position = destination.transform.position;
        unit.curCoord = destination.placement.coord;
        
        StageManager.Instance.cellMaps[unit.curCoord].UnitEnter(unit);
    }
    public IEnumerator Move(StageCell destination)
    {
        StageCell start = StageManager.Instance.cellMaps[unit.curCoord];
        StageManager.PathFinding.GetPath(start, destination, out List<StageCell> cells);
        StageManager.Instance.cellMaps[unit.curCoord].UnitExit();
    
        animation.SetRun();

         // 이전 프레임 위치
        foreach (StageCell cell in cells)
        {
            Vector3 to = cell.transform.position;
            transform.rotation = Quaternion.LookRotation(to - transform.position);

            bool reachedDestination = false;
            Vector3 prevPosition = transform.position;

            while (!reachedDestination)
            {
                if (CheckObstacle())
                    animation.ReactObstacle();

                yield return null;

                float distance = (transform.position - to).sqrMagnitude;

                if ((transform.position - prevPosition).sqrMagnitude > distance ||
                    distance <= 0.1f)
                {
                    transform.position = to;
                    reachedDestination = true;
                }

                prevPosition = transform.position;
            }
        }
        
        unit.curCoord = destination.placement.coord;
        StageManager.Instance.cellMaps[unit.curCoord].UnitEnter(unit);

        animation.SetIdle();
        yield return waitForSec;
    }

    
    bool CheckObstacle()
    {
        if (animation.isOnObstacle) 
            return false;
        
        Ray ray = new Ray(transform.position + Vector3.up * 0.3f, transform.forward);
        var hits = Physics.RaycastAll(ray, 0.2f, obstacleMask);
        if (hits.Length == 0)
            return false;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.TryGetComponent(out StageObstacle obstacle) && obstacle.placement.cost > 0)
                return true;
        }

        return false;
    }

    public void PostAction()
    {
        // 후속 행동 : 엄폐하기
        Transform point = cover.CheckCoverPoint(unit.curCoord, unit.type, out int coverType);
        if(point != null)
            cover.Cover(point, coverType);
    }
    
    
    public bool Ready(int opt, bool isReverted = false)
    {
        Option = opt;

        if (!isReverted)
        {
            ReceiverStep = ReceiverStep.Ready;
            CommandCoord = unit.curCoord;   
        }
        
        GameManager.Instance.Indicator.ShowStageCell(Option, unit.curCoord, unit.data.UnitBase.StepRange);

        // 디버프 등으로 인해서 못 움직이는 경우 등을 위해서 return을 만듦
        // 조건에 걸리면 상단에서 return false 해줄 것
        return true; 
    }

    public void Revert()
    {
        if (ReceiverStep == ReceiverStep.Ready)
        {
            return;
        }
        
        GameManager.Instance.Indicator.HideMovementIndicator();
        ReceiverStep = ReceiverStep.Ready;
    }

    public void Clear()
    {
        GameManager.Instance.Indicator.HideStageCell();
    }

    public void Interact()
    {
        if (GameManager.Instance.Interaction.Click(out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out StageCell cell))
            {
                // 인디케이터 표기
                GameManager.Instance.Indicator.ShowMovementIndicator(unit, cell.placement.coord);
                
                // 사운드 호출
                UIBattle.PlayUIBattleClickVirtualMove();
                
                // 명령 업데이트
                CommandCoord = cell.placement.coord;
                unit.CommandSystem.UpdateCommand(Option, CommandCoord, CreateCommand);
                
                // 리시버 상태 업데이트
                ReceiverStep = ReceiverStep.Determine;
            }
            else if (hit.collider.TryGetComponent(out Unit hitUnit))
            {
                if (hitUnit is EnemyUnit) // 공격으로 전환, 0번 스킬 선택
                    GameManager.Instance.Mediator.CallSkillSelect(0);
                else // 조종할 플레이어 교체
                    GameManager.Instance.Interaction.SelectPlayerUnit(hitUnit as PlayerUnit);
            }
            else
            {
                // TODO : UI 표시 - 잘못된 대상 선택
            }
            
        }
    }
    
    public IUnitCommand CreateCommand()
    {
        return new MoveCommand(unit, StageManager.Instance.cellMaps[CommandCoord]);
    }
}
