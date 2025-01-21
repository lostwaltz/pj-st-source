using System;
using System.Collections.Generic;
using UnityEngine;

public class UIGuide : UIBase
{
    private UIGuideMessage msg;
    [SerializeField] GuideContent[] guideContents;
    Dictionary<GuideTrigger, GuideContent> contentMap = new Dictionary<GuideTrigger, GuideContent>();
    Dictionary<GuideTrigger, bool> checkList = new Dictionary<GuideTrigger, bool>();
    
    private void Awake()
    {
        Initialize();
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }

    void Initialize()
    {
        msg = Core.UIManager.GetUI<UIGuideMessage>();
        msg.transform.SetParent(transform);
        
        Vector3 pos = msg.transform.localPosition;
        pos.y = 0f;
        msg.transform.localPosition = pos;
        
        msg.Initialize(this);
        
        msg.Close();
        
        for (int i = 0; i < guideContents.Length; i++)
        {
            if (guideContents[i].trigger.Equals(GuideTrigger.Manual))
                continue;
            else if (contentMap.ContainsKey(guideContents[i].trigger))
                continue;
            
            contentMap.Add(guideContents[i].trigger, guideContents[i]);
            checkList.Add(guideContents[i].trigger, false);
        }

        SubscribeEvent();
    }

    void SubscribeEvent()
    {
        Core.EventManager.Subscribe<GuidePopUpEvent>(OpenMessage);
    }

    void UnsubscribeEvent()
    {
        Core.EventManager.Unsubscribe<GuidePopUpEvent>(OpenMessage);
    }

    void OpenMessage(GuidePopUpEvent popUpEvent)
    {
        if(checkList[popUpEvent.Trigger])
            return;
        
        checkList[popUpEvent.Trigger] = true;
        
        msg.SetContent(contentMap[popUpEvent.Trigger]);
        msg.Open();
    }

    public GuideContent GetContent(int id)
    {
        return guideContents[id];
    }
}

public enum GuideTrigger
{
    Manual = 0,
    OnStart,
    // OnUnitPlace,
    OnPlayerTurnStart,
    // OnPlayerSelected,
}

[Serializable]
public struct GuideContent
{
    public string title;
    public GuideTrigger trigger;
    public Vector2 size;
    public int nextContent;
    [TextArea] public string[] desc;
}