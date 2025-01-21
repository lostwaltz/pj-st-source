using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICombatGoalSlot : UIBase
{
    [SerializeField] private TMP_Text challengeText;
    [SerializeField] private Image challengeImg;

    public void SetChallengeSlot(ChallengeGoalSO challengeGoalData)
    {
        if (challengeGoalData.IsConditionMet() != true)
        {
            challengeImg.color = Color.gray;
        }

        challengeText.text = challengeGoalData.challengeGoalDescription;
    }
}
