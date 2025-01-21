using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumTypes;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class HowToPlayingGame : Tutorial
{
    private bool controlUpdate = false;
    private Action callback;

    private void CallEvent()
    {
        callback?.Invoke();
        
        callback = null;
    }

    private void MoveCamera(Vector2 pos)
    {
        Core.InputManager.OnMoveReceived -= MoveCamera;
        callback = null;
        
        CallEvent();

        ShowRotate();
    }

    private void ShowRotate()
    {
        UIPopupMessage popupMessage = Core.UIManager.GetUI<UIPopupMessage>();
        popupMessage.ShowMessage("Q E 키로 카메라를 회전할 수 있습니다.", ref callback);
        Core.InputManager.OnRotateReceived += RotateCamera;
    }

    private void RotateCamera(int rotation)
    {
        CallEvent();
    }
    
    public override void Enter()
    { 
        Core.UIManager.CloseUI<UICombatSelectUnit>();
        UIBattleCanvas.Reservation = true;

        GameUnitManager.Instance.InstantiateUnitByUI(Core.UnitManager.unitInstanceList[0], new Vector2(4, 0));
        GameUnitManager.Instance.InstantiateUnitByUI(Core.UnitManager.unitInstanceList[1], new Vector2(4, 1));

        DialogManager.Instance.ShowDialog<UIMessageDialog>(700100);

        Unit unit = GameUnitManager.Instance.PrevUnits[0];

        IndicatorCursor cursor = GameManager.Instance.Indicator.GetInTutorial<IndicatorCursor>();
        cursor.gameObject.SetActive(false);

        GameManager.Instance.Interaction.layerMask = 0;
        GameManager.Instance.Interaction.layerMask = 1 << 7;
        GameManager.Instance.Interaction.OnClicked += Interaction;
        
        UIDialog.DialogsContainer[5].OnEnter += () =>
        {
            UIPopupMessage popupMessage = Core.UIManager.GetUI<UIPopupMessage>();
            popupMessage.ShowMessage("W A S D 키로 카메라를 움직일 수 있습니다", ref callback);
            Core.InputManager.OnMoveReceived += MoveCamera;
            cursor.gameObject.SetActive(true);
            
            Manager.boxCollider.enabled = true;
            Manager.transform.position = GameUnitManager.Instance.PrevUnits[0].transform.position;
            
            UIDialog.SetActiveInteractable(false);
            cursor.Show(unit.curCoord);
            
            GameUnitManager.Instance.InitPrevUnits();
            Core.EventManager.Publish(new GameStartEvent());
            UIInGameManager.Instance.Init();
        };
        UIDialog.DialogsContainer[6].OnEnter += () =>
        {
            Manager.transform.position = StageManager.Instance.cellMaps[new Vector2(1, 0)].transform.position;
            
            
            GameManager.Instance.Indicator.ShowSelectedPlayer(unit.curCoord);
            GameManager.Instance.Indicator.ShowStageCell(0, unit.curCoord, unit.data.UnitBase.StepRange);
            cursor.Show(new Vector2(1, 0));
        };
        UIDialog.DialogsContainer[7].OnEnter += () =>
        {
            Manager.boxCollider.enabled = false;
            
            StageCell cell = StageManager.Instance.cellMaps[new Vector2(1, 0)];
            GameManager.Instance.Interaction.curSelected = unit as PlayerUnit;
            UIBattleCanvas ui = Core.UIManager.OpenUI<UIBattleCanvas>();
            ui.OnAddInteractionUnit(unit as PlayerUnit);
            ui.uiPlayableUnitPanel.uiSkillSlotsPanel.cancelButton.gameObject.SetActive(false);
            GameManager.Instance.Indicator.ShowMovementIndicator(unit, cell.placement.coord);
            unit.CommandSystem.UpdateCommand(0, cell.placement.coord, InstanceMoveCommand);
            GameManager.Instance.Mediator.OnSkillSelected += WrapNext;
        };
        UIDialog.DialogsContainer[8].OnEnter += () =>
        {
            GameManager.Instance.Interaction.layerMask = 0;
            GameManager.Instance.Interaction.layerMask = 1 << 7;
            
            Manager.boxCollider.enabled = true;
            GameManager.Instance.Mediator.OnSkillSelected -= WrapNext;
            
            
            Unit enemy = GameUnitManager.Instance.Units[UnitType.EnemyUnit].Last();
            
            Manager.transform.position = enemy.transform.position;

            cursor.Show(enemy.curCoord);
        };
        
        UIDialog.DialogsContainer[9].OnEnter += () =>
        {
            GameManager.Instance.Interaction.OnClicked -= Interaction;
            
            UIBattleCanvas ui = Core.UIManager.OpenUI<UIBattleCanvas>();
            
            ui.uiPlayableUnitPanel.uiSkillSlotsPanel.OnClickSkillTarget(null);
            foreach (var skill in ui.uiPlayableUnitPanel.uiSkillSlotsPanel.uiSkillsList)
            {
                skill.GetComponent<Button>().enabled = false;
            }
            
            Unit enemy = GameUnitManager.Instance.Units[UnitType.EnemyUnit].Last();

            List<Unit> targets = new List<Unit>() {enemy};
            GameManager.Instance.Indicator.ShowAttackIndicator(unit.curCoord, enemy.curCoord);
            GameManager.Instance.Indicator.ShowAttackEstimate(unit, unit.curCoord, ref targets);
            
            unit.CommandSystem.UpdateCommand(1, unit.curCoord, CreateCommand);
            
            ui.uiPlayableUnitPanel.uiSkillSlotsPanel.OnConfirmSkill += WrapNext;
        };

        UIDialog.DialogsContainer[10].OnEnter += () =>
        {
            UIDialog.SetActiveInteractable(true);
            GameManager.Instance.Indicator.HideAll();
            cursor.gameObject.SetActive(false);
            Manager.IsCompleteTutorial = true;
            
            Core.DataManager.StageClearData[-1] = true;
            // PlayerPrefs.SetInt("IsCompleteTutorial", 1);
        };
        
        UIDialog.DialogsContainer[11].OnEnter += () =>
        {
            GameManager.Instance.Interaction.layerMask = GameManager.Instance.Interaction.OriginLayer;
            GameManager.Instance.Interaction.OnClicked -= Interaction;
            UIBattleCanvas.Reservation = false;

            controlUpdate = true;
            
            UIBattleCanvas ui = Core.UIManager.GetUI<UIBattleCanvas>();
            
            foreach (var skill in ui.uiPlayableUnitPanel.uiSkillSlotsPanel.uiSkillsList)
                skill.GetComponent<Button>().enabled = true;
            
            ui.uiPlayableUnitPanel.uiSkillSlotsPanel.cancelButton.gameObject.SetActive(true);
            ui.uiPlayableUnitPanel.uiSkillSlotsPanel.uiActionCancelPanel.gameObject.SetActive(true);
        };
    }

    private IUnitCommand InstanceMoveCommand()
    {
        return new MoveCommand(GameUnitManager.Instance.PrevUnits[0], StageManager.Instance.cellMaps[new Vector2(1, 0)]);
    }
    
    public IUnitCommand CreateCommand()
    {
        Unit unit = GameUnitManager.Instance.PrevUnits[0];
        Unit enemy = GameUnitManager.Instance.Units[UnitType.EnemyUnit].Last();
        List<Unit> targets = new List<Unit>(){enemy};
        List<int> damages = new List<int>(){9999};
        
        return new AttackCommand(unit, 0, targets, damages);
    }

    private void Interaction(IClickable hit)
    {
        UIDialog.NextDialog(null);
    }

    private void WrapNext(int _)
    {
        UIDialog.NextDialog(null);
    }

    private void WrapNext()
    {
        UIDialog.NextDialog(null);
    }
    
    public override void Exit()
    {
        Core.UGSManager.Data.CallSave();
    }

    public override void Execute()
    {
        if (!controlUpdate)
        {
            UIBattleCanvas ui = Core.UIManager.GetUI<UIBattleCanvas>();
            ui.uiPlayableUnitPanel.uiSkillSlotsPanel.uiActionCancelPanel.gameObject.SetActive(false);
        }
    }
}
