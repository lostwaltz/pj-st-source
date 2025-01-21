using System.Collections.Generic;
using UnityEngine;

namespace UnitBT
{
    /// <summary>
    /// 장애물 우회해서 움직이는 노드
    /// </summary>
    public class AvoidObstacleMovementNode : IBehaviourNode
    {
        public BehaviourStatus Execute()
        {
            BehaviourContext context = AutomaticUnitController.Context;

            Unit subject = context.Subject;

            Unit target = context.Target;

            // 시작 위치와 목표 위치의 StageCell 가져오기
            StageCell startCell = StageManager.Instance.cellMaps[subject.curCoord];
            StageCell targetCell = StageManager.Instance.cellMaps[target.curCoord];

            // StagePathFinding을 사용하여 경로 찾기
            List<StageCell> path = new List<StageCell>();
            StageManager.PathFinding.GetPath(startCell, targetCell, out path);

            // 경로를 찾지 못했거나 현재 위치가 최적의 위치인 경우
            if (path == null || path.Count == 0)
            {
                return BehaviourStatus.Success;
            }

            // 이동 범위 내에서 가장 멀리 갈 수 있는 위치 찾기
            StageCell nextCell = null;
            int stepRange = subject.data.UnitBase.StepRange;

            for (int i = 0; i < path.Count && i < stepRange; i++)
            {
                if (IsValidMovePosition(path[i].placement.coord))
                {
                    nextCell = path[i];
                }
            }

            if (nextCell == null)
            {
                return BehaviourStatus.Success;
            }

            // 이동 명령 생성 및 실행
            context.AttackCoord = nextCell.placement.coord;

            if (subject.CommandSystem == null)
            {
                return BehaviourStatus.Failure;
            }

            subject.CommandSystem.UpdateCommand(0, nextCell.placement.coord, () =>
            {
                return new MoveCommand(subject, nextCell);
            });

            return BehaviourStatus.Success;
        }

        private bool IsValidMovePosition(Vector2 pos)
        {
            if (!StageManager.Instance.cellMaps.ContainsKey(pos))
            {
                return false;
            }

            if (StageManager.Instance.obstaclesMaps.ContainsKey(pos))
            {
                return false;
            }

            if (StageManager.Instance.cellMaps[pos].unitIndexInCell >= 0)
            {
                return false;
            }

            if (StageManager.Instance.cellMaps[pos].Cost < 0)
            {
                return false;
            }

            return true;
        }
    }
}