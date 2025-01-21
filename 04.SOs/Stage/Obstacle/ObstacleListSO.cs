using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ObstacleList", menuName = "StageSO/New ObstacleList")]
public class ObstacleListSO : ScriptableObject
{
    public List<StageObstacle> ObstaclesPrefabs;

    // 에디터용 메서드
    public void SetPrefabs()
    {
        ObstaclesPrefabs.Clear();
        
        StageObstacle[] prefabs = Resources.LoadAll<StageObstacle>(Constants.Path.Obstacles);
        for (int i = 0; i < prefabs.Length; i++)
        {
            int id = i;
            prefabs[i].placement.id = id;
            ObstaclesPrefabs.Add(prefabs[i]);
        }
    }
}
