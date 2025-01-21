using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//TODO: Musicmanager 코어에 초기화 필요 , 볼륨이 1로 됌 
public class BGM : MonoBehaviour
{
    private static BGM instance;
    private static Coroutine fadeOutCoroutine;
    private static SoundData mainMenuBGM;
    private static SoundData battleBGM;
    private static SoundData battleIntroBGM;
    private static SoundData battleWinBGM;
    private static SoundData battleLoseBGM;
    private static SoundData gachaShopBGM;
    private static SoundData maintenanceRoomBGM;
    private static SoundData titleBGM;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public static void PlayBGMAfterFadeOut(Action playNewBGM)
    {
        if (fadeOutCoroutine != null)
        {
            instance.StopCoroutine(fadeOutCoroutine);
        }

        fadeOutCoroutine = instance.StartCoroutine(FadeOutAndPlayRoutine(playNewBGM));
    }
    private static IEnumerator FadeOutAndPlayRoutine(Action playNewBGM)
    {
        MusicManager.Instance.FadeOutStop();
        yield return new WaitForSeconds(1.0f);

        playNewBGM?.Invoke();
        fadeOutCoroutine = null;
    }



    public static void PlayMainMenuBGM()
    {
        if (mainMenuBGM == null)
        {
            mainMenuBGM = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGM_PATH + "BGM_MainMenu#2 (BGM_MainMenu)"),
                Volume = 0.005f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = true
            };
        }
        MusicManager.Instance.Play(mainMenuBGM.Clip);
    }

    public static void LoadMainBGM(SoundData bmg)
    {
        mainMenuBGM = bmg;
    }
    
    public static void StopMainMenuBGM()
    {
        MusicManager.Instance.FadeOutStop();
    }

    public static void PlayBattleBGM()
    {
        if (battleBGM == null)
        {
            battleBGM = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGM_PATH + "BGM_Battle_YellowField#1 (BGM_Battle_YellowField)"),
                Volume = 0.005f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = true
            };
        }
        MusicManager.Instance.Play(battleBGM.Clip);
    }

    public static void PlayBattleIntroBGM()
    {
        if (battleIntroBGM == null)
        {
            battleIntroBGM = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGM_PATH + "BGM_Battle_YellowField#2 (BGM_Battle_YellowField)"),
                Volume = 0.005f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = false
            };
        }
        MusicManager.Instance.Play(battleIntroBGM.Clip);
    }

    public static void StopBattleIntroBGM()
    {
        MusicManager.Instance.Stop();
    }

    public static void StopBattleBGM()
    {
        MusicManager.Instance.Stop();
    }

    public static void PlayBattleWinBGM()
    {
        if (battleWinBGM == null)
        {
            battleWinBGM = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGM_PATH + "BGM_Win (BGM_Win)"),
                Volume = 0.005f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = false
            };
        }
        MusicManager.Instance.Play(battleWinBGM.Clip);
    }
    public static void PlayGachaShopBGM()
    {
        if (gachaShopBGM == null)
        {
            gachaShopBGM = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGM_PATH + "BGM_ShopGacha#1 (BGM_ShopGacha)"),
                Volume = 0.005f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = true
            };
        }
        MusicManager.Instance.Play(gachaShopBGM.Clip);
    }

    public static void PlayMaintenanceRoomBGM()
    {
        if (maintenanceRoomBGM == null)
        {
            maintenanceRoomBGM = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGM_PATH + "BGM_MainMenu#1 (BGM_MainMenuMaintenanceRoom)"),
                Volume = 0.005f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = true
            };
        }
        MusicManager.Instance.Play(maintenanceRoomBGM.Clip);
    }

    public static void PlayTitleBGM()
    {
        if (titleBGM == null)
        {
            titleBGM = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGM_PATH + "BGM_Title (BGM_Title)"),
                Volume = 0.005f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = true
            };
        }
        MusicManager.Instance.Play(titleBGM.Clip);
    }
}
