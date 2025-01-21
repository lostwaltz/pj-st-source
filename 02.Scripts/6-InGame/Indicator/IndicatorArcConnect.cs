using UnityEngine;

public class IndicatorArcConnect : IndicatorConnect
{
    [SerializeField] float pointPerDistance = 10f;
    [SerializeField] int minPoint = 5;
    
    public override void Show(Vector2 start, Vector2 end)
    {
        Vector3 startPoint = StageManager.Instance.cellMaps[start].transform.position;
        Vector3 endPoint = StageManager.Instance.cellMaps[end].transform.position;
        
        float dist = (endPoint - startPoint).sqrMagnitude;
        int count = (int)Mathf.Max(dist / pointPerDistance, minPoint);
        
        line.positionCount = count + 1;
        for (int i = 0; i <= count; i++)
        {
            Vector3 pos = Vector3.Lerp(startPoint, endPoint, (float)i / count);
            pos.y = Mathf.Sin((float)i / count * Mathf.PI) + yOffset;
            line.SetPosition(i, pos);
        }

        Show(start);
    }
}