using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UITest : UIPopup
{
    public SoundData SoundData;
    public SoundBuilder SoundBuilder;

    private void Awake()
    {
        SoundBuilder = Core.SoundManager.CreateSoundBuilder();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        
        
        Close();
        //Core.SceneLoadManager.LoadScene("GameScene");
        SoundBuilder.Play(SoundData);
    }
}
