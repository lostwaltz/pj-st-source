using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIGachaManager : UIBase
{
    public List<UIGachaScrollSlot> uiGachaScrollSlot = new();
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button detailInfoBtn;
    [SerializeField] private Button creditsAddButton;
    [SerializeField] private TMP_Text creditsText;
    [SerializeField] private UIStartGachaPanel uiStartGachaPanel;
    [SerializeField] private UIRareGachaPanel uiRareGachaPanel;
    [SerializeField] private UIEpicGachaPanel uiEpicGachaPanel;
    [SerializeField] private UILegendaryGachaPanel uiLegendaryGachaPanel;
    [SerializeField] private UIOnlyLegendaryGachaPanel uiOnlyLegendaryPanel;

    private void Start()
    {
        Core.CurrencyManager.SubscribeCurrencyValueChanged<Credits>(SpendCredits);

        BindEvent(exitBtn.gameObject, (eventData) => { exitBtn.transform.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.2f); },
            UIEvent.Down);
        BindEvent(exitBtn.gameObject, (eventData) => { exitBtn.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f); },
            UIEvent.Up);
        exitBtn.onClick.AddListener(OnSubmit);

        creditsText.text = Core.CurrencyManager.GetAmount<Credits>().ToString();
        detailInfoBtn.onClick.AddListener(OnClickDetailBtn);
    }

    protected override void OpenProcedure()
    {
        base.OpenProcedure();

        Init();
        
        BGM.PlayBGMAfterFadeOut(() =>
        {
            BGM.PlayGachaShopBGM();
        });

        BGE.StopCurrentBGE();
        UIShop.PlayGachaChangeScene();

    }

    public void Init()
    {
        uiStartGachaPanel.Open();
        Core.DataManager.GachaKey = 1; // 데이터 매니저 키값 초기화
    }
    
    private void SpendCredits(int value)
    {
        creditsText.text = value.ToString();
    }
    private void OnSubmit()
    {

        UISound.PlayBackButtonClick();
        Core.UIManager.GetUI<UIScreenFade>().FadeTo(0f, 1f, 0.2f).OnComplete(() =>
        {
            OnExit();
            Core.UIManager.OpenUI<UILobbyMain>();
        });
    }

    private void OnClickDetailBtn()
    {
        UIBattle.PlayUIBattleCharacterFilteringWindowOpen();
        Core.UIManager.OpenUI<UIGradeInfoCanvas>();
    }


    private void OnExit()
    {
        Close();
        uiRareGachaPanel.gameObject.SetActive(false);
        uiEpicGachaPanel.gameObject.SetActive(false);
        uiLegendaryGachaPanel.gameObject.SetActive(false);
        uiOnlyLegendaryPanel.gameObject.SetActive(false);
        Core.UIManager.CloseUI<UIGachaWarningPanel>();
        Core.UIManager.CloseUI<UIGradeInfoCanvas>();
        Core.UIManager.GetUI<UIScreenFade>().FadeTo(1f, 0f, 0.2f);
    }
}