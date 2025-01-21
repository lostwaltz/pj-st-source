using System;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using JetBrains.Annotations;
using UnityEngine;

public class UnitCoverSystem : MonoBehaviour
{
    Unit unit;
    UnitAnimation animation;

    // 0.7 (정면 대각선) ~ 1 (정면)
    float coveringThreshold = 0.2f; // 측면에 가까운 대각선에서 엄폐물을 피해서 공격이 들어오는 정도를 위함
    
    [SerializeField] List<StageObstacle> nearObstacles = new List<StageObstacle>();
    [SerializeField] int[] covered = new int[4];
    // 엄폐 방향 파악 : 4 방향
    Vector2Int[] fourDir2D = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
    Vector3Int[] fourDir3D = { Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back };

    public void Initialize(Unit owner)
    {
        unit = owner;
        animation = unit.Animation;
    }

    public void Initialize()
    {
        animation = GetComponent<UnitAnimation>();
    }
    
    
    [CanBeNull]
    public Transform InstantCheckCover(Vector2 coord, out int type)
    {
        if (StageManager.Instance.obstaclesMaps.TryGetValue(coord, out StageObstacle obstacle))
        {
            // 계산 단순화를 위해서
            // 현재 서있는 타일에 장애물이 있는지 먼저 체크
            // 있다면 바로 엄폐
            Transform coverPoint = obstacle.coverPoints.OrderBy(n => (n.position - transform.position).sqrMagnitude).First();
            // Cover(coverPoint, (int)obstacle.type);
            type = (int)obstacle.type;
            return coverPoint;
        }
        
        type = (int)ObstacleCoverType.HighCover;
        return null;
    }
    
    [CanBeNull]
    public Transform CheckCoverPoint(Vector2 coord, UnitType unitType, out int type)
    {
        type = (int)ObstacleCoverType.HighCover;
        GetNearObstacles(coord, out List<StageObstacle> obs);
        
        if (obs.Count > 0)
        {
            // 우선순위 적용
            UnitType opposite = unitType == GameUnitManager.Playable ? GameUnitManager.Enemy : GameUnitManager.Playable;
            Vector3 enemyPosition = GameUnitManager.Instance.GetNearestEnemyPosition(opposite, transform.position);
            Vector3 dirForEnemy = (enemyPosition - transform.position).normalized;

            float currentDot = -1f;
            Transform coverPoint = obs[0].coverPoints[0];
            StageObstacle selectedObstacle = obs[0];

            foreach (var ob in obs)
            {
                Transform point = ob.coverPoints.OrderBy(n => (n.position - transform.position).sqrMagnitude).First();
                Vector3 dirForPoint = (point.position - transform.position).normalized;

                // 1에 가까울수록 방향이 일치하는 것
                float dot = Vector3.Dot(dirForEnemy, dirForPoint);
                if (currentDot <= dot)
                {
                    currentDot = dot;
                    coverPoint = point;
                    selectedObstacle = ob;
                }
            }

            type = (int)selectedObstacle.type;
            return coverPoint;
        }
        
        return null;
    }

    public void Cover(Transform point, int type)
    {
        transform.rotation = Quaternion.LookRotation((point.position - transform.position).normalized);
        transform.position = Vector3.Lerp(transform.position, point.position, 0.3f);
        
        // 엄폐 정보
        // 엄폐 방향 : 4방향
        // 엄폐 보호량 : int
        
        animation.SetCover(type);
    }
    
    public void GetNearObstacles(Vector2 coord, out List<StageObstacle> obstacles)
    {
        nearObstacles.Clear();
        obstacles = new List<StageObstacle>();
        
        StageManager.Interaction.GetNearCoord(coord, out List<Vector2> nearCoords);
        for (int i = 0; i < nearCoords.Count; i++)
        {
            if (StageManager.Instance.obstaclesMaps.TryGetValue(nearCoords[i], out StageObstacle nearObstacle))
            {
                if (nearCoords[i].Equals(coord) ||  // 지금 칸이 장애물
                    nearObstacle.placement.cost < 0) // 1칸, 셀 장애물
                {
                    obstacles.Add(nearObstacle);
                    continue;
                }
                
                // 벽 장애물
                Vector3 dir = (transform.position - nearObstacle.transform.position).normalized;
                float dot = Vector3.Dot(dir, nearObstacle.transform.forward);
                
                if (dot > 0.9f) // 내가 보는 방향과 일치하는 곳
                    obstacles.Add(nearObstacle);
            }
        }
        
        nearObstacles = obstacles;
        
        if(unit != null)
            CalcObstacleDirection();
    }

    public void CalcObstacleDirection()
    {
        for (int i = 0; i < covered.Length; i++)
            covered[i] = 0;

        for (int i = 0; i < nearObstacles.Count; i++)
        {
            // Vector2 obsDir = nearObstacles[i].placement.coord - unit.curCoord;
            Vector2Int obsDir = FloatToInt(nearObstacles[i].placement.coord - unit.curCoord);
            
            if (obsDir.Equals(Vector2Int.zero))
            {
                for (int j = 0; j < fourDir3D.Length; j++)
                {
                    if (FloatToInt(nearObstacles[i].transform.forward).Equals(fourDir3D[j]))//(IsEqualVector(fourDir3D[j], nearObstacles[i].transform.forward))
                    {
                        if (covered[j] < nearObstacles[i].obstacleData.CoverBonus)
                            covered[j] = nearObstacles[i].obstacleData.CoverBonus;
                        break;
                    }
                }
                continue;
            }
            
            for (int j = 0; j < fourDir2D.Length; j++)
            {
                if (obsDir.Equals(fourDir2D[j]))//(IsEqualVector(fourDir2D[j], obsDir))
                {
                    if(covered[j] < nearObstacles[i].obstacleData.CoverBonus)
                        covered[j] = nearObstacles[i].obstacleData.CoverBonus;
                    break;
                }
            }
        }
    }

    public int GetCoverBonus(Vector2 unitPoint, Vector2 attackPoint)
    {
        Vector2 againstDir = (attackPoint - unitPoint).normalized;
        
        // 사선으로 공격을 받을 시, 어떤 면으로 엄폐를 받는지 애매해짐
        // 이때 기준을 더 유리한 보너스를 제공하는 것으로 작업
        int bonus = 0;
        for (int i = 0; i < fourDir2D.Length; i++)
        {
            float dot = Vector2.Dot(againstDir, fourDir2D[i]);
            if (dot < coveringThreshold) 
                continue;
            
            if (dot > coveringThreshold && covered[i] > 0 && bonus < covered[i])
                bonus = covered[i];
        }
        
        return bonus;
    }

    Vector2Int FloatToInt(Vector2 floatVec)
    {
        return new Vector2Int((int)floatVec.x, (int)floatVec.y);
    }

    Vector3Int FloatToInt(Vector3 floatVec)
    {
        return new Vector3Int((int)floatVec.x, (int)floatVec.y, (int)floatVec.z);
    }
}