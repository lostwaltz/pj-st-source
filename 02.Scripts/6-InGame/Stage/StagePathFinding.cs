using System.Collections.Generic;
using UnityEngine;

public class StagePathFinding
{
    StageManager Manager { get; set; }
    Dictionary<Vector2, StageCell> maps => Manager.cellMaps;
    
    // 상하좌우 + 대각선 이동
    private int[,] eightDirection = Define.EightDirection;

    public StagePathFinding(StageManager manager)
    {
        Manager = manager;
    }
    
    // 경로 찾기 A star
    public void GetPath(StageCell start, StageCell end, out List<StageCell> path)
    {
        path = new List<StageCell>();

        List<CellNode> openList = new List<CellNode>();
        Dictionary<Vector2, CellNode> checkList = new Dictionary<Vector2, CellNode>();

        float h = GetHeuristic(start.placement.coord, end.placement.coord);
        CellNode startNode = new CellNode(start.placement.coord, null, 0f, h);
        CellNode endNode = new CellNode(end.placement.coord, null, 0f, h);
        CellNode closestNode = startNode;

        openList.Add(startNode);
        checkList.Add(startNode.coord, startNode);

        while (openList.Count > 0)
        {
            CheckNearNode(openList[0], endNode, ref openList, ref checkList);

            openList[0].isClosed = true;
            openList.RemoveAt(0);

            foreach(CellNode node in openList)
            {
                if (node.h < closestNode.h) 
                    closestNode = node;

                if(node.h == 0)
                {
                    endNode = node;
                    openList.Clear();
                    break;
                }
            }
        }

        CellNode p = closestNode;

        while(p.parent != null)
        {
            path.Add(maps[p.coord]);
            p = p.parent;
        }

        path.Reverse();
    }

    void CheckNearNode(CellNode parent, CellNode end, ref List<CellNode> openList, ref Dictionary<Vector2, CellNode> checkList)
    {
        Vector2 coord = parent.coord;

        for(int i = 0; i < eightDirection.GetLength(1); i++)
        {
            Vector2 searchCoord = new Vector2(coord.x + eightDirection[0, i], coord.y + eightDirection[1, i]);

            // 범위 내
            if (maps.TryGetValue(searchCoord, out StageCell cell) && cell != null)
            {
                // 갈 수 있는 칸인지 확인
                if (cell.Cost < 0 || cell.unitIndexInCell >= 0) 
                    continue;
                
                if (!checkList.TryGetValue(searchCoord, out CellNode searchingCell)) // 없던 경우
                {
                    // 코스트 처리
                    // root 2
                    float g = parent.g + cell.Cost;
                    float h = GetHeuristic(searchCoord, end.coord);
                    
                    searchingCell = new CellNode(searchCoord, parent, g, h);

                    openList.Add(searchingCell);
                    checkList.Add(searchCoord, searchingCell);
                }
                else if (!searchingCell.isClosed) // 있었는데 닫히지 않은 경우
                {
                    if (searchingCell.g + cell.Cost < searchingCell.g) // 더 나은 경로 찾기
                    {
                        searchingCell.parent = searchingCell;
                        searchingCell.g = searchingCell.g + cell.Cost;
                        searchingCell.CalcCost();
                    }
                }
            }
        }
    }

    float GetHeuristic(Vector2 from, Vector2 to)
    {
        return Mathf.Max(Mathf.Abs(to.x - from.x), Mathf.Abs(to.y - from.y)); // 체스판 거리 방식
        // return  Mathf.Abs(to.x - from.x) + Mathf.Abs(to.y - from.y); // 맨해튼 거리 방식
    }
}


public class CellNode
{
    public CellNode parent;
    public Vector2 coord;
    public float g;
    public float h;
    public float cost;

    public bool isClosed;

    public CellNode(Vector2 newCoord, CellNode newParent, float newG, float newH)
    {
        coord = newCoord;
        parent = newParent;
        g = newG;
        h = newH;
        isClosed = false;

        CalcCost();
    }

    public void CalcCost()
    {
        cost = g + h;
    }
}