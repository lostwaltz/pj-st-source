using System;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class UISkillSlotsPanel : UIBase
{
    [SerializeField] private UIUnitInfoPanel uiUnitInfolPanel;

    [SerializeField] public UIActionCancelPanel uiActionCancelPanel;
    [SerializeField] private UISelectionPanel uiSelectionPanel;
    [SerializeField] private UISkillInfoPanel uiSkillInfoPanel;
    [SerializeField] private UILeftSkillInfoPanel uiLeftSkillInfoPanel;

    private UIReselectionCanvas uiReselectionCanvas; //외부 데이터

    [SerializeField] private Button confirmButton;
    [SerializeField] public Button cancelButton;
    
    [SerializeField] private CanvasGroup confirmButtonCanvasGroup;

    [SerializeField] public List<UISkill> uiSkillsList = new();
    
    public int selectedSkill;
    
    InteractionMediator mediator;
    
    public event Action OnConfirmSkill;
    
    
    void Start()
    {
        // GameManager.Instance.Interaction.OnClicked += OnClickEnemy;
        uiReselectionCanvas = Core.UIManager.GetUI<UIReselectionCanvas>();
        uiReselectionCanvas.Close();

        mediator = GameManager.Instance.Mediator;
        mediator.OnSkillSelected += InteractSkill;
        mediator.OnSkillInteracted += OnClickSkillTarget;
        mediator.OnAfterReverted += AfterReverted;
        // GameManager.Instance.Interaction.OnWrongSelected += OnClick;
    }
    
    public override void Open()
    {
        base.Open();
        InitializeUI();
    }
    
    void InitializeUI()
    {
        uiLeftSkillInfoPanel.Close();
        uiSelectionPanel.Close();
        
        uiActionCancelPanel.Open();
        uiUnitInfolPanel.Open();
    }

    public void SetUnitInfo(Unit selectedUnit)
    {
        UnitInstance instance = selectedUnit.data;
        UnitHealthSystem unitHealthSystem = selectedUnit.HealthSystem;
        UnitSkillSystem skillSystem = selectedUnit.SkillSystem;

        List<Skill> skillInfoList = skillSystem.Skills;

        int remolding = selectedUnit.GaugeSystem.GetGauge<ReMolding>().Value;

        for (int i = 0; i < uiSkillsList.Count; i++)
        {
            if (skillInfoList[i].Data.SkillBase.Key == 999999 || skillInfoList[i].Data.SkillBase.Key == 999998 || skillInfoList[i].Data.SkillBase.Key == 999997|| skillInfoList[i].Data.SkillBase.Key == 999996)
            {
                uiSkillsList[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                uiSkillsList[i].transform.parent.gameObject.SetActive(true);
            }

            uiSkillsList[i].skillIndex = i;
            uiSkillsList[i].skillSlot.sprite = Resources.Load<Sprite>(skillInfoList[i].Data.SkillBase.Path);
            uiSkillsList[i].skillName = skillInfoList[i].Data.SkillBase.Name;
            uiSkillsList[i].skillDescription = skillInfoList[i].Data.SkillBase.Description;
            uiSkillsList[i].skillTargetDescription = skillInfoList[i].Data.SkillBase.TargetDiscription;

            uiSkillsList[i].SkillButton.interactable = skillInfoList[i].Data.Cost <= remolding;

            UISkill skill = uiSkillsList[i];
            skill.SetRemolding(skillInfoList[i].Data.Cost);
            
            switch (selectedUnit.SkillSystem.Skills[i].Type)
            {
                case SkillType.Active:
                    uiSkillsList[i].skillLevel = skillInfoList[i].Data.Level + " <size=16>액티브</size>";
                    break;
                case SkillType.Passive:
                    uiSkillsList[i].skillLevel = skillInfoList[i].Data.Level + " <size=16>패시브</size>";
                    break;
                default:
                    return;
            }
        }
    }

    public void OnInteractionSkill(int index)
    {
        // 한 메서드에서 해주어야할 것이 너무 많음
        UISound.PlayStageNodeClick();
        mediator.CallSkillSelect(index);
        
        // 리셀렉션은 일종의 예외처리이므로 잠시 주석처리
        // if (selectedSkill == index)
        // {
        //     uiReselectionCanvas.Open();
        //     //동일 조건
        //     ToggleConfirmButton(false);
        //     
        // }
        // else
        // {
        // uiReselectionCanvas.Close();
        // uiSkillsList[selectedSkill].skillSlotImgOutLine.enabled = false;
        // uiSkillsList[selectedSkill].skillSelectedImg.enabled = false;     
        // }
    }

    void InteractSkill(int index)
    {
        // 행동 분리
        // 스킬을 골랐을때 해주어야할 일
        ToggleSelectedMark(selectedSkill, false);
        
        selectedSkill = index;

        SetReadyUI();

        // TODO : 몬스터를 클릭한 순간이 아닐때 => 스킬만 클릭시, 클릭불가하게 만들기 OK
        ToggleConfirmButton(false);

        // GameManager.Instance.Interaction.SelectSkill(selectedSkill);
        ToggleSelectedMark(selectedSkill, true);
    }

    void SetReadyUI()
    {
        uiSkillInfoPanel.Close();       // 스킬 정보 패널 닫아주기
        uiSelectionPanel.Open();        // 스킬 클릭 시 확인취소 패널오픈
        uiActionCancelPanel.Close();    // 행동 종료 패널 닫아주기

        uiUnitInfolPanel.Close();       // 플레이어블 유닛 인포 닫아주기
        uiLeftSkillInfoPanel.SetSkillInfoTxt(selectedSkill); // 왼쪽스킬인포 열어주기
    }

    void ToggleSelectedMark(int index, bool isOn)
    {
        uiSkillsList[index].skillSlotImgOutLine.enabled = isOn;
        uiSkillsList[index].skillSelectedImg.enabled = isOn;
    }

    void ToggleConfirmButton(bool isOn)
    {
        confirmButton.interactable = isOn;
        confirmButtonCanvasGroup.alpha = isOn ? 1f : 0.5f;
    }

    // 스킬의 선택 대상을 전달 받고 그에 따라 UI 처리
    public void OnClickSkillTarget(IClickable thing)
    {
        // if (thing is EnemyUnit)
        // {
        // }
        
        uiReselectionCanvas.Close();
        
        if (!uiSelectionPanel.gameObject.activeSelf)
        {
            uiSelectionPanel.Open();
            uiActionCancelPanel.Close();
            // 적을 바로 클릭했을텐데, 그럼..........0번이 선택된것 처럼 활성화 되어야함.

            selectedSkill = 0;

            uiUnitInfolPanel.Close(); // 플레이어블 유닛 인포 닫아주기
            uiLeftSkillInfoPanel.SetSkillInfoTxt(selectedSkill); // 왼쪽 스킬 인포 열어주기

            // GameManager.Instance.Interaction.SelectSkill(selectedSkill);
        
            ToggleSelectedMark(selectedSkill, true);
        }
        ToggleConfirmButton(true);
    }

    public void OnClickConfirmBtn()
    {
        OnConfirmSkill?.Invoke();
        OnConfirmSkill = null;
        UISound.PlayStageNodeClick();
        uiSelectionPanel.Close();
        //초기화
        ToggleSelectedMark(selectedSkill, false);

        uiLeftSkillInfoPanel.Close();

        GameManager.Instance.Interaction.ExecuteCommand();
        
        // uiActionCancelPanel.Open(); 굳이 필요해 보이지 않은?
        // selectedSkill = 10;
        //Close();
    }

    public void OnClickCancelBtn()
    {
        UISound.PlayStageNodeClick();
        mediator.CallRevert();
    }
    
    // CancelSkill() 수정
    // commandType : 0 - Move, 1 - UseSkill
    // commandStep : 0 - Ready, 1 - Determine
    void AfterReverted(PlayerCommandType commandType, ReceiverStep commandStep)
    {
        // 행동 분리
        // TODO : UISkill <OutLine, Image> 꺼줘야함
        switch (commandType)
        {
            case PlayerCommandType.Move:
                // 캐릭터 이동 상태
                ToggleSelectedMark(selectedSkill, false);
                Open();
                break;
            case PlayerCommandType.UseSkill:
                if (commandStep.Equals(ReceiverStep.Ready))
                {
                    SetReadyUI();
                    ToggleConfirmButton(false);
                }
                break;
        }
    }
    
    
}