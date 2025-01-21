using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementManager : SingletonDontDestroy<AchievementManager>
{
    private Dictionary<string, AchievementCategory> achievementDataDict;
    private EventManager eventManager;
    public IReadOnlyDictionary<string, AchievementCategory> AchievementData => achievementDataDict;
    public int AchievementCount { get; private set; } 

    private const int DefaultKey = -1;

    public Dictionary<int, AchievementInstance> achievementInstances;

    public Dictionary<string, AchievementCategory> OriginAchievementDataDict;


    private void Start()
    {
    }

    public void ResetAchievements()
    {
        Init();
        
        Core.UGSManager.Data.CallSave();
    }

    public void Init()
    {
        achievementInstances = new();
        OriginAchievementDataDict = new Dictionary<string, AchievementCategory>();
        
        var actionLength = Enum.GetValues(typeof(ExternalEnums.AchActionType)).Length;
        var targetLength = Enum.GetValues(typeof(ExternalEnums.AchTargetType)).Length;

        var container = Core.DataManager.AchTable.DataBaseList;

        achievementDataDict = new Dictionary<string, AchievementCategory>();

        foreach (var info in container)
        {
            if (!achievementDataDict.TryGetValue(info.CategoryTitle, out var category))
            {
                category = new AchievementCategory(actionLength, targetLength);
                achievementDataDict[info.CategoryTitle] = category;
            }
            
            var instance = new AchievementInstance(info);
            
            achievementInstances.Add(info.Key, instance);
            
            category.GetOrAddInstList(info.AchAction, info.AchTarget, info.ID).Add(instance);
        }

        AchievementCount = container.Count;

        foreach (var pair in achievementDataDict)
        {
            OriginAchievementDataDict[pair.Key] = new AchievementCategory(pair.Value);
        }

        SubEvent();
        Core.EventManager.OnClearEvent -= SubEvent;
        Core.EventManager.OnClearEvent += SubEvent;
    }

    public void SubEvent()
    {
        Core.EventManager.Unsubscribe<AchievementEvent>(OnTriggerAchievement);
        Core.EventManager.Subscribe<AchievementEvent>(OnTriggerAchievement);
    }
    

    private void OnTriggerAchievement(AchievementEvent data)
    {
        foreach (var categoryPair in achievementDataDict)
        {
            var categoryArray = categoryPair.Value.AchievementMatrix;
            var actionIndex = (int)data.AchAction;
            var targetIndex = (int)data.AchTarget;

            if (categoryArray[actionIndex, targetIndex] == null)
                continue;

            var removes = new List<AchievementInstance>();
            
            if (categoryArray[actionIndex, targetIndex]
                    .TryGetValue(DefaultKey, out List<AchievementInstance> defaultValues) && data.ID != DefaultKey)
            {
                foreach (var instance in defaultValues)
                {
                    if (instance.IncrementProgress(data.Amount))
                        removes.Add(instance);
                }
                
                foreach (var target in removes)
                    defaultValues.Remove(target);
                
                removes.Clear();
            }
            
            if (!categoryArray[actionIndex, targetIndex].TryGetValue(data.ID, out List<AchievementInstance> achList))
                continue;
            
            foreach (var t in achList)
            {
                if(t.IncrementProgress(data.Amount))
                    removes.Add(t);
            }
            
            foreach (var target in removes)
                achList.Remove(target);
            
            removes.Clear();
        }
    }

    public Dictionary<string, List<AchievementInstance>> GetProcessedAchievements()
    {
        var processedAchievements = new Dictionary<string, List<AchievementInstance>>();

        foreach (var categoryPair in OriginAchievementDataDict)
        {
            var categoryName = categoryPair.Key;
            var categoryArray = categoryPair.Value.AchievementMatrix;

            if (!processedAchievements.ContainsKey(categoryName))
                processedAchievements[categoryName] = new List<AchievementInstance>();

            foreach (var dict in categoryArray)
            {
                if (dict == null)
                    continue;

                foreach (var list in dict.Values)
                    processedAchievements[categoryName].AddRange(list);
            }
        }
        
        return processedAchievements;
    }
}