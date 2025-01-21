using EnumTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIGachaScrollSlot : UIBase
{
    [SerializeField] private GameObject icon;

    public UIStartGachaPanel uiStartGachaBGPanel;
    public UIRareGachaPanel uiRareGachaPanel;
    public UIEpicGachaPanel uiEpicGachaPanel;
    public UILegendaryGachaPanel uiLegendaryGachaPanel;
    public UIOnlyLegendaryGachaPanel uiOnlyLegendaryGachaPanel;

    //public int SelectedKey { get; set; }
    public int index;

    //UIGachaScroll Script에서 인덱스 결정해줌

    private UIDragHandler uiDragHandler;
    [SerializeField] private GameObject uiGachaScrollView;

    private void Awake()
    {
        uiDragHandler = new UIDragHandler(uiGachaScrollView);
        BindEvent(gameObject, uiDragHandler.HandleDrag, UIEvent.Drag);
        BindEvent(gameObject, uiDragHandler.HandleDragBegin, UIEvent.BeginDrag);
        BindEvent(gameObject, uiDragHandler.HandleDragEnd, UIEvent.EndDrag);

        BindEvent(gameObject, OnEnter, UIEvent.Enter);
        BindEvent(gameObject, OnExit, UIEvent.Exit);
        BindEvent(gameObject, OnClick);
    }

    public void OnClick(PointerEventData eventData)
    {
        //데이터 매니저에 키값 저장하기
        Core.DataManager.GachaKey = index;
        
        if (index == 1)
        {
            CloseAllPanel();
            uiStartGachaBGPanel.Open();
        }
        if (index == 2)
        {
            CloseAllPanel();
            uiRareGachaPanel.Open();
        }
        if (index == 3)
        {
            CloseAllPanel();
            uiEpicGachaPanel.Open();
        }
        if (index == 4)
        {
            CloseAllPanel();
            uiLegendaryGachaPanel.Open();
        }
        if (index == 5)
        {
            CloseAllPanel();
            uiOnlyLegendaryGachaPanel.Open();
        }

        UIShop.PlayChangeGacha();
    }

    public void OnEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        transform.DOScale(Vector3.one * 1.1f, 0.2f);
    }

    public void OnExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
        transform.DOScale(Vector3.one, 0.2f);
    }

    public void CloseAllPanel()
    {
        Core.UIManager.CloseUI<UIGachaWarningPanel>();
        Core.UIManager.CloseUI<UIGradeInfoCanvas>();
        uiStartGachaBGPanel.Close();
        uiRareGachaPanel.Close();
        uiEpicGachaPanel.Close();
        uiLegendaryGachaPanel.Close();
        uiOnlyLegendaryGachaPanel.Close();
    }

}