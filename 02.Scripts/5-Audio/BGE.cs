using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGE : MonoBehaviour
{
    private static SoundData mainMenuBGE;
    private static SoundData battleBGE;

    public static void PlayMainMenuBGE()
    {
        if (mainMenuBGE == null)
        {
            mainMenuBGE = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGE_PATH + "BGE_CommandVehicle (BGE_CommandVehicle)"),
                Volume = 0.1f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(mainMenuBGE);
    }
    public static void PlayBattleBGE()
    {
        if (battleBGE == null)
        {
            battleBGE = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.BGE_PATH + "BGE_Battle_City#1 (BGE_Battle_City)"),
                Volume = 0.1f,
                Pitch = 1f,
                FrequentSound = false,
                Loop = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(battleBGE);
    }

    public static void StopCurrentBGE()
    {
        if (mainMenuBGE != null)
        {
            Core.SoundManager.StopSound(mainMenuBGE);
            mainMenuBGE = null;
        }
        if (battleBGE != null)
        {
            Core.SoundManager.StopSound(battleBGE);
            battleBGE = null;
        }
    }

}
