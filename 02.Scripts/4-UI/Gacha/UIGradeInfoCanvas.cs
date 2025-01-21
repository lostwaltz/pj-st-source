using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGradeInfoCanvas : UIBase
{
    [SerializeField] private Button exitBtn;

    public void Awake()
    {
        exitBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.AddListener(OnclickExit);
    }

    public void OpenGradeInfo()
    {
        
    }

    public void OnclickExit()
    {
        UISound.PlayBackButtonClick();
        UIBattle.PlayUIBattleCharacterFilteringWindowClose();
        gameObject.SetActive(false);
    }
}
