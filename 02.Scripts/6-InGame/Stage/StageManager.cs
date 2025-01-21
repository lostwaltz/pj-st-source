using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageManager : Singleton<StageManager>
{
    // 서브 컴포넌트
    public static StageInteraction Interaction { get; private set; }
    public static StagePathFinding PathFinding { get; private set; }

    // 인스펙터 할당 중
    [SerializeField] public StageSO stageData;
    [SerializeField] ObstacleListSO obstacleList;
    [SerializeField] StageCell cellPrefab;

    [Header("Instances")]
    public List<StageCell> cells = new List<StageCell>();
    public Dictionary<Vector2, StageCell> cellMaps = new Dictionary<Vector2, StageCell>();

    public List<StageObstacle> obstacles = new List<StageObstacle>();
    public Dictionary<Vector2, StageObstacle> obstaclesMaps = new Dictionary<Vector2, StageObstacle>();

    public Dictionary<Vector2, List<CoverData>> coverMaps = new Dictionary<Vector2, List<CoverData>>();



    [Header("Cell Related")]
    [SerializeField] Material cellMaterial;
    [SerializeField][ColorUsage(true, true)] Color[] cellColor;


    private void Awake()
    {
        Interaction = new StageInteraction(this);
        PathFinding = new StagePathFinding(this);
    }

    public void Initialize()
    {
        if (Core.DataManager != null &&
            Core.DataManager.SelectedStage != null)
        {
            stageData = Core.DataManager.SelectedStage;
            for (int i = 0; i < stageData.playerPlacements.Count; i++)
            {
                var placement = stageData.playerPlacements[i];
                placement.isPlaced = false;
                stageData.playerPlacements[i] = placement;
            }
        }

        cells.Clear();
        cellMaps.Clear();

        obstacles.Clear();
        obstaclesMaps.Clear();
        
        coverMaps.Clear();

        SetStage();
        SetObstable();
        SetCoverData();
        ChangeCellColor(0);
    }


    #region *** Stage Setting ***

    void SetStage()
    {
        ClearStage();

        foreach (var cellPlacement in stageData.cellPlacements)
        {
            StageCell stageCell = Instantiate(cellPrefab, transform);
            stageCell.Initialize(cellPlacement);

            cells.Add(stageCell);
            cellMaps.Add(cellPlacement.coord, stageCell);
        }
    }

    void ClearStage()
    {
        GameObject obj = new GameObject("Destroy");
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i] == null) continue;
            cells[i].transform.SetParent(obj.transform);
        }

#if UNITY_EDITOR
        DestroyImmediate(obj);
#else
        Destroy(obj);
#endif
        cells.Clear();
        cellMaps.Clear();
    }

    private void SetObstable()
    {
        // Debug.LogError($"Set Obstable");
        ClearObstable();

        for (int i = 0; i < stageData.obstaclePlacements.Length; i++)
        {
            Placement obstaclePlacement = stageData.obstaclePlacements[i];
            PlacementArea area = stageData.obstacleAreas[i];

            StageObstacle obstacle = Instantiate(obstacleList.ObstaclesPrefabs[obstaclePlacement.id], transform);
            obstacle.Initialize(obstaclePlacement, area.areaCoord);

            obstacles.Add(obstacle);

            for (int j = 0; j < area.areaCoord.Count; j++)
                obstaclesMaps.Add(area.areaCoord[j], obstacle); 
        }
    }

    public void ClearObstable()
    {
        GameObject obj = new GameObject("Destroy");
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (obstacles[i] == null) continue;
            obstacles[i].transform.SetParent(obj.transform);
        }

#if UNITY_EDITOR
        DestroyImmediate(obj);
#else
        Destroy(obj);
#endif
        obstacles.Clear();
        obstaclesMaps.Clear();
    }

    private void SetCoverData()
    {
        // Debug.LogError($"Set Cover Data");
        // 좌표에 따른 은,엄폐 지점 캐싱
        // 비용이 너무 비싸서 스테이지 세팅 시, 최초 한 번만 계산하고 캐싱하는 방식으로 처리
        int coverIndex = 0;
        foreach (Vector2 coord in cellMaps.Keys)
        {
            Interaction.GetNearCoord(coord, out List<Vector2> neighbors);
            
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (obstaclesMaps.TryGetValue(neighbors[i], out StageObstacle obs) && obs != null)
                {
                    // 검사 중인 셀의 위치
                    Vector3 cellPos = cellMaps[coord].transform.position;
                    
                    // 여러 은폐점 중 1곳만 채택
                    Transform closestCoverPoint = obs.coverPoints[0];
                    float leastDistance = float.MaxValue;
                    for (int j = 0; j < obs.coverPoints.Length; j++)
                    {
                        Vector3 sub = cellPos - obs.coverPoints[j].position;
                        if (sub.sqrMagnitude < leastDistance)
                        {
                            leastDistance = sub.sqrMagnitude;
                            closestCoverPoint = obs.coverPoints[j];
                        }
                    }

                    // 인접 지점의 조건 : sqrMagnitude 거리가 1 이하
                    if (leastDistance > 1f)
                        continue;
                    
                    CoverData newData = new CoverData()
                    {
                        index = coverIndex++,
                        position = Vector3.Lerp(closestCoverPoint.position, cellPos, 0.05f) + Vector3.up * 0.5f,
                        direction = (cellPos - closestCoverPoint.position).normalized
                    };

                    if (!coverMaps.ContainsKey(coord))
                        coverMaps.Add(coord, new List<CoverData>());

                    coverMaps[coord].Add(newData);

                }
            }
        }

    }

    #endregion

    public HashSet<Vector2> ShowStageCells(int colorOpt, Vector2 coord, int distance)
    {
        Interaction.HideActiveCells();

        ResetCellColors();

        ChangeCellColor(colorOpt);

        return Interaction.ShowMovableCells(coord, distance);
    }

    private void ResetCellColors()
    {
        cellMaterial.SetColor("_Color", cellColor[0]);
    }

    public void HideStageCells()
    {
        Interaction.HideActiveCells();
    }

    public void ChangeCellColor(int i)
    {
        i = Math.Max(i, 0);
        i = Math.Min(i, cellColor.Length - 1);
        cellMaterial.SetColor("_Color", cellColor[i]);
    }

    public static bool IsValidCell(Vector2 newCoord)
    {
        throw new NotImplementedException();
    }
}

public struct CoverData
{
    public int index;
    public Vector3 position;
    public Vector3 direction;
}
