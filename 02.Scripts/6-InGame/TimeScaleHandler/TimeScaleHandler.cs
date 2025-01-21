using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

public class TimeScaleHandler
{
    private float scaleValue;
    
    public void SetScale(float scale)
    {
        if (GameManager.Instance.CommandController.CommandCoroutineHandle == null)
            Time.timeScale = scale;
        else
        {
            scaleValue = scale;
            GameManager.Instance.CommandController.OnPostProcessed -= TimeHandle;
            GameManager.Instance.CommandController.OnPostProcessed += TimeHandle;
        }
    }

    private void TimeHandle(GamePhase gamePhase)
    {
        GameManager.Instance.CommandController.OnPostProcessed -= TimeHandle;
        Time.timeScale = scaleValue;
    }
}

