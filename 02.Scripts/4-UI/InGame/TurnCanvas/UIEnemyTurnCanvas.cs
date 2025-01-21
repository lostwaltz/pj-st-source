using EnumTypes;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIEnemyTurnCanvas : UIBase
{
    [SerializeField] private TMP_Text numberTxt;
    [SerializeField] private Image image;
    
    public void Init()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.SubscribePhaseEvent(GamePhase.EnemyTurn,SetTurnNum); 
    }
    void SetTurnNum()
    {
        UIBattle.PlayUIBattleEnemyTurn();
        gameObject.SetActive(true);
        int turnCount = GameManager.Instance.PhaseMachine.TurnCount;
        int enemyTurnCount = turnCount / 2 + turnCount % 2;
        numberTxt.text = $"{enemyTurnCount}";

        image.DOFade(0.8f, 0.3f).ChangeStartValue(new Color(1.0f, 1.0f, 1.0f, 0.0f)).SetEase(Ease.Linear);
        StartCoroutine(CorEnemyTurnCanvas(1f));
    }
    IEnumerator CorEnemyTurnCanvas(float delay)
    {
        yield return new WaitForSeconds(delay);
        Core.UIManager.CloseUI<UIEnemyTurnCanvas>();
        Core.UIManager.OpenUI<UIBattleCanvas>();
        
        Core.EventManager.Publish(new ShowEndTurnCanvasUIEvent(UnitType.EnemyUnit));
    }
}
