using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UIUnitListCategory : UIAnimation
{
    [SerializeField] private UIUnitListCategoryElement elementPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject blocker;
    
    private readonly List<UIUnitListCategoryElement> elementList = new ();

    protected override void Awake()
    {
        base.Awake();
        
        BindEvent(blocker.gameObject, _ => { Close(); });
    }

    private void OnEnable()
    {
        for (var i = 0; i < typeof(SortType).GetEnumValues().Length; i++)
        {
            if(elementList.Count <= i)
                elementList.Add(Instantiate(elementPrefab, content));
            
            elementList[i].gameObject.SetActive(true);
            elementList[i].UpdateText((SortType)i);
        }
    }
}
