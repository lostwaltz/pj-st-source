using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class UIPopUpChallengeObjective : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup VerticalLayoutGroup;
    [SerializeField] private UIObjective ObjectiveSuccess;
    [SerializeField] private UIObjective ObjectiveFail;

    private void OnEnable()
    {
        ChallengeObjectiveManager.Instance.OnChallengeGoalsUpdated += CreateSuccessObjectives;
    }

    private void OnDisable()
    {
        if (ChallengeObjectiveManager.Instance != null)
        {
            ChallengeObjectiveManager.Instance.OnChallengeGoalsUpdated -= CreateSuccessObjectives;
        }
    }

    private void CreateSuccessObjectives(List<ChallengeGoalSO> completedGoals)
    {
        // 현재 스테이지의 모든 도전 과제를 가져옴
        var allGoals = Core.DataManager.SelectedStage.challengeGoals;
        StartCoroutine(CreateObjectivesSequentially(allGoals, completedGoals));
    }

    private IEnumerator CreateObjectivesSequentially(List<ChallengeGoalSO> allGoals, List<ChallengeGoalSO> completedGoals)
    {
        float delayBetweenItems = 0.15f;
        var container = Core.UGSManager.Data.CloudDatas[typeof(UGS.ChallengeObjective)] as UGS.CloudDataContainer<UGS.ChallengeObjective>;
        if (container == null) yield break;

        var dataList = container.GetData() as List<UGS.ChallengeObjective>;
        if (dataList == null) yield break;

        int currentStageKey = Core.DataManager.SelectedStage.stageData.StageKey;


        // 이전에 완료된 목표들의 goalKey를 저장
        var previouslyCompletedGoals = new HashSet<int>();
        foreach (var data in dataList)
        {
            if (data.isCompleted)
            {
                previouslyCompletedGoals.Add(data.goalKey);
            }
        }

        // 성공한 목표 먼저 생성 (이번에 새로 완료된 목표만)
        var successGoals = completedGoals.Where(g => !previouslyCompletedGoals.Contains(g.goalKey)).ToList();
        foreach (var goal in successGoals)
        {
            UIObjective objectiveUI = Instantiate(ObjectiveSuccess, VerticalLayoutGroup.transform);
            objectiveUI.SetChallengeGoal(goal);
            yield return new WaitForSeconds(delayBetweenItems);
        }

        // 실패한 목표 생성 (이전에 완료되지 않은 목표 중 이번에도 완료되지 않은 것)
        var failGoals = allGoals.Where(g => !completedGoals.Contains(g) && !previouslyCompletedGoals.Contains(g.goalKey)).ToList();
        foreach (var goal in failGoals)
        {
            UIObjective objectiveUI = Instantiate(ObjectiveFail, VerticalLayoutGroup.transform);
            objectiveUI.SetChallengeGoal(goal);
            yield return new WaitForSeconds(delayBetweenItems);
        }
    }

    // 기존 UI 프리팹들을 모두 제거
    public void ClearObjectives()
    {
        foreach (Transform child in VerticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // UI 업데이트
    public void UpdateObjectives()
    {
        ClearObjectives();
        CreateSuccessObjectives(ChallengeObjectiveManager.Instance.GetCompletedGoals());
    }
}
