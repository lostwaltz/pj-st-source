using UnityEngine;

public abstract class IndicatorConnect : IndicatorComponent
{
    [SerializeField] protected LineRenderer line;
    [SerializeField] protected float yOffset;

    public abstract void Show(Vector2 start, Vector2 end);
}