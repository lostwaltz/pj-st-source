using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAchievement : UIPopup
{
    [SerializeField] private GameObject layoutGroup;
    [SerializeField] private UICategoryElement categoryElement;
    [SerializeField] private UIAchElement achElement;

    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject cateContent;
    [SerializeField] private GameObject achContent;

    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text curProgress;
    [SerializeField] private TMP_Text maxProgress;

    private readonly List<UICategoryElement> achCategoryList = new();
    private readonly List<UIAchElement> achInstList = new();

    public void TakeAllReward()
    {
        int amount = 0;
        foreach (var element in achInstList)
        {
            if (false == element.AchievementInstance.IsTakeReward && element.AchievementInstance.IsComplete)
                amount += element.TakeRewardAll();
        }
        
        if(amount <= 0)
            return;
        
        Core.UIManager.GetUI<UIReward>().TakeReward(amount);
        UISound.PlayStageBattleStart();
    }
    
    private void Awake()
    {
        Duration = 0.25f;

        Dictionary<string, List<AchievementInstance>> achDataDic = AchievementManager.Instance.GetProcessedAchievements();

        foreach (var achData in achDataDic)
        {
            var newCategoryElement = Instantiate(categoryElement, cateContent.transform);
            newCategoryElement.gameObject.SetActive(true);
            newCategoryElement.SetCategoryName(achData.Value.First().OriginData.CategoryTitleKor);

            achCategoryList.Add(newCategoryElement);

            foreach (var achievement in achData.Value)
                achCategoryList.Last().AddAchData(achievement);

            achCategoryList.Last().UpdateUI();
            achCategoryList.Last().OnSelect += UpdateUI;
        }
    }

    public void ResetCategory()
    {
        foreach (var element in achCategoryList)
        {
            element.AchievementList.Clear();
        }
        
        Dictionary<string, List<AchievementInstance>> achDataDic = AchievementManager.Instance.GetProcessedAchievements();

        int index = 0;
        foreach (var achData in achDataDic)
        {
            achCategoryList[index].SetCategoryName(achData.Value.First().OriginData.CategoryTitleKor);
            
            foreach (var achievement in achData.Value)
                achCategoryList[index].AddAchData(achievement);

            achCategoryList[index].UpdateUI();

            index++;
        }
    }

    public void UpdateUI(List<AchievementInstance> achList, UICategoryElement category)
    {
        var index = 0;

        foreach (var element in achCategoryList)
        {
            element.UpdateUI();
        }

        foreach (var instance in achList)
        {
            if (index >= achInstList.Count)
                achInstList.Add(Instantiate(achElement, achContent.transform));

            achInstList[index].gameObject.SetActive(true);
            achInstList[index].SetAchievementData(instance);
            index++;
        }

        for (var i = index; i < achInstList.Count; i++)
            achInstList[i].gameObject.SetActive(false);

        var count = 0;
        var maxCount = 0;
        foreach (var element in achCategoryList)
        {
            count += element.curProgress;
            maxCount += element.maxProgress;
        }

        curProgress.text = count.ToString();
        maxProgress.text = maxCount.ToString();

        if (maxCount > 0)
            progressBar.fillAmount = (float)count / maxCount;

        scrollRect.verticalNormalizedPosition = 1;
        StartCoroutine(PlayAnimationsWithDelay(achInstList.GetRange(0, index), 0.05f)); // 필요한 슬롯만 전달
    }

    private IEnumerator PlayAnimationsWithDelay(List<UIAchElement> achList, float delay)
    {
        foreach (var ach in achList)
        {
            ach.Fade();
            yield return new WaitForSeconds(delay);
        }
    }

    public override void Open()
    {
        base.Open();

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.transform as RectTransform);
    }

    protected override void OpenProcedure()
    {
        CheckDailyCycle();
        
        achCategoryList.First().OnSelectUI();
    }

    protected override void CloseProcedure()
    {
        UISound.PlayBackButtonClick();
        base.CloseProcedure();

        foreach (var element in achInstList)
        {
            element.gameObject.SetActive(false);
        }
    }
    
    
    void CheckDailyCycle()
    {
        string now = DateTime.UtcNow.ToShortDateString();
        string lastOpenTimeStr = PlayerPrefs.GetString("CheckReset");

        if (!now.Equals(lastOpenTimeStr))
        {
            AchievementManager.Instance.ResetAchievements();
            ResetCategory();
        }

        // 현재 시간을 저장
        PlayerPrefs.SetString("CheckReset", now);
        PlayerPrefs.Save();
    }
}
