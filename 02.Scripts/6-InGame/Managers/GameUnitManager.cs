using System;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;
using System.Linq;

public class GameUnitManager : Singleton<GameUnitManager>
{
    public const UnitType Playable = UnitType.PlayableUnit;
    public const UnitType Enemy = UnitType.EnemyUnit;

    public Dictionary<UnitType, List<Unit>> Units = new();
    public Dictionary<UnitType, Dictionary<int, Unit>> UnitDic = new();
    public Dictionary<UnitType, Dictionary<int, bool>> UnitIsAlive = new();

    public List<Unit> PrevUnits = new();
    public event Action OnValueChangedPrevUnits;
    
    public Action OnGameExit;
    
    protected override void Awake()
    {
        base.Awake();

        Units.Add(Playable, new List<Unit>());
        UnitIsAlive.Add(Playable, new Dictionary<int, bool>());
        UnitDic.Add(Playable, new Dictionary<int, Unit>());

        Units.Add(Enemy, new List<Unit>());
        UnitIsAlive.Add(Enemy, new Dictionary<int, bool>());
        UnitDic.Add(Enemy, new Dictionary<int, Unit>());

        GameManager.Instance.SubscribePhaseEvent(GamePhase.Start, Initialize);
    }

    public void Initialize()
    {
        InstantiateEnemy();
    }

    public void InstantiateUnitByUI(UnitInstance unitInstance, Vector2 coord)
    {
        StageSO currentStage = Core.DataManager.SelectedStage;
        var placement = currentStage.playerPlacements.Find(p => p.coord == coord && !p.isPlaced);

        if (placement.Equals(default(PlayerPlacement)))
            return;

        // 중복 위치 체크
        if (IsUnitExistAtPosition(coord))
            return;

        // 중복 이면 이동
        Unit existingUnit = FindDuplicateUnit(unitInstance);
        if (existingUnit != null)
        {
            UpdateUnitPosition(existingUnit, coord);
            return;
        }

        GameObject unitPrefab = Resources.Load<GameObject>(Constants.Path.Units + "UnitPrefab");
        PlayerUnit playerUnit = Instantiate(unitPrefab, null).GetComponent<PlayerUnit>();
        playerUnit.Init(PrevUnits.Count, coord, currentStage.playerDirection, unitInstance);
        PrevUnits.Add(playerUnit);
        OnValueChangedPrevUnits?.Invoke();
    }


    /// <summary>
    /// 유닛 중복 체크 관련 메서드들
    /// </summary>
    private bool IsUnitExistAtPosition(Vector2 coord)
    {
        bool isUnitExist = Units[Playable].Any(unit => unit.curCoord == coord) ||
                          PrevUnits.Any(unit => unit.curCoord == coord);

        return isUnitExist;
    }

    private Unit FindDuplicateUnit(UnitInstance unitInstance)
    {
        Unit existingUnit = Units[Playable].FirstOrDefault(unit => unit.data.UnitBase.Key == unitInstance.UnitBase.Key);
        if (existingUnit == null)
        {
            existingUnit = PrevUnits.FirstOrDefault(unit => unit.data.UnitBase.Key == unitInstance.UnitBase.Key);
        }
        return existingUnit;
    }

    private void UpdateUnitPosition(Unit unit, Vector2 newCoord)
    {
        StageSO currentStage = Core.DataManager.SelectedStage;
        Vector2 oldCoord = unit.curCoord;

        // 기존 위치 상태 업데이트
        UpdateCellState(oldCoord, false);
        StageManager.Instance.cellMaps[oldCoord].UnitExit();

        // 새 위치로 이동
        unit.curCoord = newCoord;
        unit.transform.position = StageManager.Instance.cellMaps[newCoord].transform.position;
        StageManager.Instance.cellMaps[newCoord].UnitEnter(unit);


        UpdateCellState(newCoord, true);
    }

    private void UpdateCellState(Vector2 coord, bool isPlaced)
    {
        StageSO currentStage = Core.DataManager.SelectedStage;
        int placementIndex = currentStage.playerPlacements.FindIndex(p => p.coord == coord);
        if (placementIndex != -1)
        {
            var placement = currentStage.playerPlacements[placementIndex];
            placement.isPlaced = isPlaced;
            currentStage.playerPlacements[placementIndex] = placement;
        }
    }

    public void InitPrevUnits()
    {
        foreach (var unit in PrevUnits)
        {
            StageSO currentStage = Core.DataManager.SelectedStage;
            Vector2 coord = unit.curCoord;

            int index = PrevUnits.IndexOf(unit);
            unit.index = index;

            Units[Playable].Add(unit);
            UnitIsAlive[Playable].Add(index, true);
            UnitDic[Playable].Add(index, unit);

            int placementIndex = currentStage.playerPlacements.FindIndex(p => p.coord == coord);
            var updatedPlacement = currentStage.playerPlacements[placementIndex];
            updatedPlacement.isPlaced = true;
            currentStage.playerPlacements[placementIndex] = updatedPlacement;
        }
    }

    void InstantiateEnemy()
    {
        GameObject enemyPrefab = Resources.Load<GameObject>(Constants.Path.Units + "EnemyUnitPrefab");
        var currentStage = Core.DataManager.SelectedStage;

        foreach (var placement in currentStage.enemyPlacements)
        {
            int index = Units[Enemy].Count;
            EnemyUnit enemy = Instantiate(enemyPrefab, null).GetComponent<EnemyUnit>();
            UnitInfo info = Core.DataManager.UnitTable.GetByKey(placement.unitKey);
            UnitInstance inst = new UnitInstance(info, placement.level);
            enemy.InitEnemy(index, placement.coord, currentStage.enemyDirection, inst, placement.enemyType);

            Units[Enemy].Add(enemy);
            UnitIsAlive[Enemy].Add(index, true);
            UnitDic[Enemy].Add(index, enemy);
        }
    }

    public void RemoveUnit(UnitType type, Unit unit)
    {
        Units[type].Remove(unit);
        UnitIsAlive[type][unit.index] = false;
    }

    public Vector3 GetNearestEnemyPosition(UnitType typeToFind, Vector3 position)
    {
        List<Unit> units = Units[typeToFind];

        Vector3 result = Vector3.zero;
        float least = float.MaxValue;

        for (int i = 0; i < units.Count; i++)
        {
            float dist = (units[i].transform.position - position).sqrMagnitude;
            if (dist < least)
            {
                least = dist;
                result = units[i].transform.position;
            }
        }

        return result;
    }

    public Unit CheckHealth(UnitType unitType, Unit subject)
    {
        foreach (var unit in Units[unitType])
        {
            if(unit == subject) continue;
            
            if (unit.HealthSystem.Health < unit.HealthSystem.MaxHealth)
                return unit;
        }
        return null;
    }

    public void ReleaseGameScene()
    {
        Core.UIManager.ClearDetector();
        
        foreach (var unitList in Units)
        {
            foreach (var unit in unitList.Value)
                unit.ReleaseUnit();
        }
        
        OnGameExit?.Invoke();

        OnGameExit = null;
    }
}
