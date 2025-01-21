using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIEventHandler))]
public class UIToolTip : UIBase
{
    private UIToolTipMessage msg;
    Vector2 originalSize = new (1920, 1080);
    [SerializeField] Vector3 position;
    [TextArea] public string toolTips;
    
    private void Awake()
    {
        UIEventHandler handler = GetComponent<UIEventHandler>();
        handler.OnEnterEvent += Show;
        handler.OnExitEvent += Hide;
        
        AdjustScreenRatio();
    }

    public void AdjustScreenRatio()
    {
        Vector2 ratio = new (Screen.width / originalSize.x, Screen.height / originalSize.y);
        position.x *= ratio.x;
        position.y *= ratio.y;
    }
    
    public void Show(PointerEventData evt)
    {
        if (msg == null)
        {
            msg = Core.UIManager.GetUI<UIToolTipMessage>();
            msg.transform.parent = transform.parent;
            msg.transform.localScale = Vector3.one;;
        }
        
        msg.SetMessage(toolTips, transform.position + position);
    }

    public void Hide(PointerEventData evt)
    {
        msg.Close();
    }
}
