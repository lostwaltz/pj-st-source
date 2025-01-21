using System;
using UnityEngine;
using UnityEngine.UI;

public class UICombatSelectUnit : UIAnimation
{
    [SerializeField] private GameObject[] FilterButtons;
    [SerializeField] private UIUnitList uiUnitList;
    [SerializeField] private Image combatStartButton;
    [SerializeField] private Button clearUnitButton;
    
    [SerializeField] private GameObject mark;
    
    private Camera cam; 

    private Unit selectedUnit;

    protected override void Awake()
    {
        base.Awake();
        BindEvent(FilterButtons[0], _ =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            uiUnitList.SetFilterType(FilterType.None);
        });
        BindEvent(FilterButtons[(int)ExternalEnums.Speciality.Vanguard + 1], _ =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            uiUnitList.SetFilterType(FilterType.BySpecialityVanguard);
        });
        BindEvent(FilterButtons[(int)ExternalEnums.Speciality.Striker + 1], _ =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            uiUnitList.SetFilterType(FilterType.BySpecialityStriker);
        });
        BindEvent(FilterButtons[(int)ExternalEnums.Speciality.Supporter + 1], _ =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            uiUnitList.SetFilterType(FilterType.BySpecialitySupporter);
        });
        BindEvent(FilterButtons[(int)ExternalEnums.Speciality.Defender + 1], _ =>
        {
            UIBattle.PlayUIBattleClickNormalSound();
            uiUnitList.SetFilterType(FilterType.BySpecialityDefender);
        });

        BindEvent(combatStartButton.gameObject, _ =>
        {
            if (GameUnitManager.Instance.PrevUnits.Count <= 0)
            {
                var ui = Core.UIManager.GetUI<UIPopupMessage>();
                ui.ShowMessage("유닛을 배치해주세요!");

                return;
            }

            UIBattle.PlayUIBattleCharacterAssign();
            BGM.StopBattleIntroBGM();
            BGM.PlayBattleBGM();
            
            Core.EventManager.Publish(new GameStartEvent());
            
            UIInGameManager.Instance.Init();
            GameUnitManager.Instance.InitPrevUnits();

            Close();
        });

        clearUnitButton.onClick.AddListener(RemoveUnit);
    }

    protected override void Start()
    {
        base.Start();

        cam = Camera.main;

        var elementList = uiUnitList.GetUnitElementList();
        foreach (var element in elementList)
            element.TryInstantiateUnit += InstantiateUnit;
        MusicManager.Instance.Stop();
        BGM.PlayBattleIntroBGM(); //todo: 임시 배치 스테이지마다 브금이 달라야함 
        BGE.PlayBattleBGE();

        GameManager.Instance.Input.OnClickPressed += OnClick;
    }

    private void InstantiateUnit(UnitInstance unit, Vector2 position)
    {
        GameUnitManager.Instance.InstantiateUnitByUI(unit, position);
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.Interaction == null) return;

        GameManager.Instance.Input.OnClickPressed -= OnClick;
    }

    private void OnClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(ray, out var hit, Mathf.Infinity, LayerMask.GetMask("Unit"));

        if (!isHit || !hit.collider.TryGetComponent(out Unit unit)) return;
        
        if(!(unit as PlayerUnit)) return;
            
        selectedUnit = unit;
    }

    private void UpdatePlacementStateOnRemove(Unit unit)
    {
        if (unit.type != GameUnitManager.Playable) return;

        StageSO currentStage = Core.DataManager.SelectedStage;
        int placementIndex = currentStage.playerPlacements.FindIndex(p => p.coord == unit.curCoord);
        if (placementIndex != -1)
        {
            var placement = currentStage.playerPlacements[placementIndex];
            placement.isPlaced = false;
            currentStage.playerPlacements[placementIndex] = placement;
        }
    }

    private void RemoveUnit()
    {
        if (null == selectedUnit) return;

        UpdatePlacementStateOnRemove(selectedUnit);
        selectedUnit.ReleaseUnit();
        GameUnitManager.Instance.PrevUnits.Remove(selectedUnit);
        UIBattle.PlayUIBattleCharacterDeploy();
    }

    private void Update()
    {
        if (null == selectedUnit)
        {
            mark.SetActive(false);
            return;
        }
        
        //TODO: 매직넘버 수정
        mark.SetActive(true);
        
        mark.transform.position = selectedUnit.transform.position + new Vector3(0, 2.2f, 0);
        
        Vector3 direction = cam.transform.forward;
        direction.y = 0;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Vector3 currentRotation = mark.transform.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, targetRotation.eulerAngles.y, currentRotation.z);
        
        mark.transform.rotation = Quaternion.Euler(newRotation);
    }
}
