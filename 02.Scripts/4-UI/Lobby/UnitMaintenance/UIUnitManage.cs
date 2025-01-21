using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EnumTypes;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitManage : UIBase
{
    [SerializeField] private UIUnitScroll uiUnitScroll;
    [SerializeField] private UIUnitStatusDisplay uiUnitStatusDisplay;
    [SerializeField] private Button ExitBtn;

    [Header("상세정보")]
    [SerializeField] private UIUnitManageClassInfo uiUnitManageClassInfo;

    [SerializeField] private Button btnClassInfo;

    private GameObject aimoTrainingPlatform;

    private void Start()
    {

        BindEvent(ExitBtn.gameObject, (eventData) => { ExitBtn.transform.DOScale(new Vector3(-0.9f, 0.9f, 1f), 0.2f); }, UIEvent.Down);
        BindEvent(ExitBtn.gameObject, (eventData) => { ExitBtn.transform.DOScale(new Vector3(-1f, 1f, 1f), 0.2f); }, UIEvent.Up);
        ExitBtn.onClick.AddListener(OnSubmit);

        btnClassInfo.onClick.AddListener(() => { uiUnitManageClassInfo.Open(); });
    }

    protected override void OpenProcedure()
    {
        // GameObject aimoPrefab = Resources.Load<GameObject>("Prefabs/Maps/Aimo_Training_Platform");
        // aimoTrainingPlatform = Instantiate(aimoPrefab, Vector3.zero, Quaternion.identity);
        LobbyManager.Instance.MoveSpace(LobbySpace.TrainingPlatform);
        
        uiUnitStatusDisplay.Open();
        uiUnitScroll.Open();

        BGM.PlayBGMAfterFadeOut(() =>
        {
            BGM.PlayMaintenanceRoomBGM();
        });
    }

    private void OnSubmit()
    {
        UISound.PlayBackButtonClick();
        Core.UIManager.GetUI<UIScreenFade>().FadeTo(0f, 1f, 0.2f).
            OnComplete(() =>
            {
                OnExit();
                Core.UIManager.OpenUI<UILobbyMain>();

                Core.UGSManager.Data.CallSave();
            });
    }

    private void OnExit()
    {
        foreach (var slot in uiUnitScroll.GetUIUnitScrollSlots())
        {
            slot.OnDestroyTrainingRoomUnit();
        }
        // if (aimoTrainingPlatform != null)
        // {
        //     Destroy(aimoTrainingPlatform);
        // }

        Close();

        Core.UIManager.GetUI<UIScreenFade>().FadeTo(1f, 0f, 0.2f);
    }

    public void InitClose()
    {
        foreach (var slot in uiUnitScroll.GetUIUnitScrollSlots())
        {
            slot.OnDestroyTrainingRoomUnit();
        }


        Close();
    }
}
