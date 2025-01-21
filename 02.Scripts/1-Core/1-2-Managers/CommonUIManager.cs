using UnityEngine;

public class CommonUIManager : UIManager
{
    public override void Init()
    {
        var canvas = OpenUI<UICommonCanvas>();
        
        var popUp = GetUI<UIInformPopup>();
        var loading = GetUI<UIUGSLoading>();
        
        InitTransform(popUp.transform as RectTransform, canvas.transform);
        InitTransform(loading.transform as RectTransform, canvas.transform);
        
        popUp.gameObject.SetActive(false);
        loading.gameObject.SetActive(false);
    }

    void InitTransform(RectTransform rect, Transform parent)
    {
        rect.SetParent(parent);
        rect.anchoredPosition = Vector2.zero;
    }
}