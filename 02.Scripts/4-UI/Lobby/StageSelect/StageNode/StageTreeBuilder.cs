using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class StageData
{
    public int StageKey;
    public int DependencyKey;
}
public class StageNode
{
    public StageSO StageSO;
    public StageData Data;
    public readonly List<StageNode> Children = new();
    public Vector2 Position;
    public int Depth;

    public StageNode(StageData data, StageSO stageSo)
    {
        Data = data;
        StageSO = stageSo;
    }
}

public class StageTreeBuilder
{
    private readonly Dictionary<int, StageNode> stageNodeDict = new Dictionary<int, StageNode>();

    public List<StageNode> BuildTree(List<StageSO> stageDataList)
    {
        List<StageNode> roots = new();
        
        foreach (var stage in stageDataList)
        {
            if (!stageNodeDict.ContainsKey(stage.stageData.StageKey))
                stageNodeDict[stage.stageData.StageKey] = new StageNode(stage.stageData, stage);
        }

        foreach (var stage in stageDataList)
        {
            if (stage.stageData.DependencyKey == stage.stageData.StageKey)
                roots.Add(stageNodeDict[stage.stageData.StageKey]);
            else
            {
                if (!stageNodeDict.TryGetValue(stage.stageData.DependencyKey, out var value))
                    continue;
                
                value.Children.Add(stageNodeDict[stage.stageData.StageKey]);
            }
        }

        return roots;
    }
}