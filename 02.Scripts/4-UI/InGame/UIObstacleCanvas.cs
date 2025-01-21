using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EnumTypes;
using TMPro;
using UnityEngine;

public class UIObstacleCanvas : UIBase
{
    [SerializeField] private TMP_Text obstacleNameTxt;
    [SerializeField] private TMP_Text obstacleDescriptionTxt;
    [SerializeField] private TMP_Text obstacleTypeTxt;

    public void Init()
    {
        GameManager.Instance.Interaction.OnClicked += SetObstacleInfo;
    }

    void SetObstacleInfo(IClickable thing)
    {
        //TODO : 스킬선택 중-> 상태일때는 클릭이 안들어와야함??
        
        if (thing is not StageObstacle)
        {
            CloseObstacleUI();
            return;
        }
        
        gameObject.SetActive(true);
        ObstacleSO selectedobstacleSO = (thing as StageObstacle).obstacleData;
        obstacleNameTxt.text = selectedobstacleSO.obstacleName;
        obstacleDescriptionTxt.text = selectedobstacleSO.description;
        obstacleTypeTxt.text = selectedobstacleSO.GetObstacleType(selectedobstacleSO.obstacleType);
    }

    void CloseObstacleUI()
    {
        Close();
    }
}
