using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tutorial
{
    protected UIMessageDialog UIDialog => DialogManager.Instance.GetCurDialog<UIMessageDialog>();
    protected TutorialManager Manager => TutorialManager.Instance;

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Execute();
}
