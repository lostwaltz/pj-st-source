using System;
using System.Collections.Generic;
using UnityEngine;
using EnumTypes;
using Refactor;

public class GameManager : Singleton<GameManager>
{
    public PlayerInput Input { get; private set; }
    public PlayerInteraction Interaction { get; private set; }
    public InteractionMediator Mediator { get; private set; }
    
    public GameIndicator Indicator { get; private set; }
    public GameCommandController CommandController { get; private set; }
    public GameCommandUndoHandler CommandUndoHandler { get; private set; }
    public GamePhaseMachine PhaseMachine { get; private set; }
    public AutomaticUnitController AutoController { get; private set; }
    public TimeScaleHandler TimeScaleHandler { get; private set; }

    public GamePhase currentPhase;
    public GamePhase nextPhase;
    public GameResult result;

    public Action ToggleAutoBtn;
    public Action OnGameExit;

    // 페이즈가 시작할 때 호출하는 델리게이트
    // 게임 매니저는 Start부터 시작하므로 구독자들은 Awake에서 미리 구독해둘 것
    Dictionary<GamePhase, Action> OnPhaseEntered = new();

    private void Start()
    {
        // 임시 할당
        InitializeEffectPools();
        PhaseMachine.ChangePhase(PhaseMachine.StartPhase);
    }

    public void Initialize()
    {
        Input = TryGet<PlayerInput>();
        Interaction = TryGet<PlayerInteraction>();
        Indicator = TryGet<GameIndicator>();
        CommandController = TryGet<GameCommandController>();
        CommandUndoHandler = TryGet<GameCommandUndoHandler>();

        PhaseMachine = new GamePhaseMachine();
        AutoController = new AutomaticUnitController();
        Mediator = new InteractionMediator();
        TimeScaleHandler = new TimeScaleHandler();

        StageManager stageManager = StageManager.Instance;
        stageManager.Initialize();
        if (stageManager.stageData.useGuide)
        {
            Core.UIManager.OpenUI<UIGuide>();
            SubscribePhaseEvent(GamePhase.Start,
                () => { Core.EventManager.Publish(new GuidePopUpEvent(GuideTrigger.OnStart)); }
            );
            SubscribePhaseEvent(GamePhase.PlayerTurn,
                () => { Core.EventManager.Publish(new GuidePopUpEvent(GuideTrigger.OnPlayerTurnStart)); }
            );
        }
    }

    T TryGet<T>() where T : MonoBehaviour
    {
        if (TryGetComponent(out T comp))
            comp.enabled = true;
        else
            comp = gameObject.AddComponent<T>();

        return comp;
    }

    public void SetGamePhase(GamePhase phase)
    {
        currentPhase = phase;

        if (OnPhaseEntered.ContainsKey(phase))
            OnPhaseEntered[phase]?.Invoke();
    }

    public void SubscribePhaseEvent(GamePhase phase, Action onPhaseEntered)
    {
        if (!OnPhaseEntered.ContainsKey(phase))
            OnPhaseEntered.Add(phase, null);

        OnPhaseEntered[phase] += onPhaseEntered;
    }

    public void UnsubscribePhaseEvent(GamePhase phase, Action onPhaseEntered)
    {
        if (OnPhaseEntered.ContainsKey(phase))
            OnPhaseEntered[phase] -= onPhaseEntered;
    }


    public void BackToLobby()
    {
        GameUnitManager.Instance.ReleaseGameScene();
        Time.timeScale = 1f;
        ResetGameState();
        
        Core.SceneLoadManager.LoadScene("LobbyScene");
    }

    public void ReleaseGameScene()
    {
        GameUnitManager.Instance.ReleaseGameScene();
        
        ResetGameState();

    }

    private void ResetGameState()
    {
        // 초기화 할게 너무 많다
    }


    /// <summary>
    /// 임시 데미지hud 풀을 게임매니저에서 생성해놨음
    /// </summary>
    private void InitializeEffectPools()
    {
        Debug.Log("Initializing Effect Pools...");
        try
        {
            CreatePoolFromResource("DamageHUD", "Prefabs/DamegeHUD", 20, 50);
        }
        catch (Exception e)
        {
            Debug.LogError($"DamageHUD 풀 생성 실패: {e.Message}");
        }
    }

    private void CreatePoolFromResource(string poolKey, string resourcePath, int initialSize, int maxSize)
    {
        var prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null)
        {
            Debug.LogError($"프리팹을 찾을 수 없음: {resourcePath}");
            return;
        }

        var damageHUD = prefab.GetComponent<DamageHUD>();
        if (damageHUD == null)
        {
            Debug.LogError($"DamageHUD 컴포넌트를 찾을 수 없음: {resourcePath}");
            return;
        }

        Core.ObjectPoolManager.CreateNewPool(poolKey, damageHUD, initialSize, maxSize);
        Debug.Log($"Pool created for {poolKey}");
    }

}