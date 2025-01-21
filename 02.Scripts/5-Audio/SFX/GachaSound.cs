using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaSound
{
    private static SoundData gachaShowResultCommonSound;
    private static SoundData gachaShowResultRSound;
    private static SoundData gachaShowResultSRSound;
    private static SoundData gachaShowResultSSRSound;
    private static SoundData gachaShowCharacterRSound;
    private static SoundData gachaShowCharacterSRSound;
    private static SoundData gachaShowCharacterSSRSound;
    private static SoundData gachaStartSound;
    private static SoundData gachaExitSound;

    public static void PlayGachaShowResultCommon()
    {
        if (gachaShowResultCommonSound == null)
        {
            gachaShowResultCommonSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#21 (UI_Gacha_Show_Result2)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaShowResultCommonSound);
    }
    public static void PlayGachaShowResultR()
    {
        if (gachaShowResultRSound == null)
        {
            gachaShowResultRSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#1 (UI_Gacha_Show_Result_R)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(gachaShowResultRSound);
    }
    public static void PlayGachaShowResultSR()
    {
        if (gachaShowResultSRSound == null)
        {
            gachaShowResultSRSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#2 (UI_Gacha_Show_Result_SR)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaShowResultSRSound);
    }
    public static void PlayGachaShowResultSSR()
    {
        if (gachaShowResultSSRSound == null)
        {
            gachaShowResultSSRSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#3 (UI_Gacha_Show_Result_SSR)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaShowResultSSRSound);
    }

    public static void PlayGachaShowCharacterR()
    {
        if (gachaShowCharacterRSound == null)
        {
            gachaShowCharacterRSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#4 (UI_Gacha_Show_Char_R)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaShowCharacterRSound);
    }
    public static void PlayGachaShowCharacterSR()
    {
        if (gachaShowCharacterSRSound == null)
        {
            gachaShowCharacterSRSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#5 (UI_Gacha_Show_Char_SR)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaShowCharacterSRSound);
    }
    public static void PlayGachaShowCharacterSSR()
    {
        if (gachaShowCharacterSSRSound == null)
        {
            gachaShowCharacterSSRSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#6 (UI_Gacha_Show_Char_SSR)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaShowCharacterSSRSound);
    }

    public static void PlayGachaStart()
    {
        if (gachaStartSound == null)
        {
            gachaStartSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#19 (UI_Gacha_Star_S)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaStartSound);
    }
    public static void PlayGachaExit()
    {
        if (gachaExitSound == null)
        {
            gachaExitSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.GACHASOUND_PATH + "UI_Gacha#20 (UI_Gacha_Star_N)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(gachaExitSound);
    }
}

