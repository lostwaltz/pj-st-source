using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorSelectedCell : IndicatorCell
{
    private Vector2 currentCoord;

    public override void Show(Vector2 coord)
    {
        currentCoord = coord;
        base.Show(coord);
    }

    public bool IsAtCoord(Vector2 coord)
    {
        return currentCoord == coord && gameObject.activeInHierarchy;
    }
}
