using System.Collections;
using DG.Tweening;
using EnumTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerTurnCanvas : UIBase
{
    [SerializeField] private TMP_Text numberTxt;
    [SerializeField] private Image image;
    
    void Start()
    {
        UIBattle.PlayUIBattlePlayerTurn();
    }
    public void Init()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.SubscribePhaseEvent(GamePhase.PlayerTurn,SetTurnNum); 
        
        StartCoroutine(CorPlayerTurnCanvas(1f));
    }
    public void SetTurnNum()
    {
        UIBattle.PlayUIBattlePlayerTurn();
        gameObject.SetActive(true);

        
        int turnCount = GameManager.Instance.PhaseMachine.TurnCount;
        int playerTurnCount = turnCount / 2 + turnCount % 2;
        numberTxt.text = $"{playerTurnCount}";
        
        image.DOFade(0.8f, 0.3f).ChangeStartValue(new Color(1.0f, 1.0f, 1.0f, 0.0f)).SetEase(Ease.Linear);
        StartCoroutine(CorPlayerTurnCanvas(1f));
    }

    IEnumerator CorPlayerTurnCanvas(float delay)
    {
        yield return new WaitForSeconds(delay);
        Core.UIManager.CloseUI<UIPlayerTurnCanvas>();
        Core.UIManager.OpenUI<UIBattleCanvas>();
        Core.EventManager.Publish(new ShowEndTurnCanvasUIEvent(UnitType.PlayableUnit));
    }
}
