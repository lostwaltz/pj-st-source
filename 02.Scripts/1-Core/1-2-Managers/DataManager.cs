using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using UGS;
using UnityEngine;

public class DataManager
{
    //SO DATA
    public Dictionary<int, StageSO> StageDict = new();
    public List<StageSO> StageDataList = new();
    public SelectorListSO SkillSelectorList;
    
    //JSON DATA
    public DataBase<SkillInfo> SkillTable { get; private set; }
    public DataBase<UnitInfo> UnitTable { get; private set; }
    public DataBase<WeaponInfo> WeaponTable { get; private set; }
    public DataBase<GachaTable> GachaTable { get; private set; }
    public DataBase<AchievementInfo> AchTable { get; private set; }
    public DataBase<DialogInfo> DialogInfo { get; private set; }
    public DataBase<DialogTable> DialogTable { get; private set; }
    
    //USER INTERACTION DATA
    public StageSO SelectedStage = null;
    public int GachaKey = 1; //아무것도 누르지 않았을때, 초기화값
    public int GachaCount;
    
    //GAME INSTANCE DATA
    // public readonly Dictionary<StageSO, bool> StageClearDict = new();
    public readonly Dictionary<int, bool> StageClearData = new();
    
    
    public void Init()
    {
        SkillTable = new DataBase<SkillInfo>(Utils.Str.Clear().Append(Path.DataPath).Append("JSON/SkillInfo").ToString());
        UnitTable = new DataBase<UnitInfo>(Utils.Str.Clear().Append(Path.DataPath).Append("JSON/UnitInfo").ToString());
        WeaponTable = new DataBase<WeaponInfo>(Utils.Str.Clear().Append(Path.DataPath).Append("JSON/WeaponInfo").ToString());
        GachaTable = new DataBase<GachaTable>(Utils.Str.Clear().Append(Path.DataPath).Append("JSON/GachaTable").ToString());
        AchTable = new DataBase<AchievementInfo>(Utils.Str.Clear().Append(Path.DataPath).Append("JSON/AchievementInfo").ToString());
        DialogInfo = new DataBase<DialogInfo>(Utils.Str.Clear().Append(Path.DataPath).Append("JSON/DialogInfo").ToString());
        DialogTable = new DataBase<DialogTable>(Utils.Str.Clear().Append(Path.DataPath).Append("JSON/DialogTable").ToString());
        
        StageSOList stageDataList = Resources.Load<StageSOList>("Data/SOs/StageSOList");
        foreach (StageSO stageSo in stageDataList.stages)
        {
            StageDict.Add(stageSo.stageData.StageKey, stageSo);
            // StageClearDict.Add(stageSo, false);
            StageClearData.Add(stageSo.stageData.StageKey, false);
        }
        StageClearData.Add(-1, false);
        
        StageDataList = stageDataList.stages.ToList();

        SelectedStage = StageDataList.First();

        SkillSelectorList = Resources.Load<SelectorListSO>("Data/SOs/SkillSelectorSOList");
        
        AchievementManager.Instance.Init();
        Core.UGSManager.Data.OnSaveCalled += SaveData;
        Core.UGSManager.Data.OnAllDataLoaded += LoadData;
    }
    
    private void SaveData(Dictionary<Type, ICloudDataContainer> datas)
    {
        CloudDataContainer<PlayerStageClear> clearData = datas[typeof(PlayerStageClear)] as CloudDataContainer<PlayerStageClear>;
        CloudDataContainer<PlayerAchievement> achData = datas[typeof(PlayerAchievement)] as CloudDataContainer<PlayerAchievement>;
        
        if (achData == null) return;
        
        // if(clearData.Map.TryGetValue())
        foreach (var key in StageClearData.Keys) 
        {
            // -1 : 튜토리얼
            if (clearData.Map.TryGetValue(key, out int index))
                clearData.Data[index].Set(StageClearData[key]);
            else
                clearData.Add(new PlayerStageClear(key, StageClearData[key]));
        }

        foreach (KeyValuePair<int, AchievementInstance> pair in AchievementManager.Instance.achievementInstances)
        {
            float progress = pair.Value.CurProgress;
            bool takeReward = pair.Value.IsTakeReward;

            if (achData.Map.TryGetValue(pair.Key, out int index))
                achData.Data[index].Set(pair.Value.CurProgress, pair.Value.IsTakeReward);
            else
                achData.Add((new PlayerAchievement(pair.Key, progress, takeReward)));
        }
    }
    
    private void LoadData(Dictionary<Type, ICloudDataContainer> datas)
    {
        if (!AchievementManager.HasInstance)
            AchievementManager.Instance.Init();
        
        CloudDataContainer<PlayerStageClear> clearData = datas[typeof(PlayerStageClear)] as CloudDataContainer<PlayerStageClear>;
        CloudDataContainer<PlayerAchievement> achData = datas[typeof(PlayerAchievement)] as CloudDataContainer<PlayerAchievement>;

        if(clearData == null) return;
        if(achData == null) return;
        
        foreach (var data in clearData.Data)
        {
            if (StageClearData.ContainsKey(data.Key))
                StageClearData[data.Key] = data.IsClear;
            else
                StageClearData.Add(data.Key, data.IsClear);
        }

        foreach (var data in achData.Data)
        {
            var instance = AchievementManager.Instance.achievementInstances[data.Key];
            instance.IncrementProgress(data.Progress);
            instance.IsTakeReward = data.IsTakeReward;
        }
    }
}
