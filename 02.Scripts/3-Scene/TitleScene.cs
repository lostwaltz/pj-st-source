using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : SceneBase
{
    public override void OnEnter()
    {
        BGM.PlayTitleBGM();
    }

    public override void OnExit()
    {

    }
}
