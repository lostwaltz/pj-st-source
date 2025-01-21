using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIGachaScroll : UIBase
{
    [SerializeField] private Transform content;
    [SerializeField] private ScrollRect ScrollRect;
    [SerializeField] private VerticalLayoutGroup VerticalLayoutGroup;

    [SerializeField] private List<UIGachaScrollSlot> uiGachaScrollSlots = new();
    [SerializeField] private UIGachaScrollSlot gachaScrollSlotPrefab;
    
    private void Start()
    {
        for (int i = 0; i < uiGachaScrollSlots.Count; i++)
        {
            uiGachaScrollSlots[i].index = i + 1;
        }
    }

    //TODO : 동적생성할때 사용할 스크립트
}