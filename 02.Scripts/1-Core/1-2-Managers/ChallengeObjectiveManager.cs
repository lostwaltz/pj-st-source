using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EnumTypes;
using UGS;


// 도전 과제 확인
//ChallengeObjectiveManager.Instance.CheckChallengeGoals();

// 완료된 도전 과제 가져오기
//var completedGoals = ChallengeObjectiveManager.Instance.GetCompletedGoals();

// UI 업데이트를 위한 이벤트 구독
//ChallengeObjectiveManager.Instance.OnChallengeGoalsUpdated += UpdateUI;


/// <summary>
/// 게임의 도전 과제를 관리하는 매니저 클래스입니다.
/// </summary>
public class ChallengeObjectiveManager : Singleton<ChallengeObjectiveManager>
{
    [SerializeField] private UIPopUpChallengeObjective uiPopUpChallengeObjectivePrefab;
    private UIPopUpChallengeObjective uiPopUpChallengeObjective;
    private List<ChallengeGoalSO> challengeGoals;
    private List<ChallengeGoalSO> completedChallengeGoals = new();

    public event Action<List<ChallengeGoalSO>> OnChallengeGoalsUpdated;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        ResetManager();

        // 이벤트 구독
        ////Core.EventManager.Subscribe<UnitOnDeathEvent>(OnUnitDied);
        Core.EventManager.Subscribe<GameStartEvent>(OnGameStart);

        //턴 종료 시 체크를 위한 이벤트 구독
        var gameManager = GameManager.Instance;
        gameManager.SubscribePhaseEvent(GamePhase.End, OnTurnEnd);
    }

    /// <summary>
    /// 매니저의 상태를 초기화합니다.
    /// </summary>
    public void ResetManager()
    {
        // UI 프리팹 초기화
        if (uiPopUpChallengeObjectivePrefab != null && uiPopUpChallengeObjective == null)
        {
            uiPopUpChallengeObjective = Instantiate(uiPopUpChallengeObjectivePrefab);
        }

        // 도전 과제 목록 초기화
        challengeGoals = Core.DataManager.SelectedStage.challengeGoals;

        // UI 상태 초기화
        if (uiPopUpChallengeObjective != null)
        {
            uiPopUpChallengeObjective.ClearObjectives();
        }
    }



    private void OnGameStart(GameStartEvent _)
    {
        CheckChallengeGoals();
    }

    private void OnTurnEnd()
    {
        CheckChallengeGoals();
    }





    /// <summary>
    /// 모든 도전 과제의 달성 여부를 확인합니다.
    /// </summary>
    public void CheckChallengeGoals()
    {
        bool goalsUpdated = false;
        List<ChallengeGoalSO> newlyCompletedGoals = new List<ChallengeGoalSO>();

        foreach (var goal in challengeGoals)
        {
            if (!completedChallengeGoals.Contains(goal) && goal.IsConditionMet())
            {
                completedChallengeGoals.Add(goal);
                newlyCompletedGoals.Add(goal);
                goalsUpdated = true;
            }
        }

        // UI 업데이트를 먼저 수행
        if (goalsUpdated)
        {
            OnChallengeGoalsUpdated?.Invoke(completedChallengeGoals);
        }

        // UI 생성 후 UGS에 저장
        foreach (var goal in newlyCompletedGoals)
        {
            SaveChallengeGoalToUGS(goal);
        }
    }

    private void SaveChallengeGoalToUGS(ChallengeGoalSO goal)
    {
        var stageKey = Core.DataManager.SelectedStage.stageData.StageKey;

        // CloudData의 ChallengeObjective 컨테이너에서 기존 데이터 가져오기
        var container = Core.UGSManager.Data.CloudDatas[typeof(ChallengeObjective)] as CloudDataContainer<ChallengeObjective>;
        if (container == null) return;
        var dataList = container.GetData() as List<ChallengeObjective> ?? new List<ChallengeObjective>();

        // 이미 완료된 목표는 다시 저장하지 않음
        var existingData = dataList.FirstOrDefault(data =>
            data.StageKey == stageKey &&
            data.goalKey == goal.goalKey);

        if (existingData != null)
        {
            // 이미 완료된 목표는 업데이트하지 않음
            if (existingData.isCompleted) return;

            // 목표가 달성된 경우에만 완료 상태로 업데이트
            if (goal.IsConditionMet())
            {
                existingData.isCompleted = true;
            }
        }
        else if (goal.IsConditionMet()) // 새로운 목표는 달성된 경우에만 추가
        {
            // 새로운 목표 추가
            var goalData = new ChallengeObjective(stageKey, goal.goalKey);
            goalData.Complete();
            dataList.Add(goalData);
        }

        // 저장 요청
        Core.UGSManager.Data.CallSave(CloudDataType.ChallengeObjective);
    }

    /// <summary>
    /// 완료된 도전 과제 목록을 반환합니다.
    /// </summary>
    public List<ChallengeGoalSO> GetCompletedGoals()
    {
        return completedChallengeGoals;
    }

    /// <summary>
    /// 특정 도전 과제의 완료 여부를 반환합니다.
    /// </summary>
    public bool IsGoalCompleted(ChallengeGoalSO goal)
    {
        return completedChallengeGoals.Contains(goal);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            ////     GameManager.Instance.UnsubscribePhaseEvent(GamePhase.PlayerTurn, OnTurnEnd);
            ////     GameManager.Instance.UnsubscribePhaseEvent(GamePhase.EnemyTurn, OnTurnEnd);
            GameManager.Instance.UnsubscribePhaseEvent(GamePhase.End, OnTurnEnd);
        }

        //// Core.EventManager.Unsubscribe<UnitOnDeathEvent>(OnUnitDied);
        Core.EventManager.Unsubscribe<GameStartEvent>(OnGameStart);

        // UI 프리팹 정리
        if (uiPopUpChallengeObjective != null)
        {
            Destroy(uiPopUpChallengeObjective.gameObject);
            uiPopUpChallengeObjective = null;
        }
    }
}