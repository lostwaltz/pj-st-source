using UnityEngine;

public class IndicatorIcon : IndicatorComponent
{
    [SerializeField] private Vector3 directionOffset;
    
    public void Show(Vector3 position, Vector3 direction)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction + directionOffset);
    }
}
