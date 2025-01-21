using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAchElement : UIBase
{
    [SerializeField] private Image imgProgressBar;

    [SerializeField] private TMP_Text textTitle;
    [SerializeField] private TMP_Text textDescription;
    [SerializeField] private TMP_Text textDone;
    [SerializeField] private TMP_Text textRewardAmount;

    [SerializeField] private TMP_Text textCurProgress;
    [SerializeField] private TMP_Text textMaxProgress;
    [SerializeField] private Image imgIcon;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Button btnGetReward;
    
    [SerializeField] private GameObject layout;

    private FadeTo fadeTo;
    public AchievementInstance AchievementInstance;

    private void Start()
    {
        btnGetReward.onClick.AddListener(TakeReward);
    }

    public void SetAchievementData(AchievementInstance achievement)
    {
        AchievementInstance = achievement;

        textTitle.text = achievement.OriginData.Title;
        textDescription.text = achievement.OriginData.Description;
        textDone.text = achievement.IsComplete
            ? "<color=#FFFFFF>완료</color>" // 하얀색
            : "<color=#808080>미완료</color>";
        textRewardAmount.text = achievement.OriginData.RewardAmount.ToString();
        btnGetReward.gameObject.SetActive(!achievement.IsTakeReward && achievement.IsComplete);

        textCurProgress.text = ((int)achievement.CurProgress).ToString();
        textMaxProgress.text = ((int)achievement.MaxProgress).ToString();

        imgProgressBar.fillAmount = achievement.CurProgress / achievement.MaxProgress;

        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);
        
        canvasGroup.alpha = 0f;
    }

    public void TakeReward()
    {
        UISound.PlayStageBattleStart();

        AchievementInstance.IsTakeReward = true;
        var path = Utils.Str.Clear().Append(Path.Reward).Append(AchievementInstance.OriginData.RewardPath).ToString();

        var factory = Resources.Load<AchievementRewardFactory>(path);
        factory.rewardableFactory.Amount = AchievementInstance.OriginData.RewardAmount;
        factory.rewardableFactory.CreateReward().ApplyReward();
        btnGetReward.gameObject.SetActive(false);
        
        Core.UIManager.GetUI<UIReward>().TakeReward(100);
    }

    public int TakeRewardAll()
    {
        AchievementInstance.IsTakeReward = true;
        var path = Utils.Str.Clear().Append(Path.Reward).Append(AchievementInstance.OriginData.RewardPath).ToString();

        var factory = Resources.Load<AchievementRewardFactory>(path);
        factory.rewardableFactory.Amount = AchievementInstance.OriginData.RewardAmount;
        factory.rewardableFactory.CreateReward().ApplyReward();
        btnGetReward.gameObject.SetActive(false);
        
        return AchievementInstance.OriginData.RewardAmount;
    }
    
    

    public void Fade()
    {
        fadeTo ??= new FadeTo(0f, 1f, Ease.Linear, false, false, false);

        fadeTo.BehaviorExecute(transform, 0.2f);
    }
}
