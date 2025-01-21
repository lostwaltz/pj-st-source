using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase
{
    public Action OnEnterEvent;
    
    public abstract void OnEnter(); 

    public abstract void OnExit();

    public virtual IEnumerator OnLoadAssets()
    {
        yield return null;
    } 
}
