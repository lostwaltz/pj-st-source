using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Coverable,
    SpecialTopography
}

[CreateAssetMenu(fileName = "Obstacle", menuName = "StageSO/New Obstacle")]
public class ObstacleSO : ScriptableObject
{
    [Header("Info")]
    public string obstacleName;
    public string description;
    public ObstacleType obstacleType;
    public int CoverBonus = 20; // 은폐 시 제공하는 방어력 : 20 ~ 35 중 
    
    public GameObject obstaclePrefab;

    public Dictionary<ObstacleType, string> obstacleDic = new()
    {
        { ObstacleType.Coverable, "엄폐물" },
        { ObstacleType.SpecialTopography, "특수지형" }
        
    };
    
    //외부에서 활용 할 메서드
    public string GetObstacleType(ObstacleType type)
    {
        return obstacleDic.TryGetValue(type, out string name) ? name : "알 수 없는 타입";
    }
}
