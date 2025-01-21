using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : SingletonDontDestroy<TutorialManager>, IClickable
{
    public bool IsCompleteTutorial = false;

    private readonly Dictionary<Type, Tutorial> tutorials = new();

    public BoxCollider boxCollider;
    
    private Tutorial currentTutorial;
    public void InitTutorial()
    {
        Core.SceneLoadManager.sceneDic["LobbyScene"].OnEnterEvent += CheckTutorial; 
        return;
        IsCompleteTutorial = PlayerPrefs.GetInt(nameof(IsCompleteTutorial)) == 1;
        
        if(IsCompleteTutorial) 
            return;
        
        tutorials.Add(typeof(HowToPlayingGame), new HowToPlayingGame());
        
        Core.SceneLoadManager.sceneDic["LobbyScene"].OnEnterEvent += StartTutorial;
        
        gameObject.layer = LayerMask.NameToLayer("TutorialObject");
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    void CheckTutorial()
    {
        IsCompleteTutorial = Core.DataManager.StageClearData[-1];
        if (!IsCompleteTutorial)
        {
            tutorials.Add(typeof(HowToPlayingGame), new HowToPlayingGame());
            
            gameObject.layer = LayerMask.NameToLayer("TutorialObject");
            boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.enabled = false;

            StartTutorial();
        }
    }

    private void StartTutorial()
    {
        Core.SceneLoadManager.sceneDic["LobbyScene"].OnEnterEvent -= StartTutorial;
        
        Core.SceneLoadManager.LoadScene("GameScene", Core.DataManager.SelectedStage.sceneName);
        
        Core.SceneLoadManager.sceneDic["GameScene"].OnEnterEvent += EnterGameScene;
    }

    private void EnterGameScene()
    {
        Core.SceneLoadManager.sceneDic["GameScene"].OnEnterEvent -= EnterGameScene;
        
        ChangeTutorial(typeof(HowToPlayingGame));
    }

    public void ChangeTutorial(Type type)
    {
        currentTutorial?.Exit();
        
        tutorials.TryGetValue(type, out Tutorial tutorial);
        
        currentTutorial = tutorial;

        currentTutorial?.Enter();
    }

    private void Update()
    {
        currentTutorial?.Execute();
    }

    public string GetInfo()
    {
        return "";
    }
}
