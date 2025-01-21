using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager
{
    private SceneDataContainer dataContainer;
    private Core core;
    private Coroutine coroutine;

    public static string CurrentScene { get; private set; }
    public static string PrevScene { get; private set; }
    public static string NextScene { get; private set; }
    public string SceneName { get; private set; }
    
    public readonly Dictionary<string, SceneBase> sceneDic = new();

    public Action<float> OnLoadingProgressUpdated = null;
    public event Action OnSceneLoadComplete;

    public void Init()
    {
        core = Core.Instance;

        dataContainer = Resources.Load<SceneDataContainer>(Utils.Str.Clear().Append(Path.DataPath).Append("Scene/SceneData").ToString());

        if (dataContainer == null) return;

        foreach (var data in dataContainer.sceneDataList)
        {
            Type type = Type.GetType(data.sceneName);

            if (type == null || !typeof(SceneBase).IsAssignableFrom(type))
            {
                Debug.LogError($"Can't find scene type {data.sceneName}");
                continue;
            }

            MethodInfo method = typeof(SceneFactory).GetMethod("CreateFactory")?.MakeGenericMethod(type);

            if (method == null) return;

            sceneDic.Add(data.sceneName, (SceneBase)method.Invoke(null, null));
        }

        CurrentScene = SceneManager.GetActiveScene().name;

        sceneDic.TryGetValue(CurrentScene, out var scene);
        scene?.OnEnter();
    }

    public void LoadScene(string sceneName, string stageSceneName = null)
    {
        NextScene = sceneName;
        SceneName = stageSceneName ?? sceneName;

        if (null != coroutine) return;

        SceneManager.LoadScene("LoadingScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "LoadingScene") return;

        coroutine = core.StartCoroutine(LoadScene());
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        Coroutine loadAsset = null;

        loadAsset = Core.Instance.StartCoroutine(sceneDic[NextScene].OnLoadAssets());
            
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName); // SceneManager.LoadSceneAsync(NextScene);
        if (op != null) op.allowSceneActivation = false;

        while (op is { isDone: false })
        {
            yield return null;

            OnLoadingProgressUpdated?.Invoke(op.progress);

            if (op.progress < 0.9f)
                continue;

            op.allowSceneActivation = true;
            
            if (loadAsset != null)
                yield return loadAsset;
            
            break;
        }

        SceneManager.sceneLoaded += OnSceneActivated;

        PrevScene = CurrentScene;
        CurrentScene = NextScene;

        sceneDic[PrevScene].OnExit();

        coroutine = null;
    }

    private void OnSceneActivated(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != SceneName) return; // if (scene.name != CurrentScene) return;

        sceneDic[CurrentScene]?.OnEnter();
        OnSceneLoadComplete?.Invoke();
        SceneManager.sceneLoaded -= OnSceneActivated;
    }
}
