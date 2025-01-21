using System;
using UnityEngine;

public class Core : SingletonDontDestroy<Core>
{
    public Sprite customSprite;

    public static DataManager DataManager { get; private set; }
    public static UIManager UIManager { get; private set; }
    public static CommonUIManager CommonUIManager { get; private set; }

    public static SceneLoadManager SceneLoadManager { get; private set; }
    public static ObjectPoolManager ObjectPoolManager { get; private set; }
    public static SoundManager SoundManager { get; private set; }
    public static UnitManager UnitManager { get; private set; }
    public static InputManager InputManager { get; private set; }
    public static EventManager EventManager { get; private set; }
    public static CurrencyManager CurrencyManager { get; private set; }
    public static UGSManager UGSManager { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void OnRuntimeMethodLoad()
    {
        Debug.Log("Core OnRuntimeMethodLoad");

#if !UNITY_EDITOR
        Instance.InitSetting();
#endif
    }

    protected override void Awake()
    {
        base.Awake();

        Debug.Log("Core Awake");
        Application.targetFrameRate = 60;

#if UNITY_EDITOR
        Instance.InitSetting();
#endif
    }

    private void Start()
    {
        Debug.Log("Core Start");
    }

    private void InitSetting()
    {
        Debug.Log("Init Setting");
        if (EventManager != null)
            return;

        EventManager = new EventManager();
        UIManager = new UIManager();
        CommonUIManager = new CommonUIManager();
        DataManager = new DataManager();
        SceneLoadManager = new SceneLoadManager();
        ObjectPoolManager = new ObjectPoolManager();
        SoundManager = new SoundManager();
        UnitManager = new UnitManager();
        InputManager = new InputManager();
        CurrencyManager = new CurrencyManager();
        UGSManager = new UGSManager();

        UIManager.Init();
        CommonUIManager.Init();
        UGSManager.Init(); // 유관 매니저들이 UGS 이벤트가 필요할 수 있어서 맨위에 생성

        ObjectPoolManager.Init();
        DataManager.Init();
        SoundManager.Init(true, 50, 50, 50);
        InputManager.Init();

        CurrencyManager.Init();
        UnitManager.Init();

        SceneLoadManager.Init();
        
        TutorialManager.Instance.InitTutorial();
    }

}
