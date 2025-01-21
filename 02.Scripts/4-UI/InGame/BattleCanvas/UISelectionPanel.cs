using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectionPanel : UIBase
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private CanvasGroup confirmButtonCanvasGroup;
    [SerializeField] private UIActionCancelPanel uiActionCancelPanel;
    
    private void OnEnable()
    {
        UISound.PlayStageNodeClick();
    }

}

