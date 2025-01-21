using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LobbyScene : SceneBase
{
    public override void OnEnter()
    {
        Core.UIManager.OpenUI<UILobbyMain>();
        
        // if (Core.StageList != null && Core.StageList.stages != null)
        // {
        //     Core.LoadStageClearData();
        // }
        
        OnEnterEvent?.Invoke();

        OnEnterEvent = null;
    }

    public override void OnExit()
    {
        DOTween.KillAll();
    }

    public override IEnumerator OnLoadAssets()
    {
        ResourceRequest bgmLoadRequest = Resources.LoadAsync<AudioClip>(Constants.Sound.BGM_PATH + "BGM_MainMenu#2 (BGM_MainMenu)");

        while (!bgmLoadRequest.isDone)
        {
            yield return null;
        }

        var data = new SoundData
        {
            Clip = bgmLoadRequest.asset as AudioClip,
            Volume = 0.005f,
            Pitch = 1f,
            FrequentSound = false,
            Loop = true
        };

        BGM.LoadMainBGM(data);
    }
}
