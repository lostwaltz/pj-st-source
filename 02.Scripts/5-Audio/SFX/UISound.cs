using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISound
{
    private static SoundData lobbyMainSound;
    private static SoundData lobbyStartButtonClickSound;
    private static SoundData backButtonClickSound;
    private static SoundData stageSelectUIStartSound;
    private static SoundData stageNodeClickSound;
    private static SoundData stageInfoUIStartSound;
    private static SoundData stageBattleStartSound;
    private static SoundData characterMaintanceUISound;
    private static SoundData unitListOpenSound;
    private static SoundData errorSound;
    private static SoundData changeSceneSound;
    private static SoundData lobbySceneSelectSound;
    private static SoundData mainClickNormalSound;

    private static SoundData InterfaceOpenSound;
    private static SoundData WindowCloseSound;

    private static SoundData SpineLevelUpSound1;
    private static SoundData SpineLevelUpSound2;
    private static SoundData SpineLevelUpSound3;
    private static SoundData SpineLevelUpSound4;
    private static SoundData SpineLevelUpSound5;
    private static SoundData SpineLevelUpSound6;

    private static SoundData LevelUpSound;


    public static void PlayLobbyMain()
    {
        if (lobbyMainSound == null)
        {
            lobbyMainSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#148 (UI_System_Main_SFX_MainLobby)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(lobbyMainSound);
    }
    public static void PlayLobbyStartButtonClick()
    {
        if (lobbyStartButtonClickSound == null)
        {
            lobbyStartButtonClickSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#237 (UI_LobbyMain_Start)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(lobbyStartButtonClickSound);
    }
    public static void PlayCharacterMaintanceUI()
    {
        if (characterMaintanceUISound == null)
        {
            characterMaintanceUISound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#305 (UI_System_ServR_SFX_InterfaceFadeIn)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(characterMaintanceUISound);
    }

    public static void PlayBackButtonClick()
    {
        if (backButtonClickSound == null)
        {
            backButtonClickSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#152 (Back)"),
                Volume = 1.0f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(backButtonClickSound);
    }
    public static void PlayStageSelectUIStart()
    {
        if (stageSelectUIStartSound == null)
        {
            stageSelectUIStartSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#330 (UI_Chapter1)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(stageSelectUIStartSound);
    }
    public static void PlayStageNodeClick()
    {
        if (stageNodeClickSound == null)
        {
            stageNodeClickSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#116 (UI_Click_StageNode)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(stageNodeClickSound);
    }
    public static void PlayStageInfoUIStart()
    {
        if (stageInfoUIStartSound == null)
        {
            stageInfoUIStartSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#124 (UI_StageInfo)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(stageInfoUIStartSound);
    }
    public static void PlayStageBattleStart()
    {
        if (stageBattleStartSound == null)
        {
            stageBattleStartSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#170 (UI_System_Battle_SFX_Click)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }

        Core.SoundManager.CreateSoundBuilder().Play(stageBattleStartSound);
    }
    public static void PlayUnitListOpen()
    {
        if (unitListOpenSound == null)
        {
            unitListOpenSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#158 (UI_System_Main_SFX_ListOpen)"),
                Volume = 1.0f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(unitListOpenSound);
    }
    public static void PlayError()
    {
        if (errorSound == null)
        {
            errorSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#151 (UI_System_Main_SFX_Error)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(errorSound);
    }
    public static void PlayChangeScene()
    {
        if (changeSceneSound == null)
        {
            changeSceneSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#150 (UI_System_Main_SFX_ChangeScene,SettingWindow)"),
                Volume = 1.0f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(changeSceneSound);
    }
    public static void PlayLobbySceneSelect()
    {
        if (lobbySceneSelectSound == null)
        {
            lobbySceneSelectSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#153 (UI_System_Main_Click_StageSelect)"),
                Volume = 1.0f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(lobbySceneSelectSound);
    }
    public static void PlayMainClickNormal()
    {
        if (mainClickNormalSound == null)
        {
            mainClickNormalSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#238 (UI_System_Main_Click_Normal_v2)"),
                Volume = 1.0f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(mainClickNormalSound);
    }

    public static void PlayInterfaceOpen()
    {
        if (InterfaceOpenSound == null)
        {
            InterfaceOpenSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#305 (UI_System_ServR_SFX_InterfaceFadeIn)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(InterfaceOpenSound);
    }
    public static void PlayWindowClose()
    {
        if (WindowCloseSound == null)
        {
            WindowCloseSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#127 (UISys_Main_C_WindowClose_Normal; UISys_Main_C_WindowClose_Other; UI_System_Main_SFX_WindowClose)"),
                Volume = 0.5f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        Core.SoundManager.CreateSoundBuilder().Play(WindowCloseSound);
    }


    public static void PlaySpineLevelUp(int level)
    {
        var sound = level switch
        {
            1 => GetSpineLevelUpSound1(level),
            2 => GetSpineLevelUpSound2(level),
            3 => GetSpineLevelUpSound3(level),
            4 => GetSpineLevelUpSound4(level),
            5 => GetSpineLevelUpSound5(level),
            6 => GetSpineLevelUpSound6(level),
            _ => GetSpineLevelUpSound1(level)
        };

        Core.SoundManager.CreateSoundBuilder().Play(sound);
    }

    private static SoundData GetSpineLevelUpSound1(int level)
    {
        if (SpineLevelUpSound1 == null)
        {
            SpineLevelUpSound1 = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#318 (UI_System_ServR_SFX_WeaponUpgrade_Count_01)"),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        return SpineLevelUpSound1;
    }

    private static SoundData GetSpineLevelUpSound2(int level)
    {
        if (SpineLevelUpSound2 == null)
        {
            SpineLevelUpSound2 = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#319 (UI_System_ServR_SFX_WeaponUpgrade_Count_01)"),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        return SpineLevelUpSound2;
    }

    private static SoundData GetSpineLevelUpSound3(int level)
    {
        if (SpineLevelUpSound3 == null)
        {
            SpineLevelUpSound3 = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#320 (UI_System_ServR_SFX_WeaponUpgrade_Count_01)"),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        return SpineLevelUpSound3;
    }

    private static SoundData GetSpineLevelUpSound4(int level)
    {
        if (SpineLevelUpSound4 == null)
        {
            SpineLevelUpSound4 = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#321 (UI_System_ServR_SFX_WeaponUpgrade_Count_01)"),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        return SpineLevelUpSound4;
    }

    private static SoundData GetSpineLevelUpSound5(int level)
    {
        if (SpineLevelUpSound5 == null)
        {
            SpineLevelUpSound5 = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#322 (UI_System_ServR_SFX_WeaponUpgrade_Count_01)"),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        return SpineLevelUpSound5;
    }

    private static SoundData GetSpineLevelUpSound6(int level)
    {
        if (SpineLevelUpSound6 == null)
        {
            SpineLevelUpSound6 = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System#323 (UI_System_ServR_SFX_WeaponUpgrade_Count_01)"),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        return SpineLevelUpSound6;
    }

    public static void PlayLevelUp()
    {
        Core.SoundManager.CreateSoundBuilder().Play(GetLevelUpSound());
    }

    private static SoundData GetLevelUpSound()
    {
        if (LevelUpSound == null)
        {
            LevelUpSound = new SoundData
            {
                Clip = Resources.Load<AudioClip>(Constants.Sound.UISYSTEM_PATH + "UI_System [132] LevelUP"),
                Volume = 0.3f,
                Pitch = 1f,
                FrequentSound = true
            };
        }
        return LevelUpSound;
    }
}