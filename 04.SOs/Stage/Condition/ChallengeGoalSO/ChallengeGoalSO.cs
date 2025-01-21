using System;
using UnityEngine;
using System.Linq;
using EnumTypes;

public enum ChallengeGoaltype
{
    Victory,                 // 적 전멸 (승리)
    VictoryInTurnLimit,     // N턴 이내 승리
    CheckDeadPlayerUnit,     // 쓰러진 아군이 N명 이하
    CheckEnemyUnit,         // 특정 적 처치
}

//사용시 하위에 조건메서드를 정의 하고, 타입을 생성해 Swith문에 추가해줘야함.
[CreateAssetMenu(fileName = "ChallengeGoal", menuName = "GameConditionSO/New ChallengeGoal")]
public class ChallengeGoalSO : GameConditionPredicate
{
    [Header("도전목표 설명")]
    public string challengeGoalDescription;
    public ChallengeGoaltype challengeGoaltype;

    [Header("n턴 이내 승리 조건")]
    [SerializeField] private int turnLimit = 6;

    [Header("쓰러진 아군이 n명 이하")]
    [SerializeField] private int maxDeadPlayerUnitCount = 1;  // 허용되는 최대 아군 사망자 수

    [Header("특정 적 처치")]
    [SerializeField] private int targetEnemyKey;          // 처치해야 할 특정 적의 UnitInfo Key

    [Header("도전목표 식별자")]
    public int goalKey;  // 각 도전 과제의 고유 식별자

    public event Action<bool> OnGoalStatusChanged;

    private bool isCompleted;
    public bool IsCompleted
    {
        get => isCompleted;
        private set
        {
            if (isCompleted != value)
            {
                isCompleted = value;
                OnGoalStatusChanged?.Invoke(isCompleted);
            }
        }
    }

    public override bool IsConditionMet()
    {
        bool result = CheckCondition();

        // 게임이 끝났을 때만 상태를 업데이트
        if (GameManager.Instance.currentPhase == GamePhase.End)
        {
            IsCompleted = result;
        }

        return result;
    }

    private bool CheckCondition()
    {
        switch (challengeGoaltype)
        {
            case ChallengeGoaltype.Victory:
                return CheckVictory(); // 이번 작전에서 승리

            case ChallengeGoaltype.VictoryInTurnLimit:
                return CheckVictoryInTurnLimit(); // N턴 이내 승리

            case ChallengeGoaltype.CheckDeadPlayerUnit:
                return CheckDeadPlayerUnit(); // 쓰러진 아군이 N명 이하

            case ChallengeGoaltype.CheckEnemyUnit:
                return CheckEnemyUnit(); // 처치해야 할 특정 적

            default:
                Debug.Log(" 작전 실패 ");
                return false;
        }
    }

    public override string Description()
    {
        return challengeGoalDescription;
    }

    private bool CheckVictory()
    {
        if (GameManager.Instance.currentPhase != GamePhase.End)
        {
            return false;
        }

        return GameUnitManager.Instance.Units[GameUnitManager.Enemy].Count <= 0;
    }

    private bool CheckVictoryInTurnLimit()
    {
        if (GameManager.Instance.currentPhase != GamePhase.End)
        {
            return false;
        }

        bool withinTurnLimit = GameManager.Instance.PhaseMachine.TurnCount <= turnLimit;
        bool victory = GameUnitManager.Instance.Units[GameUnitManager.Enemy].Count <= 0;

        return withinTurnLimit && victory;
    }

    private bool CheckDeadPlayerUnit()
    {
        if (GameManager.Instance.currentPhase != GamePhase.End)
        {
            return false;
        }

        int maxUnitCount = GameUnitManager.Instance.UnitDic[GameUnitManager.Playable].Count;
        int curUnitCount = GameUnitManager.Instance.Units[GameUnitManager.Playable].Count;
        int deadCount = maxUnitCount - curUnitCount;


        // 사망자가 있으면 실패
        return deadCount == 0;
    }

    private bool CheckEnemyUnit()
    {
        if (GameManager.Instance.currentPhase != GamePhase.End)
        {
            return false;
        }

        var enemies = GameUnitManager.Instance.Units[GameUnitManager.Enemy];
        bool targetEnemyExists = enemies.Any(enemy => enemy.data.UnitBase.Key == targetEnemyKey);


        return !targetEnemyExists;
    }
}
