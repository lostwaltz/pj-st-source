using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AchievementInstance
{
    public readonly AchievementInfo OriginData;
    
    public float CurProgress = 0;
    public readonly float MaxProgress = 0;
    public bool IsComplete = false;
    public bool IsTakeReward = false;

    private readonly AchievementChecker checker;

    public AchievementInstance(AchievementInfo data)
    {
        OriginData = data;
        MaxProgress = data.TargetValue;
        CurProgress = 0;
        
        var isPathEmpty = data.ConditionPath.Equals("...") || 
                                data.ConditionPath.Equals(string.Empty);
        
        checker = Resources.Load<AchievementChecker>(isPathEmpty
            ? "Prefabs/AchChecker/default_checker"
            : Utils.Str.Clear().Append("Prefabs/AchChecker/").Append(data.ConditionPath).ToString());
    }

    public bool IncrementProgress(float amount)
    {
        if (IsComplete) return IsComplete;
        
        CurProgress = checker.CheckIncrement(CurProgress, amount);
        
        var isComplete = CurProgress >= MaxProgress;

        if (isComplete)
        {
            IsComplete = true;
            CurProgress = MaxProgress;
        }
        
        Debug.Log(CurProgress + " / " + MaxProgress +  " - " + OriginData.Title);
        
        return IsComplete;
    }
}
