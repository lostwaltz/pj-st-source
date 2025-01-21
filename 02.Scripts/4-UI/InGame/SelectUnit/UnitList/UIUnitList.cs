using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIUnitList : UIBase
{
    [Header("elementPrefab")]
    [SerializeField] private UIUnitListElement elementPrefab;
    
    [Header("SortToggleButton")]
    [SerializeField] private Image ascendButton;
    
    [Header("SortCategory")]
    [SerializeField] private Image sortCategory;
    [SerializeField] private UIUnitListCategory uiCategory;
    
    [Header("FilterButton")]
    [SerializeField] private Image filterButton;
    
    [Header("ContentTransform")]
    [SerializeField] private Transform contentTransform;
    
    private readonly List<UIUnitListElement> elementList = new ();
    
    public SortType SortType { get; private set; }
    public FilterType FilterType { get; private set; }
    public bool Ascending { get; private set; }

    private void Awake()
    {
        SortType = SortType.None;
        FilterType = FilterType.None;
        
        BindEvent(ascendButton.gameObject, _ => SetAscending(!Ascending));
        BindEvent(sortCategory.gameObject, _ => { uiCategory.Open(); });

        UpdateUI();
    }

    public void SetAscending(bool ascending)
    {
        Ascending = ascending;
        UpdateUI();
    }

    public void SetSortType(SortType sortType)
    {
        SortType = sortType;
        UpdateUI();
    }

    public void SetFilterType(FilterType filterType)
    {
        FilterType = filterType;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var index = 0;

        var query = Core.UnitManager.Filter(FilterType).Sort(SortType, Ascending).Build();

        foreach (var element in elementList)
            element.gameObject.SetActive(false);
            
        foreach (var instance in query)
        {
            if(elementList.Count <= index)
                elementList.Add(Instantiate(elementPrefab, contentTransform));
            
            elementList[index].gameObject.SetActive(true);
            elementList[index]?.UpdateElement(instance);
            
            index++;
        }
    }

    public List<UIUnitListElement> GetUnitElementList()
    {
        return new List<UIUnitListElement>(elementList);
    }
}
