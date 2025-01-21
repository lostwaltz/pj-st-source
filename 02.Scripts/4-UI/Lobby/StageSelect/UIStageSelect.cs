using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UIStageSelect : UIBase
{
    public RectTransform backgroundclose;
    [SerializeField] private RectTransform NodeContainer;
    [SerializeField] private RectTransform LineContainer;

    [SerializeField] private UIStageNode StageNodePrefab;
    [SerializeField] private GameObject LinePrefab;

    [SerializeField] private GameObject DepthSlot;
    [SerializeField] private GameObject Content;

    [SerializeField] private Button ExitBtn;

    private void Start()
    {
        BindEvent(ExitBtn.gameObject, OnBtnDown, UIEvent.Down);
        BindEvent(ExitBtn.gameObject, OnBtnUp, UIEvent.Up);
        ExitBtn.onClick.AddListener(OnSubmit);
    }
    private void CreateNodeAndLine(List<StageSO> data)
    {
        var builder = new StageTreeBuilder();
        var creator = new StageLayoutCreator(builder.BuildTree(data), NodeContainer, LineContainer, StageNodePrefab, LinePrefab, DepthSlot, Content);
        creator.CreateLayout();
    }

    private void OnEnable()
    {
        UISound.PlayStageSelectUIStart();
        // var data = new List<StageData>
        // {
        //     new StageData() { StageKey = 0, DependencyKey = 0 },
        //     new StageData() { StageKey = 1, DependencyKey = 0 },
        //     new StageData() { StageKey = 2, DependencyKey = 1},
        //     new StageData() { StageKey = 3, DependencyKey = 2},
        //     new StageData() { StageKey = 4, DependencyKey = 3},
        //     
        //     new StageData() { StageKey = 5, DependencyKey = 0 },
        //     new StageData() { StageKey = 6, DependencyKey = 0 },
        //     new StageData() { StageKey = 7, DependencyKey = 0 },
        //     new StageData() { StageKey = 8, DependencyKey = 0 },
        //     
        //     new StageData() { StageKey = 9, DependencyKey = 8 },
        //     new StageData() { StageKey = 10, DependencyKey = 9 },
        //     new StageData() { StageKey = 11, DependencyKey = 4 },
        //     new StageData() { StageKey = 12, DependencyKey = 11 },
        //     
        // };

        CreateNodeAndLine(Core.DataManager.StageDataList);
    }

    private void OnSubmit()
    {
        Core.UIManager.CloseUI<UIStageInfo>();
        Core.UIManager.GetUI<UIScreenFade>().FadeTo(0f, 1f, 0.2f).
            OnComplete(OnExit);
    }

    private void OnExit()
    {
        UISound.PlayBackButtonClick();
        LineContainer.transform.SetParent(gameObject.transform);

        foreach (Transform child in Content.transform)
            Destroy(child.gameObject);

        Close();

        Core.UIManager.GetUI<UIScreenFade>().FadeTo(1f, 0f, 0.2f);
    }

    private void OnBtnDown(PointerEventData enterEvent)
    {
        ExitBtn.transform.DOScale(new Vector3(-0.9f, 0.9f, 1f), 0.2f);
    }

    private void OnBtnUp(PointerEventData exitEvent)
    {
        ExitBtn.transform.DOScale(new Vector3(-1f, 1f, 1f), 0.2f);
    }
}
