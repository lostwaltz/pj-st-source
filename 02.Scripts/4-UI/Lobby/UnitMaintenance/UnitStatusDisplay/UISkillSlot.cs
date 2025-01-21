using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : UIAnimation
{
    public Image OutLine;
    public Image SkillIcon;
    public Button Skillbutton;
    private int skillKey;


    [SerializeField] private bool showOutLine;
    [SerializeField] private UIUnitManageSkillInfo skillInfoPanel;

    protected override void Awake()
    {
        base.Awake();
        // var hitArea = gameObject.AddComponent<Image>();
        // hitArea.color = new Color(0, 0, 0, 0);

        // var button = gameObject.AddComponent<Button>();
        Skillbutton.onClick.AddListener(OnSkillClicked);
    }

    public void SetSkillKey(int key)
    {
        skillKey = key;
    }

    private void OnSkillClicked()
    {
        if (skillInfoPanel != null)
        {
            int index = transform.GetSiblingIndex();
            var skillInfo = Core.DataManager.SkillTable.GetByKey(skillKey); // skillKey는 UISkillSlot에 저장
            skillInfoPanel.ShowSkillInfo(skillInfo);
        }
    }
}
