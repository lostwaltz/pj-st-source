
public class UIInGameManager : Singleton<UIInGameManager>
{
    public UIBattleCanvas uiBattleCanvas;
    private UIPlayerTurnCanvas uiPlayerTurnCanvas;
    private UIEnemyTurnCanvas uiEnemyTurnCanvas;
    private UIObstacleCanvas uiObstacleCanvas;
    //private UIReselectionCanvas uiReselectionCanvas;
    private UIBattleSetting uiBattleSetting;
    
    
    public void Init()
    {
        uiPlayerTurnCanvas = Core.UIManager.GetUI<UIPlayerTurnCanvas>();
        uiPlayerTurnCanvas.Open();
        uiPlayerTurnCanvas.Init();

        uiEnemyTurnCanvas = Core.UIManager.GetUI<UIEnemyTurnCanvas>();
        uiEnemyTurnCanvas.Close();
        uiEnemyTurnCanvas.Init();

        uiObstacleCanvas = Core.UIManager.GetUI<UIObstacleCanvas>();
        uiObstacleCanvas.Close();
        uiObstacleCanvas.Init();

        uiBattleSetting = Core.UIManager.GetUI<UIBattleSetting>();
        uiBattleSetting.Init();
    }
}
