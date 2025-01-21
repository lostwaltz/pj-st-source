using UnityEngine;

public class UIUGSLoading : UIBase
{
    [SerializeField] Vector3 position;
    
    private void OnEnable()
    {
        (transform as RectTransform).anchoredPosition = position;
    }
}
