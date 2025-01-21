using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAdjutantScrollSlot : UIAnimation, ISlottable
{
    [SerializeField] private Image Adjutant3DIcon;
    [SerializeField] private Image Background;
    [SerializeField] private Image SelectedIcon;
    [SerializeField] private Image LockedIcon;
    [SerializeField] private TextMeshProUGUI BGName;


    public void Initialize()
    {
        throw new System.NotImplementedException();
    }

    public void SetHighlight(bool highlight)
    {
        throw new System.NotImplementedException();
    }

    private void Onclick()
    {
        //클릭시 배경 변경 
        throw new System.NotImplementedException();
    }

    public void UnlockSlot(UIAdjutantScrollSlot slot)
    {
        throw new NotImplementedException();
    }


}
