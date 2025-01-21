using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UGS;

public class UICombatGoalSlotForStageInfoWindow : UIBase
{
    public TMP_Text challengeText;
    public Image challengeImg;
    public Color achievedImageColor = new Color(0.941f, 0.686f, 0.086f);
    public Color achievedTextColor = new Color(50 / 255f, 68 / 255f, 70 / 255f);

    private ChallengeGoalSO currentGoal;

    private void Start()
    {
        Core.UGSManager.Data.OnAllDataLoaded += UpdateUI;
    }

    private void OnDestroy()
    {
        if (Core.UGSManager != null && Core.UGSManager.Data != null)
        {
            Core.UGSManager.Data.OnAllDataLoaded -= UpdateUI;
        }
    }

    private void UpdateUI(Dictionary<Type, ICloudDataContainer> cloudDatas)
    {
        if (currentGoal != null)
        {
            RefreshUIState();
        }
    }

    public void SetChallengeSlotInfo(ChallengeGoalSO challengeGoalData)
    {
        if (challengeGoalData == null) return;

        currentGoal = challengeGoalData;
        challengeText.text = challengeGoalData.challengeGoalDescription;
        RefreshUIState();
    }

    private void RefreshUIState()
    {

        var container = Core.UGSManager.Data.CloudDatas[typeof(ChallengeObjective)] as CloudDataContainer<ChallengeObjective>;
        if (container == null) return;

        var stageKey = Core.DataManager.SelectedStage.stageData.StageKey;
        var dataList = container.GetData() as List<ChallengeObjective>;



        if (dataList != null)
        {
            bool isCompleted = dataList.Any(data =>
                data.StageKey == stageKey && data.goalKey == currentGoal.goalKey && data.isCompleted);


            // 달성했을 때만 골드 색상으로 변경
            if (isCompleted)
            {
                challengeImg.color = achievedImageColor;
                challengeText.color = achievedTextColor;
            }
            else
            {
                challengeImg.color = Color.gray;
                challengeText.color = Color.gray;
            }
        }
    }
}
