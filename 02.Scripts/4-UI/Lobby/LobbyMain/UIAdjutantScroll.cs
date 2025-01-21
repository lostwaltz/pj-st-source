using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class UIAdjutantScroll : UIBase
{
    // [SerializeField] private GameObject UILobbyMain;
    [SerializeField] private Transform Content;
    [SerializeField] private ScrollRect ScrollRect;
    [SerializeField] private HorizontalLayoutGroup HorizontalLayoutGroup;
    [SerializeField] private Button BtnBack;

    private readonly List<UIAdjutantScrollSlot> uiAdjutantScrollSlots = new();
    [SerializeField] private UIAdjutantScrollSlot AdjutantScrollSlotPrefab;


    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        HorizontalLayoutGroup.spacing = 20f;
        HorizontalLayoutGroup.padding = new RectOffset(20, 20, 0, 0);

        ScrollRect.horizontal = true;
        ScrollRect.vertical = false;
        ScrollRect.movementType = ScrollRect.MovementType.Elastic;
        ScrollRect.elasticity = 0.1f;
        ScrollRect.inertia = true;
        ScrollRect.decelerationRate = 0.135f;

        LayoutRebuilder.ForceRebuildLayoutImmediate(Content as RectTransform);

        BtnBack.onClick.AddListener(() =>
        {
            UISound.PlayBackButtonClick();
            var uiLobbyMain = Core.UIManager.GetUI<UILobbyMain>();

            uiLobbyMain.CanvasGroupTop.alpha = 1f;
            uiLobbyMain.CanvasGroupRightSide.alpha = 1f;

            DOTween.Sequence()
            .Join(uiLobbyMain.CanvasGroupBottomLeft.GetComponent<CanvasGroup>().DOFade(1f, 1f))
            .Join(uiLobbyMain.CanvasGroupBottomLeft.transform.DOLocalMoveY(0f, 1f))
            .SetEase(Ease.OutQuad);

            Core.UIManager.CloseUI<UIAdjutantScroll>();
        });
    }


    //가챠로 캐릭터얻을때 ? 이벤트구독으로 
    public void AddSlot(UIAdjutantScrollSlot slot)
    {
        slot.transform.SetParent(Content);
        uiAdjutantScrollSlots.Add(slot);
        LayoutRebuilder.ForceRebuildLayoutImmediate(Content as RectTransform);
    }



}

