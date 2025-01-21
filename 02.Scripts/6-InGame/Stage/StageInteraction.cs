using System;
using System.Collections.Generic;
using UnityEngine;

public class StageInteraction
{
    StageManager Manager { get; set; }

    // 상하좌우 이동
    private int[,] fourDirection => Define.FourDirection;

    Dictionary<Vector2, StageCell> maps => Manager.cellMaps;
    public HashSet<Vector2> activeCells = new HashSet<Vector2>();

    public Func<int, int, int> AddCostForMove => (curCost, newCost) => newCost > 0 ? curCost + newCost : -1;
    public Func<int, int, int> AddCostForSkill => (curCost, newCost) => newCost > 0 ? curCost + 1 : -1;
    public Func<int, int, int> AddCostNaively => (curCost, newCost) => curCost + 1;

    public Predicate<Vector2> FilterForMove => coord => maps[coord].Cost < 0 || maps[coord].unitIndexInCell >= 0;
    public Predicate<Vector2> FilterForSkill => coord => maps[coord].Cost < 0;


    public StageInteraction(StageManager manager)
    {
        Manager = manager;
    }


    public HashSet<Vector2> ShowMovableCells(Vector2 startCoord, int maxDistance)
    {
        ShowCells(startCoord, maxDistance, AddCostForMove, FilterForMove);
        return activeCells;
    }

    public HashSet<Vector2> ShowSkillRangeCells(Vector2 startCoord, int maxDistance)
    {
        ShowCells(startCoord, maxDistance, AddCostForSkill, FilterForSkill);
        return activeCells;
    }


    public void ShowCells(Vector2 startCoord, int maxDistance, Func<int, int, int> costLogic, Predicate<Vector2> filter)
    {
        activeCells.Clear();
        FloodFill(startCoord, maxDistance, costLogic, (coord) =>
        {
            activeCells.Add(coord);
            maps[coord].gameObject.SetActive(true);
        });

        FilterCells(filter);
    }

    void FilterCells(Predicate<Vector2> filterOper)
    {
        HashSet<Vector2> filtered = new HashSet<Vector2>();
        foreach (Vector2 coord in activeCells)
        {
            if (filterOper.Invoke(coord))
                maps[coord].gameObject.SetActive(false);
            else
                filtered.Add(coord);
        }

        activeCells = filtered;
    }

    // bfs 방식으로 변경
    public void FloodFill(Vector2 coord, int maxDepth, Func<int, int, int> calcCostOper, Action<Vector2> onAdded = null)
    {
        Queue<Vector2> queue = new Queue<Vector2>();
        Dictionary<Vector2, int> visited = new Dictionary<Vector2, int>();
        Action<Vector2, int> addAction = (point, depth) =>
        {
            visited.Add(point, depth);
            queue.Enqueue(point);
            onAdded?.Invoke(point);
        };

        addAction.Invoke(coord, 0);

        while (queue.Count > 0)
        {
            Vector2 curCoord = queue.Dequeue();

            for (int i = 0; i < fourDirection.GetLength(1); i++)
            {
                Vector2 newCoord = new Vector2(curCoord.x + fourDirection[0, i], curCoord.y + fourDirection[1, i]);
                if (!maps.ContainsKey(newCoord))
                    continue;

                // 비용 적용
                int depth = calcCostOper.Invoke(visited[curCoord], maps[newCoord].Cost);
                if (depth <= -1 || depth >= maxDepth + 1)
                    continue;

                if (!visited.ContainsKey(newCoord)) // 새 좌표 추가
                    addAction.Invoke(newCoord, depth);
                else if (visited[newCoord] > depth) // 기존보다 나은 경로로 갱신
                    visited[newCoord] = depth;
            }
        }
    }

    public void HideActiveCells()
    {
        foreach (Vector2 coord in maps.Keys)
            maps[coord].gameObject.SetActive(false);
    }

    public bool IsActiveCell(Vector2 coord)
    {
        return activeCells.Contains(coord);
    }

    public void GetNearCoord(Vector2 point, out List<Vector2> nearCoords, int scope = 1)
    {
        List<Vector2> list = new List<Vector2>();
        FloodFill(point, scope, AddCostNaively, (coord) => { list.Add(coord); });
        nearCoords = list;
    }

    public List<Vector2> GetFrontCoord(Vector2 point, int horizontalRangeCoords, int verticalRangeCoords, Vector2 direction)
    {
        List<Vector2> range = new List<Vector2>();

        if (horizontalRangeCoords == 3)
        {
            for (int i = -1; i < horizontalRangeCoords - 1; i++)
            {
                for (int j = 0; j < verticalRangeCoords; j++)
                {
                    Vector2 offset = new Vector2(i * Mathf.Abs(direction.y), i * Mathf.Abs(direction.x));
                    Vector2 localPoint = point + offset;

                    Vector2 newCoord = localPoint + direction * j;
                    range.Add(newCoord);
                }
            }
        }
        else
        {
            for (int i = 0; i < horizontalRangeCoords; i++)
            {
                for (int j = 0; j < verticalRangeCoords; j++)
                {
                    Vector2 offset = new Vector2(i * Mathf.Abs(direction.y), i * Mathf.Abs(direction.x));
                    Vector2 localPoint = point + offset;

                    Vector2 newCoord = localPoint + direction * j;
                    range.Add(newCoord);
                }
            }
        }

        return range;
    }
}
