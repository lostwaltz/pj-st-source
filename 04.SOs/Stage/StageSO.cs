using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stage", menuName = "StageSO/New Stage")]
public class StageSO : ScriptableObject
{
    public string stageName;
    public string sceneName;
    [Header("Node")]
    public StageData stageData;
    [Header("Placement")]
    public Placement[] cellPlacements;
    public Placement[] obstaclePlacements;
    public PlacementArea[] obstacleAreas;
    
    [Header("CombatResultCondition")]
    // 스테이지 승패 조건
    public List<GameConditionPredicate> winConditions;
    public List<GameConditionPredicate> lostConditions;

    [Header("ChallengeGoalsList")]
    public List<ChallengeGoalSO> challengeGoals;

    // 스테이지 보상
    [Header("Reward")]
    public List<RewardableFactory> rewardableFactories;

    public void ApplyReward()
    {
        foreach (var factory in rewardableFactories)
        {
            IRewardable rewardable = factory.CreateReward();
            rewardable.ApplyReward();
        }
    }

    [Header("Description")]
    public string stageDescription;
    public int recommendedLevel;

    [Header("적 배치")] 
    public Vector3 enemyDirection;
    public List<EnemyPlacement> enemyPlacements; //실제로는 여길 이용

    [Header("플레이어 배치")]
    public Vector3 playerDirection;
    public List<PlayerPlacement> playerPlacements;

    [Header("기타 옵션")]
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    public bool useGuide;

    [Header("스테이지 다이얼로그 이벤트")]
    public int startDialogueKey;
    public int endDialogueKey;
}

[System.Serializable]
public struct Placement
{
    public int id;
    public int cost;
    public Vector2 coord;
    public Vector3 position;
    
    public Quaternion rotation;
}

[System.Serializable]
public struct PlacementArea
{
    public int obstacleIndex;
    public List<Vector2> areaCoord;
}


public enum EnemyType
{
    Common,
    Elite,
    Boss
}

[System.Serializable]
public struct EnemyPlacement
{
    public int unitKey;
    public Vector2 coord;
    public int level;
    public EnemyType enemyType;

    public EnemyPlacement(int UnitKey, Vector2 coord, int level)
    {
        this.unitKey = UnitKey;
        this.coord = coord;
        this.level = level;
        this.enemyType = EnemyType.Common;
    }

}

[System.Serializable]
public struct PlayerPlacement
{
    public Vector2 coord;
    public bool isPlaced;

    public PlayerPlacement(Vector2 coord)
    {
        this.coord = coord;
        this.isPlaced = false;
    }
}

