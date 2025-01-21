using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UICombatGoalSlotForSettingWindow : UIBase
{
    public TMP_Text challengeText;
    public Image challengeImg;
    public TMP_Text unachievedText;
    private ChallengeGoalSO currentGoal;
    private readonly Color achievedColor = new Color(0.941f, 0.686f, 0.086f);

    public void SetUncompletedGoal(ChallengeGoalSO challengeGoalData)
    {
        if (challengeGoalData == null) return;

        currentGoal = challengeGoalData;
        challengeImg.color = Color.gray;
        unachievedText.enabled = true;
        challengeText.text = challengeGoalData.challengeGoalDescription;
    }

    public void SetCompletedGoal(ChallengeGoalSO challengeGoalData)
    {
        if (challengeGoalData == null) return;

        currentGoal = challengeGoalData;
        challengeImg.color = achievedColor;
        unachievedText.enabled = false;
        challengeText.text = challengeGoalData.challengeGoalDescription;
    }
}
