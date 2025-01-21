using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNemesis
{
    private static SoundData nemesisAttackReload;

    public static void PlayNemesisAttackReload()
    {
        if (nemesisAttackReload == null)
        {
            nemesisAttackReload = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.Character.NEMESIS + "C_SR01_Nemesis#7 (C_SR01_Nemesis_Atk_Load)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(nemesisAttackReload);
    }
}

