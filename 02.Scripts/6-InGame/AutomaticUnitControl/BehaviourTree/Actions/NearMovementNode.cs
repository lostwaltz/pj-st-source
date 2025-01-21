using System.Collections.Generic;
using UnityEngine;

namespace UnitBT
{
    public class NearMovement : IBehaviourNode
    {
        public BehaviourStatus Execute()
        {
            BehaviourContext context = AutomaticUnitController.Context;
            Unit subject = context.Subject;
            Unit target = context.Target;
            
            Vector2 optimalPosition = FindOptimalPosition(subject, target);
            if (optimalPosition == subject.curCoord)
                return BehaviourStatus.Success; // 이미 최적의 위치에 있음

            // 이동 명령 생성 및 실행
            context.AttackCoord = optimalPosition;
            StageCell destination = StageManager.Instance.cellMaps[optimalPosition];

            subject.CommandSystem.UpdateCommand(0, optimalPosition, () =>
            {
                return new MoveCommand(subject, destination);
            });
            
            return BehaviourStatus.Success;
        }

        private Vector2 FindOptimalPosition(Unit subject, Unit target)
        {
            // 이동 가능한 모든 위치 가져오기
            List<Vector2> possiblePositions = GetPossibleMovePositions(subject);
            if (possiblePositions.Count == 0)
                return subject.curCoord;

            Vector2 bestPosition = subject.curCoord;
            float shortestDistance = float.MaxValue;

            foreach (Vector2 pos in possiblePositions)
            {
                // 대상과의 거리 계산
                float distance = Vector2.Distance(pos, target.curCoord);

                // 더 짧은 거리를 발견하면 최적 위치 갱신
                if (!(distance < shortestDistance)) continue;
                
                shortestDistance = distance;
                bestPosition = pos;
            }

            return bestPosition;
        }

        
        private List<Vector2> GetPossibleMovePositions(Unit subject)
        {
            List<Vector2> positions = new List<Vector2>();
            int range = subject.data.UnitBase.StepRange;

            // 이동 범위 내의 모든 좌표 확인
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    Vector2 pos = new Vector2(subject.curCoord.x + x, subject.curCoord.y + y);
                    if (IsValidPosition(pos))
                        positions.Add(pos);
                }
            }

            return positions;
        }
        
        private bool IsValidPosition(Vector2 pos)
        {
            return StageManager.Instance.cellMaps.ContainsKey(pos) &&          // 맵 내 존재하는 위치
                   !StageManager.Instance.obstaclesMaps.ContainsKey(pos) &&    // 장애물이 없는 위치
                   StageManager.Instance.cellMaps[pos].unitIndexInCell < 0 &&  // 다른 유닛이 없는 위치
                   StageManager.Instance.cellMaps[pos].Cost >= 0;              // 이동 가능한 비용
        }
    }
}