using UnityEngine;

public class IndicatorComponent : MonoBehaviour
{
    public Vector2 Coord { get; protected set; }

    public virtual void Show(Vector2 coord)
    {
        Coord = coord;
        transform.position = StageManager.Instance.cellMaps[coord].transform.position;
        gameObject.SetActive(true);
    }
    
    public virtual void Show(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}