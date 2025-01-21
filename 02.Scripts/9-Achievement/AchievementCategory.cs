using System.Collections.Generic;

public class AchievementCategory
{
    public AchievementCategory() { }

    public AchievementCategory(AchievementCategory category)
    {
        int actionLength = category.AchievementMatrix.GetLength(0);
        int targetLength = category.AchievementMatrix.GetLength(1);
        AchievementMatrix = new Dictionary<int, List<AchievementInstance>>[actionLength, targetLength];

        for (int i = 0; i < actionLength; i++)
        {
            for (int j = 0; j < targetLength; j++)
            {
                if (category.AchievementMatrix[i, j] != null)
                {
                    AchievementMatrix[i, j] = new Dictionary<int, List<AchievementInstance>>();
                    foreach (var pair in category.AchievementMatrix[i, j])
                    {
                        AchievementMatrix[i, j][pair.Key] = new List<AchievementInstance>();
                        foreach (var instance in pair.Value)
                            AchievementMatrix[i, j][pair.Key].Add(instance);
                    }
                }
            }
        }
    }
    
    public Dictionary<int, List<AchievementInstance>>[,] AchievementMatrix { get; }

    public AchievementCategory(int actionLength, int targetLength)
    {
        AchievementMatrix = new Dictionary<int, List<AchievementInstance>>[actionLength, targetLength];
    }

    public List<AchievementInstance> GetOrAddInstList(
        ExternalEnums.AchActionType action,
        ExternalEnums.AchTargetType target,
        int key)
    {
        var actionIndex = (int)action;
        var targetIndex = (int)target;

        AchievementMatrix[actionIndex, targetIndex] ??= new Dictionary<int, List<AchievementInstance>>();

        var dict = AchievementMatrix[actionIndex, targetIndex];

        if (!dict.TryGetValue(key, out var achievementDataList))
            dict[key] = achievementDataList = new List<AchievementInstance>();

        return achievementDataList;
    }
}