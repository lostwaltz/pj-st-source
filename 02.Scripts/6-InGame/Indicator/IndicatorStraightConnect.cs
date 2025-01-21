using System;
using UnityEngine;

public enum IndicatorStraightOption
{
    Normal = 0, // white
    Red,
    Yellow,
}

[Serializable]
public struct IndicatorStraightSettings
{
    public float width;
    public Material material;
}

public class IndicatorStraightConnect : IndicatorConnect
{
    [SerializeField] IndicatorStraightSettings[] settings;
    
    public override void Show(Vector2 start, Vector2 end)
    {
        line.positionCount = 2;
        line.SetPosition(0, StageManager.Instance.cellMaps[start].transform.position + Vector3.up * yOffset);
        line.SetPosition(1, StageManager.Instance.cellMaps[end].transform.position + Vector3.up * yOffset);

        Show(start);
    }

    public void Show(Vector2 start, Vector2 end, IndicatorStraightOption option)
    {
        float width = settings[(int)option].width;
        line.material = settings[(int)option].material;
        
        line.startWidth = width;
        line.endWidth = width;
        
        Show(start, end);
    } 
}
