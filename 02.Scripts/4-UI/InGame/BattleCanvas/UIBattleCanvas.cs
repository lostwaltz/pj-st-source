using System;
using EnumTypes;
using UnityEngine;

public class UIBattleCanvas : UIBase
{
    public static bool Reservation;
    
    [SerializeField] public UIPlayableUnitPanel uiPlayableUnitPanel;
    [SerializeField] public UIMenuPanel uiMenuPanel;
    [SerializeField] private UIStageGoalPanel uiStageGoalPanel;

    private void Awake()
    {
        
        uiMenuPanel.Open();
    }

    private void Start()
    {
        uiStageGoalPanel.Open();

        uiPlayableUnitPanel.Close();
        
        // PlayerInteraction interaction = GameManager.Instance.Interaction;
        // interaction.OnUnitClicked += OnAddInteractionUnit;
        GameManager.Instance.Interaction.OnPlayerUnitSelected += OnAddInteractionUnit;
        
        GameManager gameManager = GameManager.Instance;
        gameManager.SubscribePhaseEvent(GamePhase.EnemyTurn, uiPlayableUnitPanel.Close);
        
        if(Reservation)
            uiMenuPanel.Close();
    }

    public void OnAddInteractionUnit(PlayerUnit unit)
    {
        uiPlayableUnitPanel.Open();
        uiPlayableUnitPanel.SetUnitInfo(unit);
    }

    public void CloseUIPlayerableUnit()
    {
        uiPlayableUnitPanel.Close();
    }
    
    //TODO : 여기서 관리 해줘야 할 것
    // 1 :  확인 취소 버튼 
    // 2 :  행동 취소 버튼
    // 3 :  플레이어 인포 패널
    // 4 :  메뉴 패널
    // 5 :  패널
    // 스킬 슬롯에서 관리 해줘야하는 것 : 마우스 올렸을때 = 스킬 패널 에서 관리 
    
    
}
