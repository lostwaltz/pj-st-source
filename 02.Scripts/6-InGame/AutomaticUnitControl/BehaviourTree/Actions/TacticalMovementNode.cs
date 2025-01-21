using System.Collections.Generic;
using UnityEngine;

namespace UnitBT
{
    /// <summary>
    /// 유닛의 최적 이동 위치를 결정하는 행동 노드
    /// </summary>
    public class TacticalMovementNode : IBehaviourNode
    {
        // 위치 평가에 사용되는 가중치
        private const float COVER_SCORE_WEIGHT = 1.5f;     // 엄폐 점수 가중치
        private const float RANGE_SCORE_WEIGHT = 2.0f;     // 사거리 점수 가중치
        
        public BehaviourStatus Execute()
        {
            BehaviourContext context = AutomaticUnitController.Context;
            Unit subject = context.Subject;
            Unit target = context.Target;

            if (target == null)
                return BehaviourStatus.Failure;

            // 최적의 이동 위치 찾기
            Vector2 optimalPosition = FindOptimalPosition(subject, target);
            AutomaticUnitController.Context.AttackCoord = optimalPosition;
            
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

        /// <summary>
        /// 유닛의 최적 이동 위치를 계산
        /// </summary>
        private Vector2 FindOptimalPosition(Unit subject, Unit target)
        {
            // 이동 가능한 모든 위치 가져오기
            List<Vector2> possiblePositions = GetPossibleMovePositions(subject);
            if (possiblePositions.Count == 0)
                return subject.curCoord;

            Vector2 bestPosition = subject.curCoord;
            float bestScore = float.MinValue;

            // 각 위치의 점수를 계산하여 최적의 위치 선정
            foreach (Vector2 pos in possiblePositions)
            {
                float score = EvaluatePosition(pos, subject, target);
                
                if (score > bestScore)
                {
                    bestScore = score;
                    bestPosition = pos;
                }
            }

            Debug.Log(bestPosition);
            return bestPosition;
        }

        /// <summary>
        /// 유닛이 이동 가능한 모든 위치 목록을 반환
        /// </summary>
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
                    {
                        positions.Add(pos);
                    }
                }
            }

            return positions;
        }

        /// <summary>
        /// 해당 위치가 이동 가능한 유효한 위치인지 확인
        /// </summary>
        private bool IsValidPosition(Vector2 pos)
        {
            return StageManager.Instance.cellMaps.ContainsKey(pos) &&          // 맵 내 존재하는 위치
                   !StageManager.Instance.obstaclesMaps.ContainsKey(pos) &&    // 장애물이 없는 위치
                   StageManager.Instance.cellMaps[pos].unitIndexInCell < 0 &&  // 다른 유닛이 없는 위치
                   StageManager.Instance.cellMaps[pos].Cost >= 0;              // 이동 가능한 비용
        }

        /// <summary>
        /// 위치의 종합적인 점수를 계산
        /// </summary>
        private float EvaluatePosition(Vector2 pos, Unit subject, Unit target)
        {
            float coverScore = EvaluateCoverScore(pos);           // 엄폐 점수
            float rangeScore = EvaluateRangeScore(pos, subject, target);  // 사거리 점수
            
            // 가중치를 적용한 종합 점수 계산
            return (coverScore * COVER_SCORE_WEIGHT) +
                   (rangeScore * RANGE_SCORE_WEIGHT);
        }

        /// <summary>
        /// 위치의 엄폐 점수를 계산
        /// </summary>
        private float EvaluateCoverScore(Vector2 pos)
        {
            float score = 0;

            // 인접한 셀의 장애물 확인
            Vector2[] adjacentDirections = {
                new Vector2(1, 0),   // 우
                new Vector2(-1, 0),  // 좌
                new Vector2(0, 1),   // 상
                new Vector2(0, -1)   // 하
            };

            // 각 방향의 엄폐물 확인
            foreach (Vector2 dir in adjacentDirections)
            {
                Vector2 adjacentPos = pos + dir;
                if (StageManager.Instance.obstaclesMaps.ContainsKey(adjacentPos))
                {
                    score += 0.25f; // 각 방향의 엄폐물당 0.25점
                }
            }

            return score;
        }

        /// <summary>
        /// 위치의 사거리 점수를 계산
        /// </summary>
        private float EvaluateRangeScore(Vector2 pos, Unit subject, Unit target)
        {
            float score = 1f;
            float distanceToTarget = Vector2.Distance(pos, target.curCoord);

            // 유닛과 타겟의 스킬 사거리 가져오기
            int subjectRange = (subject.SkillSystem.ActiveSkills[0].Data.SkillRange - 1);

            // 자신의 공격 범위 안이면 점수 추가
            if (distanceToTarget <= subjectRange)
            {
                score += 1f;
                score += 1f * EvaluateDistanceScore(pos, target.curCoord);
            }
            else
            {
                score -= distanceToTarget * 0.1f;
            }

            return score;
        }
        /// <summary>
        /// 목표와의 거리에 따른 점수를 계산
        /// </summary>
        private float EvaluateDistanceScore(Vector2 pos, Vector2 targetPos)
        {
            float distance = Vector2.Distance(pos, targetPos);
            float maxDistance = float.MaxValue;
            return (maxDistance - distance) / maxDistance;
        }
    }
}