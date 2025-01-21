using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICategoryElement : UIAnimation
{
    [SerializeField] private GameObject layoutGroup;

    [SerializeField] private TMP_Text textCategoryTitle;

    [SerializeField] private TMP_Text textCurProgress;
    [SerializeField] private TMP_Text textMaxProgress;

    public readonly List<AchievementInstance> AchievementList = new();

    public Action<List<AchievementInstance>, UICategoryElement> OnSelect;

    public int curProgress = 0;
    public int maxProgress = 0;

    protected override void Awake()
    {
        base.Awake();

        BindEvent(gameObject, _ => OnSelectUI());
    }

    public void OnSelectUI()
    {
        UIBattle.PlayUIBattleClickNormalSound();
        OnSelect?.Invoke(AchievementList, this);
    }

    protected override void Start()
    {
        base.Start();

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.transform as RectTransform);
    }

    public void SetCategoryName(string achDataKey)
    {
        textCategoryTitle.text = achDataKey;
    }

    public void UpdateUI()
    {
        var count = 0;
        foreach (var instance in AchievementList)
            count = instance.IsComplete ? count + 1 : count;

        curProgress = count;
        maxProgress = AchievementList.Count;

        textCurProgress.text = count.ToString();
        textMaxProgress.text = AchievementList.Count.ToString();
    }

    public void AddAchData(AchievementInstance achData)
    {
        AchievementList.Add(achData);
    }
}
