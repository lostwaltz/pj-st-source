using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShop
{
    private static SoundData gachaChangeSceneSound;
    private static SoundData changeGachaSound;


    public static void PlayGachaChangeScene()
    {
        if (gachaChangeSceneSound == null)
        {
            gachaChangeSceneSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#89 (UIsys_Gacha_ChangeScene_Up)"),
                Volume = 1.0f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(gachaChangeSceneSound);
    }
    public static void PlayChangeGacha()
    {
        if (changeGachaSound == null)
        {
            changeGachaSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#327 (UI_System_Shop_ChangeScene)"),
                Volume = 0.1f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(changeGachaSound);
    }

}