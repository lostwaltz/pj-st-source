using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopup : UIBase
{
    [SerializeField] private Transform animationRoot;
    [SerializeField] private BehaviorFactory showBehaviorFactory;
    [SerializeField] private BehaviorFactory hideBehaviorFactory;
    
    private IBehaviorExecutable showEffects;
    private IBehaviorExecutable hideEffects;
    
    protected float Duration = 0.3f; 

    public override void Open()
    {
        gameObject.SetActive(true);

        if (animationRoot is null || showBehaviorFactory is null)
        {
            OpenProcedure();
            return;
        }

        showEffects ??= showBehaviorFactory.CreateBehaviorEffects();
        
        showEffects.BehaviorExecute(animationRoot, Duration).OnComplete(OpenProcedure);
    }

    public override void Close()
    {
        if (animationRoot is null || hideBehaviorFactory is null)
        {
            CloseDone();
            return;
        }

        hideEffects ??= hideBehaviorFactory.CreateBehaviorEffects();
        hideEffects.BehaviorExecute(animationRoot, Duration).OnComplete(CloseDone);
    }

    private void CloseDone()
    {
        gameObject.SetActive(false);
        CloseProcedure();
    }
}
