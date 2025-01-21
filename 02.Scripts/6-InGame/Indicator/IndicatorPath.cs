using UnityEngine;

public class IndicatorPath : IndicatorConnect
{
    public override void Show(Vector2 start, Vector2 end)
    {
        StageCell startCell = StageManager.Instance.cellMaps[start];
        StageCell endCell = StageManager.Instance.cellMaps[end];
        StageManager.PathFinding.GetPath(startCell, endCell, out var path);
        path.Insert(0, startCell);
        
        line.positionCount = path.Count;
        Vector3[] points = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++)
        {
            points[i] = path[i].transform.position;
            points[i].y += yOffset;
        }
        
        line.SetPositions(points);

        Show(start);
    }
}
