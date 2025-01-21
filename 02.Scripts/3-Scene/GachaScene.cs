using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GachaScene : SceneBase
{
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
        Core.UGSManager.Data.CallSave();
    }
}
