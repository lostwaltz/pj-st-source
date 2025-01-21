using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UILobbyMain : UIBase
{
    [Header("RightSideUIGroup")]
    public CanvasGroup CanvasGroupRightSide;
    [SerializeField] private Button BtnStage;
    [SerializeField] private Button BtnUnitManage;
    [SerializeField] private Button BtnGacha;
    [SerializeField] private Button BtnAchievement;

    [Header("TopUIGroup")]
    public CanvasGroup CanvasGroupTop;
    [SerializeField] private TMP_Text TextCredits;
    [SerializeField] private TMP_Text TextShards;

    [Header("BottomLeftUIGroup")]
    public GameObject CanvasGroupBottomLeft;
    [SerializeField] private Button BtnSetting;
    [SerializeField] private Button BtnAdjutant;
    [SerializeField] private Button BtnChat;

    private Action<int> onCreditsChanged;
    private Action<int> onShardsChanged;

    [Header("UnitManageUI")]
    private UIUnitManage uiUnitManage;
    [Header("GachaManagerUI")]
    private UIGachaManager uiGachaManager;
    [Header("CommandCenterUI")]
    private UIAdjutantScroll uiAdjutantScroll;
    [Header("AchievementUI")]
    private UIAchievement uiAchievement;

    private void Start()
    {
        InitRightSideUI();
        InitTopSideUI();
        InitBottomLeftUI();
    }
    protected override void OpenProcedure()
    {
        base.OpenProcedure();
        BGE.StopCurrentBGE();
        BGE.PlayMainMenuBGE();
        UISound.PlayLobbyMain();
        InitTopSideUI();
        LobbyManager.Instance.MoveSpace(LobbySpace.CommandRoom);

        BGM.PlayBGMAfterFadeOut(() =>
        {
            BGM.PlayMainMenuBGM();
        });
    }

    private void InitRightSideUI()
    {
        UIManager manager = Core.UIManager;
        UIScreenFade fade = manager.GetUI<UIScreenFade>();
        fade.Close();

        uiUnitManage = Core.UIManager.GetUI<UIUnitManage>();
        uiUnitManage.InitClose();

        uiGachaManager = Core.UIManager.GetUI<UIGachaManager>();
        uiGachaManager.Close();

        uiAchievement = Core.UIManager.GetUI<UIAchievement>();
        uiAchievement.gameObject.SetActive(false);
        uiAchievement.Close();

        BtnStage.onClick.AddListener(() =>
        {
            UISound.PlayLobbyStartButtonClick(); //출격준비
            fade.FadeTo(0f, 1f, 0.2f).
                OnComplete(() => { fade.FadeTo(1f, 0f, 0.2f); manager.OpenUI<UIStageSelect>(); });
        });

        BtnUnitManage.onClick.AddListener(() =>
        {
            UISound.PlayCharacterMaintanceUI();//보유유닛

            fade.FadeTo(0f, 1f, 0.2f).OnComplete(() =>
            {
                uiUnitManage.Open();
                Close();
                fade.FadeTo(1f, 0f, 0.2f);
            });
        });

        BtnGacha.onClick.AddListener(() =>
        {
            UISound.PlayLobbySceneSelect(); //가챠상점

            fade.FadeTo(0f, 1f, 0.2f).OnComplete(() =>
            {
                Close();
                uiGachaManager.Open();
                fade.FadeTo(1f, 0f, 0.2f);
            });
        });

        BtnAchievement.onClick.AddListener(() =>
        {
            UISound.PlayLobbySceneSelect();
            uiAchievement.Open();
        });

        BindEvent(BtnStage.gameObject, _ => BtnStage.gameObject.transform.DOScale(1.1f, 0.2f), UIEvent.Enter);
        BindEvent(BtnStage.gameObject, _ => BtnStage.gameObject.transform.DOScale(1f, 0.2f), UIEvent.Exit);

        BindEvent(BtnUnitManage.gameObject, _ => BtnUnitManage.gameObject.transform.DOScale(1.1f, 0.2f), UIEvent.Enter);
        BindEvent(BtnUnitManage.gameObject, _ => BtnUnitManage.gameObject.transform.DOScale(1f, 0.2f), UIEvent.Exit);

        BindEvent(BtnGacha.gameObject, _ => BtnGacha.gameObject.transform.DOScale(1.1f, 0.2f), UIEvent.Enter);
        BindEvent(BtnGacha.gameObject, _ => BtnGacha.gameObject.transform.DOScale(1f, 0.2f), UIEvent.Exit);

        BindEvent(BtnAchievement.gameObject, _ => BtnAchievement.gameObject.transform.DOScale(1.1f, 0.2f), UIEvent.Enter);
        BindEvent(BtnAchievement.gameObject, _ => BtnAchievement.gameObject.transform.DOScale(1f, 0.2f), UIEvent.Exit);
    }

    private void InitTopSideUI()
    {
        onCreditsChanged = (value) => TextCredits.text = value.ToString();
        onShardsChanged = (value) => TextShards.text = value.ToString();

        Core.CurrencyManager.SubscribeCurrencyValueChanged<Credits>(onCreditsChanged);
        Core.CurrencyManager.SubscribeCurrencyValueChanged<Shard>(onShardsChanged);

        Core.CurrencyManager.Add<Credits>(0);
        Core.CurrencyManager.Add<Shard>(0);
    }

    private void InitBottomLeftUI()
    {
        bool isPressed = false;
        uiAdjutantScroll = Core.UIManager.GetUI<UIAdjutantScroll>();
        uiAdjutantScroll.Close();

        BtnSetting.onClick.AddListener(() =>
        {
            UISound.PlayMainClickNormal();
        });

        BtnAdjutant.onClick.AddListener(() =>
        {
            UISound.PlayMainClickNormal();
            CanvasGroupTop.alpha = 0f;
            CanvasGroupRightSide.alpha = 0f;

            DOTween.Sequence()
                .Join(CanvasGroupBottomLeft.GetComponent<CanvasGroup>().DOFade(0f, 0.1f).SetEase(Ease.OutQuad))
                .Join(CanvasGroupBottomLeft.transform.DOLocalMoveY(-100f, 1f).SetEase(Ease.OutQuad))
                .OnComplete(() =>
                {
                    //Core.UIManager.OpenUI<UIAdjutant>();
                    uiAdjutantScroll.transform.SetParent(transform);
                    uiAdjutantScroll.Open();
                });
        });

        BtnChat.onClick.AddListener(() =>
        {
            UISound.PlayMainClickNormal();

        });

        void SetButtonEvents(GameObject button)
        {
            BindEvent(button, _ => button.transform.DOScale(1.1f, 0.2f), UIEvent.Enter);
            BindEvent(button, _ =>
            {
                isPressed = true;
                button.transform.DOScale(0.95f, 0.2f);
            }, UIEvent.Down);
            BindEvent(button, _ =>
            {
                isPressed = false;
                button.transform.DOScale(1f, 0.2f);
            }, UIEvent.Up);
            BindEvent(button, _ =>
            {
                if (!isPressed)
                {
                    button.transform.DOScale(1f, 0.2f);
                }
            }, UIEvent.Exit);
        }
        SetButtonEvents(BtnSetting.gameObject);
        SetButtonEvents(BtnAdjutant.gameObject);
        SetButtonEvents(BtnChat.gameObject);
    }

    private void OnDisable()
    {
        Core.CurrencyManager.UnsubscribeCurrencyValueChanged<Credits>(onCreditsChanged);
        Core.CurrencyManager.UnsubscribeCurrencyValueChanged<Shard>(onShardsChanged);

        BtnStage.gameObject.transform.localScale = Vector3.one;
        BtnUnitManage.gameObject.transform.localScale = Vector3.one;
        BtnGacha.gameObject.transform.localScale = Vector3.one;

    }
}
