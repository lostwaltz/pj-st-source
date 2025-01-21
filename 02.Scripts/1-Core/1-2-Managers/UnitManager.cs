using System;
using System.Collections.Generic;
using System.Linq;
using UGS;


public enum SortType
{
    None, Level, Cost, Power
}

[Flags]
public enum FilterType
{
    None = 0,
    BySpecialityVanguard = 1 << 0,
    BySpecialityStriker = 1 << 1,
    BySpecialitySupporter = 1 << 2,
    BySpecialityDefender = 1 << 3
}

public class UnitManager
{
    public readonly Dictionary<SortType, string> MappingDictionary = new();
    
    public readonly List<UnitInstance> unitInstanceList = new();
    public Action<List<UnitInstance>> OnValueChanged;

    private List<UnitInstance> buildInstances;
    
    public void Init()
    {
        Core.UGSManager.Auth.OnSignUp += ProvideStartUnit;
        Core.UGSManager.Data.OnSaveCalled += SaveData;
        Core.UGSManager.Data.OnAllDataLoaded += LoadData;
        
        MappingDictionary.Add(SortType.None, "기본");
        MappingDictionary.Add(SortType.Level, "레벨");
        MappingDictionary.Add(SortType.Cost, "코스트");
        MappingDictionary.Add(SortType.Power, "전투력");
    }
    

    public void AddUnitInfo(UnitInstance unitInstance)
    {
        UnitInstance instance = GetUnitInfoByKey(unitInstance.UnitBase.Key);
        if(instance == null) 
            unitInstanceList.Add(unitInstance);
        else
        {
            instance.AddPieces(30);
        }

        buildInstances = new List<UnitInstance>(unitInstanceList);
        
        OnValueChanged?.Invoke(Build());
    }
    
    public UnitInstance GetUnitInfoByKey(int key)
    {
        return unitInstanceList.Find(unit => unit.UnitBase.Key == key);
    }

    public UnitManager Filter(FilterType filterType)
    {
        if (filterType == FilterType.None)
            return this;

        buildInstances = buildInstances
            .Where(unit =>
                (filterType.HasFlag(FilterType.BySpecialityVanguard) && unit.UnitBase.SpecialityType == ExternalEnums.Speciality.Vanguard) ||
                (filterType.HasFlag(FilterType.BySpecialityStriker) && unit.UnitBase.SpecialityType == ExternalEnums.Speciality.Striker) ||
                (filterType.HasFlag(FilterType.BySpecialitySupporter) && unit.UnitBase.SpecialityType == ExternalEnums.Speciality.Supporter) ||
                (filterType.HasFlag(FilterType.BySpecialityDefender) && unit.UnitBase.SpecialityType == ExternalEnums.Speciality.Defender)
            )
            .ToList();

        return this;
    }
    public UnitManager Sort(SortType sortType, bool ascending = true)
    {
        buildInstances.Sort((x, y) =>
        {
            int comparison = sortType switch
            {
                SortType.Level => x.Level.CompareTo(y.Level),
                SortType.Cost => x.UnitBase.Cost.CompareTo(y.UnitBase.Cost),
                SortType.Power => x.TotalPower.CompareTo(y.TotalPower),
                _ => 0
            };

            return ascending ? comparison : -comparison;
        });

        return this;
    }
    public List<UnitInstance> Build()
    {
        var result = new List<UnitInstance>(buildInstances);
        
        Reset();
        
        return result;
    }

    private void Reset()
    {
        buildInstances = unitInstanceList;
    }

    
    void ProvideStartUnit()
    {
        unitInstanceList.Add(new UnitInstance(Core.DataManager.UnitTable.GetByKey(100100), 1));
        unitInstanceList.Add(new UnitInstance(Core.DataManager.UnitTable.GetByKey(100101), 1));
        
        buildInstances = new List<UnitInstance>(unitInstanceList);
    }
    
    void SaveData(Dictionary<Type, ICloudDataContainer> datas)
    {
        CloudDataContainer<UGS.PlayerUnit> unitData = datas[typeof(UGS.PlayerUnit)] as CloudDataContainer<UGS.PlayerUnit>;

        foreach (var inst in unitInstanceList)
        {
            if (unitData.Map.TryGetValue(inst.UnitBase.Key, out int index))
                unitData.Data[index].Set(inst.Level, inst.CurrentExp, inst.SpineLevel, inst.Pieces);
            else
                unitData.Add(new UGS.PlayerUnit(inst)); // new UnitCloudData(inst);
        }
    }

    void LoadData(Dictionary<Type, ICloudDataContainer> datas)
    {
        CloudDataContainer<UGS.PlayerUnit> unitData = datas[typeof(UGS.PlayerUnit)] as CloudDataContainer<UGS.PlayerUnit>;

        for (int i = 0; i < unitData.Data.Count; i++)
        {
            var inst = new UnitInstance(Core.DataManager.UnitTable.GetByKey(unitData.Data[i].Key), unitData.Data[i].Level);
            inst.CurrentExp = unitData.Data[i].Exp;
            inst.SpineLevel = unitData.Data[i].Spine;
            inst.Pieces = unitData.Data[i].Pieces;
            
            unitInstanceList.Add(inst);
        }
        
        buildInstances = new List<UnitInstance>(unitInstanceList);
    }
}
