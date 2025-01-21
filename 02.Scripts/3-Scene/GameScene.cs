using DG.Tweening;
using UnityEngine;

public class GameScene : SceneBase
{
    public override void OnEnter()
    {
        GameManager gameManager = Resources.Load<GameManager>(Constants.Path.GameManager);
        GameManager inst = GameObject.Instantiate(gameManager);
        inst.Initialize();

        if (0 != StageManager.Instance.stageData.startDialogueKey)
            DialogManager.Instance.ShowDialog<UINovelDialog>(StageManager.Instance.stageData.startDialogueKey);
            
        Core.UIManager.OpenUI<UICombatSelectUnit>();
        
        OnEnterEvent?.Invoke();
        
        OnEnterEvent = null;    
    }

    public override void OnExit()
    {
        Core.UGSManager.Data.CallSave();
        DOTween.KillAll();
    }
}