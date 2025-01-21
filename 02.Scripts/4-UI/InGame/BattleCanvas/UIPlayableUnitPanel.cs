using UnityEngine;

public class UIPlayableUnitPanel : UIBase
{
    [SerializeField] private UIUnitInfoPanel uiUnitInfoPanel;
    [SerializeField] public UISkillSlotsPanel uiSkillSlotsPanel;
    [SerializeField] public UIActionCancelPanel uiActionCancelPanel;

    public void SetUnitInfo(Unit selectedUnit)
    {
        //TODO : 조건 수정 필요 ex.게임시작 즉시
        // 자식들 초기화 중
        uiUnitInfoPanel.Open();
        uiUnitInfoPanel.SetUnitInfo(selectedUnit);
        uiSkillSlotsPanel.Open();
        uiSkillSlotsPanel.SetUnitInfo(selectedUnit);
        // uiActionCancelPanel.Open(); -> 스킬 슬롯 초기화할 때, 같이 열어줌
    }
}