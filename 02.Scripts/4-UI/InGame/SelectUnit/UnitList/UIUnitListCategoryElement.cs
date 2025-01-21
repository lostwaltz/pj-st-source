using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using EnumTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUnitListCategoryElement : UIBase
{
    [SerializeField] private TMP_Text CategoryListText;
    [SerializeField] private TMP_Text CategoryText;
    [SerializeField] private Image imgBackground;
    [SerializeField] private UIUnitList uiUnitList;
    [SerializeField] private UIUnitListCategory uiCategoryList;

    private SortType sortType;

    private void Awake()
    {
        // BindEvent(gameObject, _ => imgBackground.DOFade(0.3f, 0.2f), UIEvent.Enter);
        // BindEvent(gameObject, _ => imgBackground.DOFade(0f, 0.2f), UIEvent.Exit);

        BindEvent(gameObject, _ =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            UpdateUnitList();
            CategoryListText.text = CategoryText.text;
        });
    }

    private void OnEnable()
    {
        UISound.PlayUnitListOpen();
    }

    public void UpdateText(SortType sortType)
    {
        this.sortType = sortType;
        CategoryText.text = Core.UnitManager.MappingDictionary[sortType];
    }

    public void UpdateUnitList()
    {
        uiUnitList.SetSortType(sortType);
    }
}
