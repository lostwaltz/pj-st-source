using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Stage SO List", menuName = "StageSO/New Stage SO List")]
public class StageSOList : ScriptableObject
{
    public StageSO[] stages;

    public void GetStageData(out List<StageData> stageDataList)
    {
        stageDataList = new List<StageData>();

        for (int i = 0; i < stages.Length; i++)
            stageDataList.Add(stages[i].stageData);
    }
}